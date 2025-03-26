using Newtonsoft.Json;

namespace API1.Controllers.Users
{
    public class ApiGateway : IApiGateway
    {
        private readonly HttpClient _httpClient;

        // Constructor que inyecta HttpClient
        public ApiGateway(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Método GET
        public async Task<T> GetAsync<T>(string endpoint)
        {
            var response = await _httpClient.GetAsync(endpoint);
            response.EnsureSuccessStatusCode(); // Lanza una excepción si la respuesta no es exitosa
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(content); // Deserializamos la respuesta JSON
        }

        // Método POST
        public async Task<T> PostAsync<T>(string endpoint, object data)
        {
            var response = await _httpClient.PostAsJsonAsync(endpoint, data);
            response.EnsureSuccessStatusCode(); // Lanza una excepción si la respuesta no es exitosa
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(content); // Deserializamos la respuesta JSON
        }

        // Método PUT
        public async Task<T> PutAsync<T>(string endpoint, object data)
        {
            var response = await _httpClient.PutAsJsonAsync(endpoint, data);
            response.EnsureSuccessStatusCode(); // Lanza una excepción si la respuesta no es exitosa
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(content); // Deserializamos la respuesta JSON
        }

        // Método DELETE
        public async Task DeleteAsync(string endpoint)
        {
            var response = await _httpClient.DeleteAsync(endpoint);
            response.EnsureSuccessStatusCode(); // Lanza una excepción si la respuesta no es exitosa
        }
    }

}
