using Dynamics.Basic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using TestPluginRegistration.Setup;

namespace TestPluginRegistration
{
    [TestClass]
    public class TestD365Connection : TestBase
    {
        [TestMethod]
        public async Task lab_connection()
        {
            // Arrange
            IRequest request = new Request().EntityName("WhoAmI");

            // Act
            var response = await crm.GetList(request);

            // Assert
            Assert.AreEqual(200, (int)response.StatusCode);
        }
    }
}
