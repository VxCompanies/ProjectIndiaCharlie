using ProjectIndiaCharlie.Desktop.ViewModels;
using ProjectIndiaCharlie.Desktop.ViewModels.Services;
using System.Windows;
using System.Windows.Controls;

namespace ProjectIndiaCharlie.Desktop.Views
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : UserControl
    {
        public LoginView()
        {
            InitializeComponent();
            //tbUserId.Text = "1110408";
            //pbPassword.Password = "qwerty123";
            //Login(new(), new());
        }

        private void UserIdChanged(object sender, RoutedEventArgs e) => btLogin.IsEnabled =
            !string.IsNullOrWhiteSpace(pbPassword.Password) &&
            !string.IsNullOrWhiteSpace(tbUserId.Text);

        private void PasswordChanged(object sender, RoutedEventArgs e) => btLogin.IsEnabled =
            !string.IsNullOrWhiteSpace(pbPassword.Password) &&
            !string.IsNullOrWhiteSpace(tbUserId.Text);

        private async void Login(object sender, RoutedEventArgs e)
        {
            if (!await StudentService.Login(tbUserId.Text, pbPassword.Password))
            {
                MessageBox.Show("Wrong id or password.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            NavigationService.MainNavigate(new IndexViewModel());
            NavigationService.IndexNavigate(new HomeViewModel());
        }
    }
}
