using AppMovil.Services;
using AppMovil.ViewModels;

namespace AppMovil.Views;

public partial class LoginPage : ContentPage
{
    private readonly AuthService _authService;

    public LoginPage()
    {
        InitializeComponent();
        _authService = new AuthService();
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        string email = emailEntry.Text;
        string password = passwordEntry.Text;

        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            await DisplayAlert("Error", "Email and password are required.", "OK");
            return;
        }

        var user = await _authService.LoginAsync(email, password);

        if (user != null)
        {
            await Navigation.PushAsync(new MainPage());
        }
        else
        {
            await DisplayAlert("Login Failed", "Invalid email or password", "OK");
        }
    }
}