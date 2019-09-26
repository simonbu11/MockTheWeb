using System.IO;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;

namespace MockTheWeb
{
    public class ResponseBuilder
    {
        private readonly HttpResponseMessage _message;

        private ResponseBuilder(HttpResponseMessage message)
        {
            _message = message;
        }

        public ResponseBuilder WithStatus(HttpStatusCode statusCode)
        {
            _message.StatusCode = statusCode;
            return this;
        }

        public ResponseBuilder WithJsonContent(object content)
        {
            var s = new DataContractJsonSerializer(content.GetType());
            using (var stream = new MemoryStream())
            {
                s.WriteObject(stream, content);

                stream.Position = 0;
                using (var reader = new StreamReader(stream))
                {
                    var json = reader.ReadToEnd();
                    _message.Content = new StringContent(json, Encoding.UTF8, "application/json");
                }
            }

            return this;
        }


        public static ResponseBuilder Response()
        {
            return new ResponseBuilder(new HttpResponseMessage(HttpStatusCode.OK));
        }

        public static ResponseBuilder Json(object content)
        {
            return Response().WithJsonContent(content);
        }

        public static implicit operator HttpResponseMessage(ResponseBuilder builder)
        {
            return builder._message;
        }
    }
}