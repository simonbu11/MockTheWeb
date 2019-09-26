using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using MockTheWeb;
using NUnit.Framework;

namespace ExampleApiClient.UnitTests
{
    public class ApiClientTests
    {
        private HttpClientMock _httpClientMock;
        private ApiClient _client;

        [SetUp]
        public void Setup()
        {
            // Create a new HttpClientMock
            _httpClientMock = new HttpClientMock();

            // Setup a default response. Can use a bad response so that EnsureSuccessStatusCode with error by default
            _httpClientMock.SetDefaultResponse(new HttpResponseMessage(HttpStatusCode.InternalServerError));

            // Use the HttpClient from the mock
            _client = new ApiClient(_httpClientMock.AsHttpClient());
        }

        [Test]
        public async Task ShouldDeserializerResponse()
        {
            var expectedPerson = new Person
            {
                Id = 1,
                FirstName = "Jessica",
                LastName = "Fandango",
            };

            // Setup client to return an appropriate message
            _httpClientMock
                .When((req) => req.Method == HttpMethod.Get && req.RequestUri.LocalPath == "/api/people/1")
                .Then(ResponseBuilder.Json(expectedPerson)); // Library also includes HttpResponseMessage helper

            // Call you code as normal
            var actual = await _client.GetById(1);

            // Assert response being processed OK
            Assert.IsNotNull(actual);
            Assert.AreEqual(expectedPerson.Id, actual.Id);
            Assert.AreEqual(expectedPerson.FirstName, actual.FirstName);
            Assert.AreEqual(expectedPerson.LastName, actual.LastName);
        }

        [Test]
        public async Task ShouldCallPeopleEndpointWithId()
        {
            _httpClientMock
                .When((req) => req.Method == HttpMethod.Get && req.RequestUri.LocalPath == "/api/people/1")
                .Then(ResponseBuilder.Json(new Person
                {
                    Id = 1,
                    FirstName = "Jessica",
                    LastName = "Fandango",
                }));

            await _client.GetById(1);

            // Can also verify unit is interactive correctly
            _httpClientMock.Verify((req) => req.Method == HttpMethod.Get,
                1); // Check a request was called an exact number of times
            _httpClientMock.Verify((req) => req.RequestUri.LocalPath == "/api/people/1",
                Times.Once()); // Or with a times version
        }
    }
}