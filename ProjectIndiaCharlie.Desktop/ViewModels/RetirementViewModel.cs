﻿using ProjectIndiaCharlie.Desktop.Models;
using ProjectIndiaCharlie.Desktop.ViewModels.Commands.AsyncCommands;
using ProjectIndiaCharlie.Desktop.ViewModels.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace ProjectIndiaCharlie.Desktop.ViewModels
{
    public class RetirementViewModel : ViewModelBase
    {
		private VStudentSubject _selectedSubject;
		public VStudentSubject SelectedSubject
		{
			get => _selectedSubject;
			set
			{
				_selectedSubject = value;
				OnPropertyChanged(nameof(SelectedSubject));
			}
		}

        public ObservableCollection<VStudentSubject> SelectedSubjects { get; set; }

        public RetireSubjectAsyncCommand RetireSubjectAsyncCommand { get; set; }

		public RetirementViewModel()
		{
			SelectedSubjects = new();
            RetireSubjectAsyncCommand = new();

            _ = GetSelectedSubjects();
        }

        private async Task GetSelectedSubjects()
        {
            SelectedSubjects.Clear();

            foreach (var subject in await StudentService.GetSchedule())
                SelectedSubjects.Add(subject);
        }
    }
}
