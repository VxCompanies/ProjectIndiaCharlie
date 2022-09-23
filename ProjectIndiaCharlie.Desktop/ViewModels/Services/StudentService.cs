using ProjectIndiaCharlie.Desktop.Models;
using ProjectIndiaCharlie.Desktop.ViewModels.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProjectIndiaCharlie.Desktop.ViewModels.Services;

public static class StudentService
{
    private const string mediaType = "application/json";
    private const string baseUrl = "https://localhost:7073/api/Student/";
    //private const string baseUrl = "https://ee05-179-52-76-51.ngrok.io/api/Student/";
    private const string loginUrl = $"{baseUrl}Login?studentId={{0}}&password={{1}}";
    private const string getSchedule = $"{baseUrl}Schedule?studentId={{0}}";
    private const string subjectSelection = $"{baseUrl}SubjectSelection?studentId={{0}}&subjectDetailID={{1}}";
    private const string subjectRetirement = $"{baseUrl}SubjectRetirement?studentId={{0}}&subjectDetailID={{1}}";
    private const string subjectElimination = $"{baseUrl}SubjectElimination?studentId={{0}}&subjectDetailID={{1}}";
    private const string requestGradeRevision = $"{baseUrl}RequestGradeRevision?studentId={{0}}&subjectDetailID={{1}}";

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

    public static async Task<IEnumerable<VStudentSubject>> GetSchedule(int studentId)
    {
        try
        {
            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(string.Format(getSchedule, studentId));

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

    public static async Task<string> SubjectSelection(int studentId, int subjectDetailId)
    {
        try
        {
            using var httpClient = new HttpClient();
            var response = await httpClient.PostAsync(string.Format(subjectSelection, studentId, subjectDetailId), null);

            var content = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<string>(content, _options)!;
        }
        catch (Exception e)
        {
            return e.Message;
        }
    }

    public static async Task<string> SubjectRetirement(int studentId, int subjectDetailId)
    {
        try
        {
            using var httpClient = new HttpClient();
            var response = await httpClient.PutAsync(string.Format(subjectRetirement, studentId, subjectDetailId), null);

            var content = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<string>(content, _options)!;
        }
        catch (Exception e)
        {
            return e.Message;
        }
    }

    public static async Task<string> SubjectElimination(int studentId, int subjectDetailId)
    {
        try
        {
            using var httpClient = new HttpClient();
            var response = await httpClient.DeleteAsync(string.Format(subjectElimination, studentId, subjectDetailId));

            var content = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<string>(content, _options)!;
        }
        catch (Exception e)
        {
            return e.Message;
        }
    }

    public static async Task<string> RequestGradeRevision(int studentId, int subjectDetailId)
    {
        try
        {
            using var httpClient = new HttpClient();
            var response = await httpClient.PostAsync(string.Format(requestGradeRevision, studentId, subjectDetailId), null);

            var content = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<string>(content, _options)!;
        }
        catch (Exception e)
        {
            return e.Message;
        }
    }
}
