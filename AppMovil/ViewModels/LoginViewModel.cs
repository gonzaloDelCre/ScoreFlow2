using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AppMovil.Services;
using AppMovil.Models;
using Microsoft.Maui.Controls;

namespace AppMovil.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly ApiService _apiService;

        [ObservableProperty]
        private string _email;

        [ObservableProperty]
        private string _password;

        public ICommand LoginCommand { get; }

        public LoginViewModel()
        {
            _apiService = new ApiService();
            LoginCommand = new RelayCommand(async () => await LoginAsync());
        }

        private async Task LoginAsync()
        {
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                await App.Current.MainPage.DisplayAlert("Error", "Usuario y contraseña requeridos", "OK");
                return;
            }

            var user = await _apiService.LoginAsync(Email, Password);
            if (user != null)
            {
                await Shell.Current.GoToAsync("//home");
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Error", "Credenciales incorrectas", "OK");
            }
        }
    }
}