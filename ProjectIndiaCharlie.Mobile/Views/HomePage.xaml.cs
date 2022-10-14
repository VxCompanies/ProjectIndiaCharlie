using ProjectIndiaCharlie.Mobile.ViewModels.Stores;

namespace ProjectIndiaCharlie.Mobile.Views;

public partial class HomePage : ContentPage
{
	public HomePage()
	{
		InitializeComponent();

		LoadStudentInfo();
	}

	private void LoadStudentInfo()
	{
		txtStudentId.Text = LogedStudent.Student!.PersonId.ToString();
		txtStudentName.Text = LogedStudent.Student!.FirstName;
		txtStudentSurname.Text = LogedStudent.Student!.PersonId.ToString();
		txtStudentCareer.Text = LogedStudent.Student!.PersonId.ToString();
	}
}