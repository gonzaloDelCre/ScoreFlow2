using App.Views;

namespace App
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
            Routing.RegisterRoute(nameof(TeamPage), typeof(TeamPage));
            Routing.RegisterRoute(nameof(Standing), typeof(Standing));
            Routing.RegisterRoute(nameof(Results), typeof(Results));

        }
    }
}
