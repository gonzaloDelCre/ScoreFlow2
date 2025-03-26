using App.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace App.ViewModels
{
    public class MainPageViewModel : BindableObject
    {
        public ICommand LoginCommand { get; set; }
        public ICommand RegisterCommand { get; set; }

        public MainPageViewModel()
        {
            // Comandos para navegar a Login y Register
            LoginCommand = new Command(OnLogin);
            RegisterCommand = new Command(OnRegister);
        }

        private async void OnLogin()
        {
            // Navegar a la página de Login
            await Shell.Current.GoToAsync(nameof(LoginPage));
        }

        private async void OnRegister()
        {
            // Navegar a la página de Register
            await Shell.Current.GoToAsync(nameof(RegisterPage));
        }
    }
}
