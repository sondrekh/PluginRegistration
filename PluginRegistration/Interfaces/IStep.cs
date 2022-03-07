using PluginRegistration.Models;
using System.Collections.Generic;

namespace PluginRegistration.Interfaces
{
    public interface IStep
    {
        SdkMessageProcessingStep Step { get; set; }
        List<SdkMessageProcessingStepImage> Images { get; set; }
        IStep Setup(string pluginId);
    }
}
