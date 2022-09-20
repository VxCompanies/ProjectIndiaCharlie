using Microsoft.Extensions.Configuration;
using ProjectIndiaCharlie.WebAdministrator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProjectIndiaCharlie.WebAdministrator.Service;

public static class StudentService
{
    private readonly static string baseUrl = Program.Configuration.GetConnectionString("AcademicsAPI");

    //public StudentService(IConfiguration configuration) => baseUrl = configuration.GetConnectionString("AcademicsAPI");

    //public StudentService(IConfiguration configuration)
    //{
    //    baseUrl = configuration.GetConnectionString("AcademicsAPI");
    //}

    private const string mediaType = "application/json";
    //private const string baseUrl = "https://localhost:7073/api";
    //private string  = connectionString;

    private static string loginUrl = $"{baseUrl}/Student/Login";
    private static string getSelectedSubjects = $"{baseUrl}/Student/SelectedSubjects";
    private static string registerStudentUrl = $"{baseUrl}/Student/Registration";

    private static readonly JsonSerializerOptions _options = new() { PropertyNameCaseInsensitive = true };

    //public static async Task<VStudentDetail?> Login(string studentId, string password)
    //{
    //    try
    //    {
    //        using var httpClient = new HttpClient();
    //        var response = await httpClient.GetAsync($"{loginUrl}?studentId={studentId}&password={password}");

    //        if (!response.IsSuccessStatusCode)
    //            return null;

    //        var content = await response.Content.ReadAsStringAsync();

    //        var logedStudent = JsonSerializer.Deserialize<VStudentDetail>(content, _options);
    //        LogedStudent.Student = logedStudent!;
    //        return logedStudent;
    //    }
    //    catch (Exception)
    //    {
    //        return null;
    //    }
    //}

    public static async Task<IEnumerable<VStudentSubject>> GetSelectedSubjects(int studentId)
    {
        try
        {
            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync($"{getSelectedSubjects}?studentId={studentId}");

            if (!response.IsSuccessStatusCode)
                return Enumerable.Empty<VStudentSubject>();

            var content = await response.Content.ReadAsStringAsync();

            var studentSubjects = JsonSerializer.Deserialize<IEnumerable<VStudentSubject>>(content, _options);

            return studentSubjects!;
        }
        catch (Exception)
        {
            return Enumerable.Empty<VStudentSubject>();
        }
    }

    public static async Task<NewPerson?> RegisterPerson(NewPerson newPerson)
    {
        try
        {
            using var httpClient = new HttpClient();

            var json = JsonSerializer.Serialize(newPerson);
            var content1 = new StringContent(json, Encoding.UTF8, mediaType);

            var response = await httpClient.PostAsync(registerStudentUrl, content1);

            if (!response.IsSuccessStatusCode)
                return null;

            var content2 = await response.Content.ReadAsStringAsync();
            var createdPerson = JsonSerializer.Deserialize<NewPerson>(content2, _options);

            return createdPerson!;
        }
        catch (Exception)
        {
            return null;
        }
    }
}
