using Microsoft.VisualStudio.TestTools.UnitTesting;
using PluginRegistration;
using PluginRegistration.Models;

namespace TestPluginRegistration
{
    [TestClass]
    public class TestRegistration
    {
        [TestMethod]
        public void Get_id_from_element_with_unknown_type()
        {
            // Arrange
            var plugin = new PluginType();
            plugin.Id = "abc";

            // Act
            string id = Registration.GetIdFromGenericObjectT<PluginType>(plugin);

            // Assert
            Assert.AreEqual(plugin.Id, id);
        }
    }
}
