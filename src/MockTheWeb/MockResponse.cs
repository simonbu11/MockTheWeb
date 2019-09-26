using System;
using System.Net.Http;

namespace MockTheWeb
{
    public class MockResponse
    {
        private readonly HttpClientMock _mock;

        internal MockResponse(Func<HttpRequestMessage, bool> condition, HttpClientMock mock)
        {
            _mock = mock;
            Condition = condition;
        }

        internal Func<HttpRequestMessage, bool> Condition { get; }
        internal Func<HttpRequestMessage, HttpResponseMessage> ResponseBuilder { get; private set; }

        public HttpClientMock Then(HttpResponseMessage response)
        {
            ResponseBuilder = (req) => response;
            return _mock;
        }

        public HttpClientMock Then(Func<HttpRequestMessage, HttpResponseMessage> responseBuilder)
        {
            ResponseBuilder = responseBuilder;
            return _mock;
        }
    }
}