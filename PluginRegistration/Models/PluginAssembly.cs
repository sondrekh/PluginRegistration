using Dynamics.Basic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;

namespace PluginRegistration.Models
{
    public class PluginAssembly
    {
        public string Name;
        public string Culture;
        public string Version;
        public string Publickeytoken;
        public string UnSetProperty;
        public int Sourcetype = 0;
        public int Isolationmode = 2;
        public string Content;

        [JsonProperty("pluginassemblyid")]
        public string Id { get; set; }

        public PluginAssembly(string path)
        {
            Load(path);
        }

        public PluginAssembly()
        {

        }

        public void Load(string path)
        {
            var assembly = System.Reflection.Assembly.LoadFile(path);
            string[] props = assembly.GetName().FullName.Split(",=".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            Name = props[0];
            Culture = props[4];
            Version = props[2];
            Publickeytoken = props[6];
            Content = Convert.ToBase64String(File.ReadAllBytes(path));
        }

        public string Serialize()
        {
            var json = new JObject();

            json.AddIfProvided("name", Name);
            json.AddIfProvided("culture", Culture);
            json.AddIfProvided("version", Version);
            json.AddIfProvided("publickeytoken", Publickeytoken);
            json.AddIfProvided("sourcetype", Sourcetype);
            json.AddIfProvided("isolationmode", Isolationmode);
            json.AddIfProvided("content", Content);

            return json.ToString();
        }
    }
}