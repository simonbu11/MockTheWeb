using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using NUnit.Framework;

namespace MockTheWeb.UnitTests
{
    public class HttpClientMockTests
    {
        [Test]
        public async Task ShouldReturnEmptyOkResponseByDefault()
        {
            var mock = new HttpClientMock();
            var client = mock.AsHttpClient();

            var actual = await client.GetAsync("/api/people");

            Assert.IsNotNull(actual);
            Assert.AreEqual(HttpStatusCode.OK, actual.StatusCode);
            Assert.IsNull(actual.Content);
        }

        [Test]
        public async Task ShouldReturnOverriddenDefaultResponse()
        {
            var expected = new HttpResponseMessage(HttpStatusCode.Accepted);
            var mock = new HttpClientMock()
                .SetDefaultResponse(expected);
            var client = mock.AsHttpClient();

            var actual = await client.GetAsync("/api/people");

            Assert.AreSame(expected, actual);
        }

        [Test]
        public async Task ShouldReturnResponseForMatchingRequest()
        {
            var expected = new HttpResponseMessage(HttpStatusCode.Accepted);
            var mock = new HttpClientMock();
            mock.When((req) => req.Method == HttpMethod.Get)
                .Then(expected);
            var client = mock.AsHttpClient();

            var actual = await client.GetAsync("/api/people");

            Assert.AreSame(expected, actual);
        }

        [Test]
        public async Task ShouldReturnLastResponseForMatchingRequestWhenMultipleConfigured()
        {
            var expected = new HttpResponseMessage(HttpStatusCode.Accepted);
            var mock = new HttpClientMock();
            mock.When((req) => req.RequestUri.LocalPath == "/api/people")
                .Then(new HttpResponseMessage(HttpStatusCode.BadRequest));
            mock.When((req) => req.Method == HttpMethod.Get)
                .Then(expected);
            var client = mock.AsHttpClient();

            var actual = await client.GetAsync("/api/people");

            Assert.AreSame(expected, actual);
        }

        [Test]
        public async Task ShouldNotErrorWhenVerifyingValidAssertion()
        {
            var mock = new HttpClientMock();
            var client = mock.AsHttpClient();

            await client.GetAsync("/api/people");

            Assert.DoesNotThrow(() =>
                mock.Verify((req) => req.Method == HttpMethod.Get, 1));
        }
        
        [Test]
        public async Task ShouldErrorWithDefaultMessageWhenVerifyingInvalidAssertion()
        {
            var mock = new HttpClientMock();
            var client = mock.AsHttpClient();

            await client.GetAsync("/api/people");

            var actual = Assert.Throws<VerificationException>(() =>
                mock.Verify((req) => req.Method == HttpMethod.Get, 0));
            Assert.AreEqual("Verification failed. Expected 0 requests to match, but found 1", actual.Message);
        }
        
        [Test]
        public async Task ShouldErrorWithConfiguredMessageWhenVerifyingInvalidAssertion()
        {
            var mock = new HttpClientMock();
            var client = mock.AsHttpClient();

            await client.GetAsync("/api/people");

            var expectedMessage = "unit test exception message";
            var actual = Assert.Throws<VerificationException>(() =>
                mock.Verify((req) => req.Method == HttpMethod.Get, 0, expectedMessage));
            Assert.AreEqual(expectedMessage, actual.Message);
        }
    }
}