using Newtonsoft.Json;
using System;
using System.IO;

namespace ETR.Simulator
{
    public class JsonSerializer : ETE.Engine.ISerializer
    {
        private static readonly JsonSerializerSettings settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All
        };

        public Stream stream;

        public T Deserialze<T>()
        {
            stream.Seek(0, SeekOrigin.Begin);

            StreamReader reader = new StreamReader(stream);
            string json = reader.ReadToEnd();
                        
            T t = JsonConvert.DeserializeObject<T>(json, settings);
            return t;
        }

        public void Serialize<T>(T obj)
        {
            String json = JsonConvert.SerializeObject(obj, Formatting.Indented, settings);

            System.Diagnostics.Debug.WriteLine(json);

            StreamWriter writer = new StreamWriter(stream);
            writer.Write(json);
            writer.Flush();
        }
    }
}
