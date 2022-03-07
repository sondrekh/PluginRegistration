using PluginRegistration.Models;
using System.Collections.Generic;

namespace PluginRegistration.Interfaces
{
    public interface IAssembly
    {
        PluginAssembly Assembly { get; set; }
        List<IPluginSetup> Plugins { get; set; }
        IAssembly Setup(string PathToAssembly);
        void AddAssemblyIdToPlugins(string assemblyId);
    }
}
