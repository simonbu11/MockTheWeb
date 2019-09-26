using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ExampleApiClient
{
    public class ApiClient
    {
        private readonly HttpClient _httpClient;

        public ApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Person> GetById(int id)
        {
            // Use the HttpClient just like normal
            var response = await _httpClient.GetAsync($"/api/people/{id}");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Person>(json);
        }
    }
}