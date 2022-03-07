using Dynamics.Basic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PluginRegistration;
using PluginRegistration.Helpers;
using PluginRegistration.Models;
using PluginRegistration.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestPluginRegistration.Setup;

namespace TestPluginRegistration
{
    [TestClass]
    public class Step_tests : TestBase
    {
        public IRequest assemblyRequest { get; set; }
        public IRequest pluginRequest { get; set; }
        public IRequest stepRequest { get; set; }
        public SdkMessageProcessingStep step { get; set; }

        [TestInitialize]
        public async Task Setup_assembly_plugin_and_step()
        {
            assemblyRequest = await SharedSetup.Assembly(crm, assemblyPath);
            pluginRequest = await SharedSetup.Plugin(crm, assemblyRequest.recordId);
            step = ObjectExamples.Step(pluginRequest.recordId);
            await step.ResolveIds(crm);
            stepRequest = new StepRequest(step);
        }

        [TestCleanup]
        public async Task teardown()
        {
            try
            {
                var stepResponse = await crm.Delete(stepRequest);
                Console.WriteLine($"Step delete: {(int)stepResponse.StatusCode}");
            }
            catch (Exception)
            {
                Console.WriteLine("No step found");
            }

            var plugin = await crm.Delete(pluginRequest);
            Console.WriteLine($"Plugin delete: {(int)plugin.StatusCode}");

            await AssemblyHelper.DeleteAssembly(crm, assemblyRequest.recordId);
        }

        [TestMethod]
        public async Task upsert_plugin_already_existing()
        {
            // Arrange
            var plugin = ObjectExamples.Plugin(assemblyRequest.recordId);

            // Act
            var response = await PluginHelper.UpsertPlugin(crm, plugin);

            // Assert
            Assert.AreEqual(204, response.statusCode);
            Assert.IsFalse(string.IsNullOrEmpty(response.recordId));
            pluginRequest.recordId = response.recordId;
        }

        [TestMethod]
        public async Task register_step()
        {
            // Act
            var response = await crm.Post(stepRequest);

            // Assert
            Console.WriteLine((int)response.StatusCode);
            stepRequest.recordId = response.GetCreatedId();
            Assert.AreEqual(204, (int)response.StatusCode);
        }

        [TestMethod]
        public async Task register_step_resolve_filter_and_messageid_on_create()
        {
            // Arrange
            var step = new SdkMessageProcessingStep()
            {
                PluginTypeId = pluginRequest.recordId,
                Message = "update",
                Entity = "contact",
                Mode = (int)Mode.Async,
                Stage = (int)Stage.postOperation,
                FilteringAttributes = "firstname,lastname"
            };

            // Act
            await step.ResolveIds(crm);

            // Assert
            Assert.IsFalse(string.IsNullOrEmpty(step.SdkMessageId));
            Assert.IsFalse(string.IsNullOrEmpty(step.SdkMessageFilterId));
        }

        [TestMethod]
        public async Task upsert_step_not_existing()
        {
            // Act
            RecordResponse response = await StepHelper.UpsertStep(crm, step);

            // Assert
            Assert.AreEqual(204, response.statusCode);
            Assert.IsFalse(string.IsNullOrWhiteSpace(response.recordId));
            stepRequest.recordId = response.recordId;
        }

        [TestMethod]
        public async Task find_and_delete_unwanted_steps()
        {
            // Arrange
            var stepA = ObjectExamples.Step(pluginRequest.recordId);
            var stepB = ObjectExamples.Step(pluginRequest.recordId);
            stepA.Message = "update";
            await stepA.ResolveIds(crm);
            await stepB.ResolveIds(crm);

            var responseA = await crm.Post(new StepRequest(stepA));
            new RecordResponse(responseA, step.GetType());

            var responseB = await crm.Post(new StepRequest(stepB));
            new RecordResponse(responseB, step.GetType());

            stepRequest = new StepRequest(stepA);
            var requestB = new StepRequest(stepB);
            stepRequest.recordId = responseA.GetCreatedId();

            requestB.recordId = responseB.GetCreatedId();


            var wantedSteps = new List<SdkMessageProcessingStep>();
            wantedSteps.Add(stepA);

            // Act 
            List<SdkMessageProcessingStep> unwantedSteps = await StepHelper.FindUnwantedSteps(crm, wantedSteps);
            var deletedStepResponses = await Registration.DeleteRecords(crm, unwantedSteps);

            // Assert 
            Assert.AreEqual(stepB, unwantedSteps.First());
            Assert.AreEqual(204, deletedStepResponses.First().statusCode);
        }
    }
}
