using Newtonsoft.Json;

namespace MockTheWeb.NewtonsoftJson
{
    public class NewtonsoftJsonSerializer : IJsonSerializer
    {

        public NewtonsoftJsonSerializer()
        {
        }
        public NewtonsoftJsonSerializer(JsonSerializerSettings serializerSettings)
        {
            SerializerSettings = serializerSettings;
        }
        
        public JsonSerializerSettings SerializerSettings { get; }
        
        public string Serialize(object item)
        {
            if (SerializerSettings != null)
            {
                return JsonConvert.SerializeObject(item, SerializerSettings);
            }
            
            return JsonConvert.SerializeObject(item);
        }
    }
}