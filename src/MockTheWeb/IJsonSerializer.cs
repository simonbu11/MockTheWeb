using System.IO;
using System.Runtime.Serialization.Json;

namespace MockTheWeb
{
    public interface IJsonSerializer
    {
        string Serialize(object item);
    }

    public class FrameworkSerializer : IJsonSerializer
    {
        public string Serialize(object item)
        {
            var s = new DataContractJsonSerializer(item.GetType());
            using (var stream = new MemoryStream())
            {
                s.WriteObject(stream, item);

                stream.Position = 0;
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}