using ProjectIndiaCharlie.Desktop.ViewModels.Commands.AsyncCommands;

namespace ProjectIndiaCharlie.Desktop.ViewModels
{
    public class LoginViewModel : ViewModelBase
	{
		private string _userId;
		public string UserId
		{
			get => _userId;
			set
			{
				_userId = value;
				OnPropertyChanged(nameof(UserId));
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

		public LoginAsyncCommand LoginAsyncCommand { get; set; }

		public LoginViewModel() => LoginAsyncCommand = new();
	}
}
