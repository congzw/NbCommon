using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.IO;
using System.Text;

namespace NbPilot.Common.Serialize
{
    public interface INbJsonSerialize
    {
        string Serialize(Object value, NbJsonSerializeConfig config = null);

        void SerializeFile(string filePath, Object value, NbJsonSerializeConfig config = null);

        T Deserialize<T>(string json, NbJsonSerializeConfig config = null);

        T DeserializeFile<T>(string filePath, NbJsonSerializeConfig config = null);
    }

    public class NbJsonSerialize : INbJsonSerialize
    {
        public string Serialize(object value, NbJsonSerializeConfig config = null)
        {
            if (config == null)
            {
                return JsonConvert.SerializeObject(value);
            }
            var settings = ConvertSettings(config);
            return JsonConvert.SerializeObject(value, settings);
        }

        public T Deserialize<T>(string json, NbJsonSerializeConfig config = null)
        {
            if (config == null)
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
            var settings = ConvertSettings(config);
            return JsonConvert.DeserializeObject<T>(json, settings);
        }

        public void SerializeFile(string filePath, object value, NbJsonSerializeConfig config = null)
        {
            string jsonValue = Serialize(value, config);
            File.WriteAllText(filePath, jsonValue, Encoding.UTF8);
        }

        public T DeserializeFile<T>(string filePath, NbJsonSerializeConfig config = null)
        {
            //var value = Deserialize<T>(filePath, config);
            string json = File.ReadAllText(filePath);
            return Deserialize<T>(json, config);
        }

        private JsonSerializerSettings ConvertSettings(NbJsonSerializeConfig config)
        {
            if (config == null)
            {
                throw new ArgumentNullException("config");
            }
            var settings = new JsonSerializerSettings();
            settings.Formatting = (Formatting)config.Formatting;
            if (config.CamelCase)
            {
                settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            }
            return settings;
        }

        #region for di extensions

        ////per call
        //private static Func<INbJsonSerialize> _resolve = () => new NbJsonSerialize();
        //singleton
        private static Func<INbJsonSerialize> _resolve = () => ResolveAsSingleton.Resolve<NbJsonSerialize, INbJsonSerialize>();
        public static Func<INbJsonSerialize> Resolve
        {
            get { return _resolve; }
            set { _resolve = value; }
        }

        #endregion
    }
}