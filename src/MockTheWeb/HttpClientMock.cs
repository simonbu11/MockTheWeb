using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace MockTheWeb
{
    public class HttpClientMock : HttpMessageHandler
    {
        private HttpResponseMessage _defaultResponse = new HttpResponseMessage(HttpStatusCode.OK);
        private readonly List<HttpRequestMessage> _requests = new List<HttpRequestMessage>();
        private readonly List<MockResponse> _responses = new List<MockResponse>();

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            _requests.Add(request);

            HttpResponseMessage response = null;
            for (var i = _responses.Count - 1; i >= 0 && response == null; i--)
            {
                var responseMock = _responses[i];
                if (responseMock.Condition(request))
                {
                    response = responseMock.ResponseBuilder(request);
                }
            }

            return Task.FromResult(response ?? _defaultResponse);
        }

        public HttpClient AsHttpClient()
        {
            return new HttpClient(this)
            {
                BaseAddress = new Uri("http://base-address.local/"),
            };
        }
        
        
        

        public HttpClientMock SetDefaultResponse(HttpResponseMessage response)
        {
            _defaultResponse = response;
            return this;
        }

        public MockResponse When(Func<HttpRequestMessage, bool> condition)
        {
            var response = new MockResponse(condition, this);
            _responses.Add(response);
            return response;
        }

        
        
        public void Verify(Func<HttpRequestMessage, bool> condition, int times = 1, string failureMessage = null)
        {
            Verify(condition, Times.Exactly(times), failureMessage);
        }

        public void Verify(Func<HttpRequestMessage, bool> condition, Times times, string failureMessage = null)
        {
            var matches = _requests.Where(condition).ToArray();
            if (!(times ?? Times.Once()).Verify(matches.Length))
            {
                var message = !string.IsNullOrEmpty(failureMessage)
                    ? failureMessage
                    : $"Verification failed. Expected {times} requests to match, but found {matches.Length}";
                throw new VerificationException(message);
            }
        }
    }
}