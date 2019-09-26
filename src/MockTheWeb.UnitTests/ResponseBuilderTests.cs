using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NUnit.Framework;

namespace MockTheWeb.UnitTests
{
    public class ResponseBuilderTests
    {
        [Test]
        public void ShouldDefaultToOKStatusWhenUsingResponseInitialiser()
        {
            var builder = ResponseBuilder.Response();
            HttpResponseMessage message = builder;

            Assert.AreEqual(HttpStatusCode.OK, message.StatusCode);
        }

        [Test]
        public void ShouldDefaultToOKStatusWhenUsingJsonInitialiser()
        {
            var builder = ResponseBuilder.Json(TestPerson.Default());
            HttpResponseMessage message = builder;

            Assert.AreEqual(HttpStatusCode.OK, message.StatusCode);
        }

        [Test]
        public void ShouldSetContentToStringContentWhenUsingJsonInitialiser()
        {
            var builder = ResponseBuilder.Json(TestPerson.Default());
            HttpResponseMessage message = builder;

            Assert.IsInstanceOf<StringContent>(message.Content);
        }

        [Test]
        public void ShouldUseUtf8JsonContentTypeWhenUsingJsonInitialiser()
        {
            var builder = ResponseBuilder.Json(TestPerson.Default());
            HttpResponseMessage message = builder;

            var content = (StringContent) message.Content;
            var contentType = content.Headers.ContentType;

            Assert.AreEqual("application/json", contentType.MediaType);
            Assert.AreEqual("utf-8", contentType.CharSet);
        }

        [Test]
        public async Task ShouldSeralizerObjectToContentWhenUsingJsonInitialiser()
        {
            var person = TestPerson.Default();
            var builder = ResponseBuilder.Json(person);
            HttpResponseMessage message = builder;

            var content = (StringContent) message.Content;
            var body = await content.ReadAsStringAsync();
            var actualPerson = JsonConvert.DeserializeObject<TestPerson>(body);
            Assert.AreEqual(person.Id, actualPerson.Id);
            Assert.AreEqual(person.FirstName, actualPerson.FirstName);
            Assert.AreEqual(person.LastName, actualPerson.LastName);
        }

        [Test]
        public void ShouldUpdateStatusWhenWithStatusUsed()
        {
            var builder = ResponseBuilder.Response().WithStatus(HttpStatusCode.Conflict);
            HttpResponseMessage message = builder;

            Assert.AreEqual(HttpStatusCode.Conflict, message.StatusCode);
        }

        [Test]
        public void ShouldSetContentToStringContentWhenWithJsonContent()
        {
            var builder = ResponseBuilder.Response().WithJsonContent(TestPerson.Default());
            HttpResponseMessage message = builder;

            Assert.IsInstanceOf<StringContent>(message.Content);
        }

        [Test]
        public void ShouldUseUtf8JsonContentTypeWhenUsingWithJsonContent()
        {
            var builder = ResponseBuilder.Response().WithJsonContent(TestPerson.Default());
            HttpResponseMessage message = builder;

            var content = (StringContent) message.Content;
            var contentType = content.Headers.ContentType;

            Assert.AreEqual("application/json", contentType.MediaType);
            Assert.AreEqual("utf-8", contentType.CharSet);
        }

        [Test]
        public async Task ShouldSeralizerObjectToContentWhenUsingWithJsonContent()
        {
            var person = TestPerson.Default();
            var builder = ResponseBuilder.Response().WithJsonContent(person);
            HttpResponseMessage message = builder;

            var content = (StringContent) message.Content;
            var body = await content.ReadAsStringAsync();
            var actualPerson = JsonConvert.DeserializeObject<TestPerson>(body);
            Assert.AreEqual(person.Id, actualPerson.Id);
            Assert.AreEqual(person.FirstName, actualPerson.FirstName);
            Assert.AreEqual(person.LastName, actualPerson.LastName);
        }
    }
}