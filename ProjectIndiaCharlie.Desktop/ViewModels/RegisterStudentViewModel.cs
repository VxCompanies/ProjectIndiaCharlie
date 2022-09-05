using ProjectIndiaCharlie.Desktop.Models;
using ProjectIndiaCharlie.Desktop.ViewModels.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectIndiaCharlie.Desktop.ViewModels
{
    public class RegisterStudentViewModel : ViewModelBase
    {
		private Student _student;
		public Student Student
		{
			get => _student;
			set
			{
				_student = value;
				OnPropertyChanged(nameof(Student));
			}
		}

		public RegisterStudentAsyncCommand RegisterStudentAsyncCommand { get; set; }

		public RegisterStudentViewModel()
        {
			Student = new()
			{
				Career = new(),
				Person = new()
			};

			RegisterStudentAsyncCommand = new();
        }
    }
}
