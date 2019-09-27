using System.Net;
using System.Net.Http;
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

        public ResponseBuilder WithJsonContent(object content, IJsonSerializer serializer = null)
        {
            serializer = serializer ?? new FrameworkSerializer();

            var json = serializer.Serialize(content);
            _message.Content = new StringContent(json, Encoding.UTF8, "application/json");

            return this;
        }


        public static ResponseBuilder Response()
        {
            return new ResponseBuilder(new HttpResponseMessage(HttpStatusCode.OK));
        }

        public static ResponseBuilder Json(object content, IJsonSerializer serializer = null)
        {
            return Response().WithJsonContent(content, serializer);
        }

        public static implicit operator HttpResponseMessage(ResponseBuilder builder)
        {
            return builder._message;
        }
    }
}