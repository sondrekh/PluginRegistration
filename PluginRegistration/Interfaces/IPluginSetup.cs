using PluginRegistration.Models;
using System.Collections.Generic;

namespace PluginRegistration.Interfaces
{
    public interface IPluginSetup
    {
        List<SdkMessageProcessingStep> steps { get; set; }
        IPluginSetup Setup(string assemblyId);
        PluginType plugin { get; set; }
        void AddSteps(string pluginId);
    }
}
