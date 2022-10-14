using ProjectIndiaCharlie.Mobile.ViewModels.Services;

namespace ProjectIndiaCharlie.Mobile.Views;

public partial class LoginPage : ContentPage
{
	public LoginPage() => InitializeComponent();

	private async void Login_Clicked(object sender, EventArgs e)
	{
		if (string.IsNullOrWhiteSpace(txtId.Text))
		{
			await DisplayAlert("Warning", "Id or Password field is empty.", "Ok"); 
			return;
		}

		if (string.IsNullOrWhiteSpace(txtPassword.Text))
		{
			await DisplayAlert("Warning", "Id or Password field is empty.", "Ok");
			return;
		}

		if (!await StudentService.Login(txtId.Text, txtPassword.Text))
		{
            await DisplayAlert("Warning", "Wrong Id or Password.", "Ok");
            return;
        }

		await Navigation.PushAsync(new HomePage());
	}
}
