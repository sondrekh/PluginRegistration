using PluginRegistration.Models;
using System.Collections.Generic;

namespace TestPluginRegistration.Setup
{
    public static class ObjectExamples
    {
        public static readonly string PathToCredentials = @"C:\Users\SondreKværneHansen\Documents\GitHub\Programmatisk registrering av plugins\Plugin registration\ids.json";

        public static SdkMessageProcessingStep Step(string pluginId)
        {
            var step = new SdkMessageProcessingStep()
            {
                Configuration = "configuration",
                InvocationSource = (int)InvocationSource.Parent,
                SupportedDeployment = 0,
                Mode = (int)Mode.Async,
                Stage = (int)Stage.postOperation,
                Message = "create",
                Entity = "contact",
                PluginTypeId = pluginId,
            };

            return step;
        }

        public static SdkMessageProcessingStepImage Image(string stepId)
        {
            var image = new SdkMessageProcessingStepImage()
            {
                SdkMessageProcessingStepId = stepId,
                Attributes = "firstname,lastname",
                MessagePropertyName = "Id",
                EntityAlias = "Image",
                ImageType = (int)ImageType.PostImage,
                Name = "Name"
            };

            return image;
        }

        public static SdkMessageFilter Filter(string messageId)
        {
            var filter = new SdkMessageFilter()
            {
                PrimaryObjectTypeCode = "contact",
                SdkMessageId = messageId
            };

            return filter;
        }

        public static PluginType Plugin(string assemblyId)
        {
            var plugin = new PluginType()
            {
                Name = "name",
                TypeName = "Plugin.BasicPlugin",
                Description = "description",
                FriendlyName = "friendlyName",
                PluginAssemblyId = assemblyId
            };

            return plugin;
        }

        public static List<SdkMessageProcessingStepImage> ImageList(string stepId)
        {
            var list = new List<SdkMessageProcessingStepImage>();
            var imageA = new SdkMessageProcessingStepImage()
            {
                SdkMessageProcessingStepId = stepId,
                Attributes = "firstname,lastname",
                MessagePropertyName = "Id",
                EntityAlias = "Image",
                ImageType = (int)ImageType.PostImage,
                Name = "Name A"
            };

            var imageB = new SdkMessageProcessingStepImage()
            {
                SdkMessageProcessingStepId = stepId,
                Attributes = "firstname,lastname",
                MessagePropertyName = "Id",
                EntityAlias = "Image",
                ImageType = (int)ImageType.PostImage,
                Name = "Name B"
            };

            list.Add(imageA);
            list.Add(imageB);

            return list;
        }

        public static List<PluginType> PluginList(string assemblyId)
        {
            var list = new List<PluginType>();
            var pluginA = new PluginType()
            {
                TypeName = "Plugin.BasicPlugin",
                PluginAssemblyId = assemblyId,
            };

            var pluginB = new PluginType()
            {
                TypeName = "Plugin.DummyPlugin",
                PluginAssemblyId = assemblyId
            };

            list.Add(pluginA);
            list.Add(pluginB);
            return list;
        }
    }
}
