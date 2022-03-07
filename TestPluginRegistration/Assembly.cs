using Dynamics.Basic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PluginRegistration;
using PluginRegistration.Helpers;
using PluginRegistration.Models;
using PluginRegistration.Requests;
using System.IO;
using System.Threading.Tasks;
using TestPluginRegistration.Setup;

namespace TestPluginRegistration
{
    [TestClass]
    public class Assembly_simple_setup : TestBase
    {
        [TestMethod]
        public void filepath_finds_plugin_dll()
        {
            // Act
            var fileExists = File.Exists(assemblyPath);

            // Assert
            Assert.IsTrue(fileExists);
        }

        [TestMethod]
        public void load_assembly_into_object()
        {
            // Act
            var assembly = new PluginAssembly();
            assembly.Load(assemblyPath);

            // Assert
            Assert.IsFalse(string.IsNullOrEmpty(assembly.Name));
            Assert.IsFalse(string.IsNullOrEmpty(assembly.Version));
            Assert.IsFalse(string.IsNullOrEmpty(assembly.Culture));
            Assert.IsFalse(string.IsNullOrEmpty(assembly.Publickeytoken));
            Assert.IsTrue(string.IsNullOrEmpty(assembly.UnSetProperty));
        }
    }

    [TestClass]
    public class Assembly_tests : TestBase
    {
        public IRequest assemblyRequest;

        [TestInitialize]
        public void setup()
        {
            var assembly = new PluginAssembly(assemblyPath);
            assemblyRequest = new AssemblyRequest(assembly);
        }

        [TestCleanup]
        public async Task teardown()
        {
            await AssemblyHelper.DeleteAssembly(crm, assemblyRequest.recordId);
        }

        [TestMethod]
        public async Task register_assembly()
        {
            // Act
            var response = await crm.Post(assemblyRequest);

            // Assert
            assemblyRequest.recordId = response.GetCreatedId();
            Assert.AreEqual(204, (int)response.StatusCode);
        }

        [TestMethod]
        public async Task register_assembly_short_example()
        {
            // Act
            var response = await AssemblyHelper.RegisterAssembly(crm, assemblyPath);

            // Assert 
            assemblyRequest.recordId = response.recordId;
            Assert.AreEqual(204, response.statusCode);
        }

        [TestMethod]
        public async Task upsert_assembly_not_existing()
        {
            // Act
            RecordResponse assemblyUpsert = await AssemblyHelper.UpsertAssemblyWithPath(crm, assemblyPath);

            // Assert
            assemblyRequest.recordId = assemblyUpsert.recordId;
            Assert.AreEqual(204, assemblyUpsert.statusCode);
            Assert.IsFalse(string.IsNullOrEmpty(assemblyUpsert.recordId));
        }
    }

    [TestClass]
    public class Assembly_update : TestBase
    {
        private string assemblyId;

        [TestInitialize]
        public async Task setup()
        {
            var response = await AssemblyHelper.UpsertAssemblyWithPath(crm, assemblyPath);
            assemblyId = response.recordId;
        }

        [TestCleanup]
        public async Task teardown()
        {
            await AssemblyHelper.DeleteAssembly(crm, assemblyId);
        }

        [TestMethod]
        public async Task upsert_existing_assembly()
        {
            // Act
            var assemblyUpsert = await AssemblyHelper.UpsertAssemblyWithPath(crm, assemblyPath);

            // Assert
            Assert.AreEqual(204, assemblyUpsert.statusCode);
            Assert.AreEqual(assemblyId, assemblyUpsert.recordId);
        }
    }
}

