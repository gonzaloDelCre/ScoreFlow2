using App.Services;
using System;
using System.Windows.Input;

namespace App.ViewModels
{
    public class LoginPageViewModel : BindableObject
    {
        public ICommand LoginCommand { get; set; }

        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                OnPropertyChanged();
            }
        }

        // Propiedades para el correo y la contraseña
        private string _email;
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged();
            }
        }

        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        public LoginPageViewModel()
        {
            LoginCommand = new Command(OnLogin);
        }

        private async void OnLogin()
        {
            IsBusy = true;  // Mostrar el indicador de carga

            try
            {
                // Ahora usamos las propiedades del ViewModel
                var userResponse = await ApiService.LoginAsync(Email, Password);
                await Shell.Current.GoToAsync("//teams");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;  // Ocultar el indicador de carga
            }
        }
    }
}
