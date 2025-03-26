using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Infrastructure.ApiClients
{
    public class ApiGatewayClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public ApiGatewayClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;

            // Configura la URL base desde appsettings.json
            _httpClient.BaseAddress = new Uri(_configuration["ApiGateway:BaseUrl"]);
        }

        // Método genérico para obtener todos los elementos
        public async Task<IEnumerable<T>> GetAllAsync<T>(string endpoint)
        {
            var response = await _httpClient.GetAsync(endpoint);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<IEnumerable<T>>();
        }

        // Método genérico para obtener un elemento por ID
        public async Task<T> GetByIdAsync<T>(string endpoint, string id)
        {
            var response = await _httpClient.GetAsync($"{endpoint}/{id}");
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return default;

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<T>();
        }

        // Método genérico para crear un elemento
        public async Task<T> CreateAsync<T>(string endpoint, object data)
        {
            var response = await _httpClient.PostAsJsonAsync(endpoint, data);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<T>();
        }

        // Método genérico para actualizar un elemento
        public async Task<T> UpdateAsync<T>(string endpoint, string id, object data)
        {
            var response = await _httpClient.PutAsJsonAsync($"{endpoint}/{id}", data);
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return default;

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<T>();
        }

        // Método genérico para eliminar un elemento
        public async Task<bool> DeleteAsync(string endpoint, string id)
        {
            var response = await _httpClient.DeleteAsync($"{endpoint}/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
