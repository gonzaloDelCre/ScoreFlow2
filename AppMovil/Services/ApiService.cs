using Application.Users.DTOs;
using AppMovil.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AppMovil.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://tuapi.com/api/") // Cambia esto por la URL de tu API
            };
        }

        public async Task<AuthResponseDTO> LoginAsync(string email, string password)
        {
            var request = new LoginRequestDTO { Email = email, Password = password };
            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("users/login", content);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var responseJson = await response.Content.ReadAsStringAsync();
            var authResponse = JsonSerializer.Deserialize<AuthResponseDTO>(responseJson);

            // Guardar el token y los datos en SecureStorage
            await SecureStorage.SetAsync("AuthToken", authResponse.Token);
            await SecureStorage.SetAsync("UserID", authResponse.UserID.ToString());
            await SecureStorage.SetAsync("FullName", authResponse.FullName);

            return authResponse;
        }
    }
}
