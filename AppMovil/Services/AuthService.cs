using Application.Users.DTOs;
using AppMovil.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace AppMovil.Services
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://localhost:7280/swagger/index.html"; 

        public AuthService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<UserResponseDTO> LoginAsync(string email, string password)
        {
            var loginRequest = new LoginRequestDTO
            {
                Email = email,
                Password = password
            };

            var content = new StringContent(JsonConvert.SerializeObject(loginRequest), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{BaseUrl}/login", content);

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                var user = JsonConvert.DeserializeObject<UserResponseDTO>(responseBody);
                return user;
            }

            return null; // Si el login falla
        }
    }
}
