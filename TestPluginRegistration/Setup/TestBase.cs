using Dynamics.Basic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Threading.Tasks;

namespace TestPluginRegistration.Setup
{
    [TestClass]
    public class TestBase
    {
        public TestContext TestContext { get; set; }
        public Crm crm { get; set; }
        public string assemblyPath = @"C:\Users\SondreKværneHansen\Documents\GitHub\Programmatisk registrering av plugins\Plugin registration\Plugin\bin\Debug\Plugin.dll";

        [TestInitialize]
        public async Task setup_crm_connection()
        {
            var authentication = CrmAuthentication.Get(ObjectExamples.PathToCredentials, TestContext);
            crm = new Crm(authentication);
            await crm.GetAccessToken();

            if (!File.Exists(assemblyPath))
                assemblyPath = (string)TestContext.Properties["assemblyPath"];
        }
    }
}
