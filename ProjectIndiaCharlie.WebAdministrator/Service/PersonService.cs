using ProjectIndiaCharlie.WebAdministrator.Models;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProjectIndiaCharlie.WebAdministrator.ViewModels.Services;

public static class PersonService
{
    private const string mediaType = "application/json";
    private const string baseUrl = "https://localhost:7073/api";
    //private const string baseUrl = "https://ee05-179-52-76-51.ngrok.io/api";
    private const string getPeopleUrl = $"{baseUrl}/People/List";
    private const string loginUrl = $"{baseUrl}/Student/Login";
    private const string registerStudentUrl = $"{baseUrl}/Student/Registration";

    private static readonly JsonSerializerOptions _options = new() { PropertyNameCaseInsensitive = true };

//    public static async Task<VStudentDetail?> Login(string studentId, string password)
//    {
//        try
//        {
//            using var httpClient = new HttpClient();
//            var response = await httpClient.GetAsync($"{loginUrl}?studentId={studentId}&password={password}");

//            if (!response.IsSuccessStatusCode)
//                return null;

//            var content = await response.Content.ReadAsStringAsync();

//            var logedStudent = JsonSerializer.Deserialize<VStudentDetail>(content, _options);
//            LogedStudent.Student = logedStudent!;
//            return logedStudent;
//        }
//        catch (Exception)
//        {
//            return null;
//        }
//    }


}
