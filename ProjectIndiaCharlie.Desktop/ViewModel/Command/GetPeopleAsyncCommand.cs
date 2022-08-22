﻿using ProjectIndiaCharlie.Core.Models;
using ProjectIndiaCharlie.Desktop.ViewModel.Service;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace ProjectIndiaCharlie.Desktop.ViewModel.Command;

public class GetPeopleAsyncCommand : AsyncCommandBase
{
    public async override Task ExecuteAsync(object? parameter)
    {
        var peopleList = (ObservableCollection<Person>)parameter!;
        peopleList.Clear();

        try
        {
            foreach (var person in await PersonService.GetPeopleAsync())
                peopleList.Add(person);
        }
        catch (Exception)
        {
        }

        //var x = from person
        //        in await PersonService.GetPeopleAsync()
        //        select new
        //        {
        //            person.FirstName,
        //            person.MiddleName
        //        };

        //peopleList = x.ToList();
    }
}
