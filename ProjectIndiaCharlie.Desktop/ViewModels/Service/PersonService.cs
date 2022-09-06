﻿using ProjectIndiaCharlie.Desktop.Models;
using ProjectIndiaCharlie.Desktop.ViewModels.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProjectIndiaCharlie.Desktop.ViewModels.Service;

public static class PersonService
{
    private const string mediaType = "application/json";
    private const string baseUrl = "https://localhost:7073/api/Person";
    private const string getPeopleUrl = $"{baseUrl}/People/List";
    private const string loginUrl = $"{baseUrl}/Login";
    private const string registerStudentUrl = $"{baseUrl}/Student/Registration";

    private static readonly JsonSerializerOptions _options = new() { PropertyNameCaseInsensitive = true };

    public static async Task<IEnumerable<Person>> GetPeopleAsync()
    {
        try
        {
            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(getPeopleUrl);

            if (!response.IsSuccessStatusCode)
                return Enumerable.Empty<Person>();

            var content = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<IEnumerable<Person>>(content, _options)!;
        }
        catch (Exception)
        {
            return Enumerable.Empty<Person>();
        }
    }

    public static async Task<Person?> Login(string personId, string password)
    {
        try
        {
            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync($"{loginUrl}?personId={personId}&password={password}");

            if (!response.IsSuccessStatusCode)
                return null;

            var content = await response.Content.ReadAsStringAsync();

            var logedUser = JsonSerializer.Deserialize<Person>(content, _options);
            LogedPerson.Person = logedUser!;
            return logedUser;
        }
        catch (Exception)
        {
            return null;
        }
    }

    public static async Task<bool> RegisterPerson(Student student)
    {
        try
        {
            using var httpClient = new HttpClient();

            var json = JsonSerializer.Serialize(student);
            var content = new StringContent(json, Encoding.UTF8, mediaType);

            var response = await httpClient.PostAsync(registerStudentUrl, content);

            if (!response.IsSuccessStatusCode)
                return false;

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public static async Task<bool> RegisterPerson(Professor professor)
    {
        try
        {
            using var httpClient = new HttpClient();

            var json = JsonSerializer.Serialize(professor);
            var content = new StringContent(json, Encoding.UTF8, mediaType);

            var response = await httpClient.PostAsync(registerStudentUrl, content);

            if (!response.IsSuccessStatusCode)
                return false;

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public static async Task<bool> RegisterPerson(Coordinator coordinator)
    {
        try
        {
            using var httpClient = new HttpClient();

            var json = JsonSerializer.Serialize(coordinator);
            var content = new StringContent(json, Encoding.UTF8, mediaType);

            var response = await httpClient.PostAsync(registerStudentUrl, content);

            if (!response.IsSuccessStatusCode)
                return false;

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
