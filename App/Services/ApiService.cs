using App.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace App.Services
{
    public class ApiService
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        private static readonly string _baseUrl = "https://ec2-54-82-96-38.compute-1.amazonaws.com/";  // Cambia por la URL de tu API


        // Método para realizar login
        public static async Task<UserResponseDTO> LoginAsync(string email, string password)
        {
            try
            {
                var loginRequest = new { Email = email, Password = password };
                var jsonRequest = JsonSerializer.Serialize(loginRequest);
                var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"{_baseUrl}/login", content);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<UserResponseDTO>(jsonResponse);
                }
                else
                {
                    throw new Exception("Error al iniciar sesión. Por favor, intenta de nuevo.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al intentar conectar con la API: {ex.Message}");
            }
        }


        public static async Task<UserResponseDTO> RegisterAsync(string fullName, string email, string password)
        {
            if (string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                throw new ArgumentException("Todos los campos son obligatorios.");

            var registerRequest = new
            {
                FullName = fullName,
                Email = email,
                Password = password
            };

            var jsonRequest = JsonSerializer.Serialize(registerRequest);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync($"{_baseUrl}/register", content);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var userResponse = JsonSerializer.Deserialize<UserResponseDTO>(jsonResponse);
                    return userResponse;
                }
                else
                {
                    throw new Exception("Error al registrar el usuario. Intenta de nuevo.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al intentar conectar con la API: {ex.Message}");
            }
        }
    }
}


