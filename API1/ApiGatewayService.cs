using Newtonsoft.Json;
using System.Text;

namespace API1
{
    public class ApiGatewayService
    {
        private readonly HttpClient _httpClient;
        private readonly Dictionary<string, string> _endpoints;

        public ApiGatewayService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClient = httpClientFactory.CreateClient();
            _endpoints = configuration.GetSection("ApiGateway:Endpoints").Get<Dictionary<string, string>>();
        }

        public async Task<string> CallLambdaEndpointAsync(string entity, string method, object data = null)
        {
            if (!_endpoints.ContainsKey(entity))
            {
                throw new ArgumentException($"Endpoint for entity '{entity}' is not defined.");
            }

            var endpoint = _endpoints[entity];
            var url = $"{endpoint}/{method}";

            var content = data != null ? new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json") : null;

            HttpResponseMessage response;

            if (method == "GET")
            {
                response = await _httpClient.GetAsync(url);
            }
            else if (method == "POST")
            {
                response = await _httpClient.PostAsync(url, content);
            }
            else if (method == "PUT")
            {
                response = await _httpClient.PutAsync(url, content);
            }
            else if (method == "DELETE")
            {
                response = await _httpClient.DeleteAsync(url);
            }
            else
            {
                throw new NotImplementedException($"HTTP method {method} is not implemented.");
            }

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }

            return $"Error: {response.StatusCode}";
        }
    }

}
