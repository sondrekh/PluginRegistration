using Dynamics.Basic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PluginRegistration.Helpers;
using PluginRegistration.Interfaces;
using PluginRegistration.Models;
using System;
using System.Threading.Tasks;
using TestPluginRegistration.Setup;

namespace TestPluginRegistration.Image
{
    [TestClass]
    public class ImageSharedSetup : TestBase
    {
        public IRequest imageRequest { get; set; }
        public IRequest stepRequest { get; set; }
        public IRequest pluginRequest { get; set; }
        public IRequest assemblyRequest { get; set; }
        public SdkMessageProcessingStepImage image { get; set; }

        [TestInitialize]
        public async Task Register_assembly_plugin_and_step()
        {
            assemblyRequest = await SharedSetup.Assembly(crm, assemblyPath);
            pluginRequest = await SharedSetup.Plugin(crm, assemblyRequest.recordId);
            stepRequest = await SharedSetup.Step(crm, pluginRequest.recordId);
            image = ObjectExamples.Image(stepRequest.recordId);
            imageRequest = new ImageRequest(image);
        }

        [TestCleanup]
        public async Task teardown()
        {
            try
            {
                var image = await crm.Delete(imageRequest);
                Console.WriteLine((int)image.StatusCode);
            }
            catch (Exception)
            {
                Console.WriteLine("Image not found");
            }


            var step = await crm.Delete(stepRequest);
            Console.WriteLine((int)step.StatusCode);

            var plugin = await crm.Delete(pluginRequest);
            Console.WriteLine((int)plugin.StatusCode);

            await AssemblyHelper.DeleteAssembly(crm, assemblyRequest.recordId);
        }
    }
}
