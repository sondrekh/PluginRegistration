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
    public class Plugin_tests : TestBase
    {
        public IRequest pluginRequest;
        public IRequest assemblyRequest;

        [TestInitialize]
        public async Task setup()
        {
            assemblyRequest = await SharedSetup.Assembly(crm, assemblyPath);
        }

        [TestCleanup]
        public async Task teardown()
        {
            var response = await crm.Delete(assemblyRequest);
            Console.WriteLine($"Deletion statuscode: {(int)response.StatusCode}");
        }

        [TestMethod]
        public async Task register_plugin()
        {
            // Arrange
            var plugin = ObjectExamples.Plugin(assemblyRequest.recordId);

            pluginRequest = new PluginRequest(plugin);

            // Act
            var response = await crm.Post(pluginRequest);

            // Assert
            Assert.AreEqual(204, (int)response.StatusCode);
        }

        [TestMethod]
        public async Task find_and_delete_unwanted_plugins()
        {
            // Arrange 
            var list = ObjectExamples.PluginList(assemblyRequest.recordId);
            var pluginRequestA = new PluginRequest(list[0]);
            var pluginRequestB = new PluginRequest(list[1]);
            await crm.Post(pluginRequestA);
            await crm.Post(pluginRequestB);

            var wantedPlugins = new List<PluginType>();
            wantedPlugins.Add(list.First());

            // Act
            List<PluginType> plugins = await PluginHelper.FindUnwantedPlugins(crm, wantedPlugins);
            var pluginDeletionResponses = await Registration.DeleteRecords(crm, plugins);

            // Assert 
            Assert.AreEqual(1, plugins.Count);
            Assert.AreEqual("Plugin.DummyPlugin", plugins.First().TypeName);
            Assert.AreEqual(204, pluginDeletionResponses.First().statusCode);
        }

        [TestMethod]
        public async Task upsert_plugin_not_already_existing()
        {
            // Arrange
            var plugin = ObjectExamples.Plugin(assemblyRequest.recordId);

            // Act
            RecordResponse response = await PluginHelper.UpsertPlugin(crm, plugin);

            // Assert
            Assert.AreEqual(204, response.statusCode);
            Assert.IsFalse(string.IsNullOrEmpty(response.recordId));
        }
    }

    [TestClass]
    public class Plugins_with_simple_setup
    {
        List<PluginType> list;
        string guid;
        PluginType plugin;

        [TestInitialize]
        public void setup()
        {
            guid = Guid.NewGuid().ToString();
            list = ObjectExamples.PluginList(guid);
            plugin = new PluginType().WithAssemblyId(guid);
        }

        [TestMethod]
        public void plugin_with_name_is_in_list()
        {
            // Arrange
            plugin.TypeName = "Plugin.BasicPlugin";

            // Act 
            bool isInList = list.Contains(plugin);

            // Assert
            Assert.IsTrue(isInList);
        }

        [TestMethod]
        public void plugin_with_name_is_not_in_list()
        {
            // Arrange
            plugin.TypeName = "Plugin.NonExisting";

            // Act 
            var isInList = list.Contains(plugin);

            // Assert
            Assert.IsFalse(isInList);
        }

        [TestMethod]
        public void add_pluginid_to_steps()
        {
            // Arrange
            var pluginId = "abc";
            var plugin = new PluginType();
            var step = new SdkMessageProcessingStep();
            plugin.Steps.Add(step);

            // Act
            plugin.AddPluginIdToSteps(pluginId);

            // Assert
            Assert.AreEqual(pluginId, plugin.Steps.First().PluginTypeId);
        }
    }
}