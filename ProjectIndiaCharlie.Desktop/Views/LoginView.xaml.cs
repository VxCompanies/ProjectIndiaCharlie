using ProjectIndiaCharlie.Desktop.ViewModels;
using ProjectIndiaCharlie.Desktop.ViewModels.Services;
using System;
using System.Windows;
using System.Windows.Controls;

namespace ProjectIndiaCharlie.Desktop.Views
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : UserControl
    {
        public LoginView() => InitializeComponent();

        //private void PasswordChanged(object sender, RoutedEventArgs e)
        //{
        //    if (DataContext == null)
        //        return;

        //    ((LoginViewModel)DataContext).Password = ((PasswordBox)sender).Password;
        //}

        private void UserIdChanged(object sender, RoutedEventArgs e) => btLogin.IsEnabled =
            !string.IsNullOrWhiteSpace(pbPassword.Password) &&
            !string.IsNullOrWhiteSpace(tbUserId.Text);

        private void PasswordChanged(object sender, RoutedEventArgs e) => btLogin.IsEnabled =
            !string.IsNullOrWhiteSpace(pbPassword.Password) &&
            !string.IsNullOrWhiteSpace(tbUserId.Text);

        private async void Login(object sender, RoutedEventArgs e)
        {
            if (await StudentService.Login(tbUserId.Text, pbPassword.Password) is null)
            {
                MessageBox.Show("Wrong id or password.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            NavigationService.MainNavigate(new IndexViewModel());
            NavigationService.IndexNavigate(new HomeViewModel());
        }
    }
}
