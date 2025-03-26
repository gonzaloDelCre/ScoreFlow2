using System;
using System.Windows.Input;
using App.Services;  // Asumiendo que ApiService está en la carpeta Services

namespace App.ViewModels
{
    public class RegisterPageViewModel : BindableObject
    {
        public ICommand RegisterCommand { get; set; }

        // Propiedades para los campos de la página de registro
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public RegisterPageViewModel()
        {
            RegisterCommand = new Command(OnRegister);
        }

        private async void OnRegister()
        {
           

            try
            {
                // Aquí llamamos al servicio para registrar al usuario
                var userResponse = await ApiService.RegisterAsync(FullName, Email, Password);

                // Si el registro es exitoso, navegar a la página de equipos
                await Shell.Current.GoToAsync("//teams");
            }
            catch (Exception ex)
            {
                // Si ocurre algún error, mostrar el mensaje de error
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }

    }
}
