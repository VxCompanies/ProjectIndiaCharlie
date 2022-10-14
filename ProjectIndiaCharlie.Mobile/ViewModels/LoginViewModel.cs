using ProjectIndiaCharlie.Mobile.ViewModels.Commands;

namespace ProjectIndiaCharlie.Mobile.ViewModels;

public class LoginViewModel : ViewModelBase
{
	private string _id;
	public string Id
	{
		get => _id;
		set
		{
			_id = value;
			OnPropertyChanged(nameof(Id));
		}
	}

	private string _password;
	public string Password
	{
		get => _password;
		set
		{
			_password = value;
			OnPropertyChanged(nameof(Password));
		}
	}

	public LoginCommand LoginCommand { get; set; } = new();
}
