using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMovil
{
    using Microsoft.Maui.Storage;

    public static class SecureStorageService
    {
        public static async Task SaveUserInfoAsync(string userId, string token)
        {
            await SecureStorage.SetAsync("userId", userId);
            await SecureStorage.SetAsync("token", token); 
        }

        public static async Task<string> GetUserIdAsync() => await SecureStorage.GetAsync("userId");

        public static async Task<string> GetTokenAsync() => await SecureStorage.GetAsync("token");
    }

}
