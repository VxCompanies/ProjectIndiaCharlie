using ProjectIndiaCharlie.Mobile.Models;
using ProjectIndiaCharlie.Mobile.ViewModels.Stores;
using System.Text.Json;

namespace ProjectIndiaCharlie.Mobile.ViewModels.Services;

public static class StudentService
{
    private const string baseUrl = "https://localhost:7073/api/Student/";
    //private const string baseUrl = "https://94e6-190-122-96-78.ngrok.io/api/Student/";
    private const string loginUrl = $"{baseUrl}Login?studentId={{0}}&password={{1}}";
    private const string getSchedule = $"{baseUrl}Schedule?studentId={{0}}";

    private static readonly JsonSerializerOptions _options = new() { PropertyNameCaseInsensitive = true };

    public static async Task<bool> Login(string studentId, string password)
    {
        try
        {
            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(string.Format(loginUrl, studentId, password));

            if (!response.IsSuccessStatusCode)
                return false;

            var content = await response.Content.ReadAsStringAsync();

            var logedStudent = JsonSerializer.Deserialize<VStudentDetail>(content, _options);
            LogedStudent.Student = logedStudent!;
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public static async Task<IEnumerable<VStudentSubject>> GetSchedule()
    {
        try
        {
            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(string.Format(getSchedule, LogedStudent.Student!.PersonId));

            if (!response.IsSuccessStatusCode)
                return Enumerable.Empty<VStudentSubject>();

            var content = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<IEnumerable<VStudentSubject>>(content, _options)!;
        }
        catch (Exception)
        {
            return Enumerable.Empty<VStudentSubject>();
        }
    }
}
