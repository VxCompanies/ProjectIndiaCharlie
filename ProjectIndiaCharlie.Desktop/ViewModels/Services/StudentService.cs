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
    //private const string baseUrl = "https://localhost:7073/api/Student/";
    private const string baseUrl = "https://94e6-190-122-96-78.ngrok.io/api/Student/";
    private const string loginUrl = $"{baseUrl}Login?studentId={{0}}&password={{1}}";
    private const string getSchedule = $"{baseUrl}Schedule?studentId={{0}}";
    private const string subjectSelection = $"{baseUrl}SubjectSelection?studentId={{0}}&subjectDetailId={{1}}";
    private const string subjectRetirement = $"{baseUrl}SubjectRetirement?studentId={{0}}&subjectDetailId={{1}}";
    private const string subjectElimination = $"{baseUrl}SubjectElimination?studentId={{0}}&subjectDetailId={{1}}";
    private const string requestGradeRevision = $"{baseUrl}RequestGradeRevision?studentId={{0}}&subjectDetailId={{1}}";
    private const string getSelectionSchedule = $"{baseUrl}GetSelectionSchedule?studentId={{0}}";
    private const string getSelectionSubjects = $"{baseUrl}GetSelectionSubjects";
    private const string getPendingRevisions = $"{baseUrl}GetPendingRevisions?studentId={{0}}";
    private const string getRequestableSubjects = $"{baseUrl}GetRequestableSubjects?studentId={{0}}";

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
    
    public static async Task<IEnumerable<VSubjectStudent>> GetRetirableSubjects()
    {
        try
        {
            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(string.Format(getRequestableSubjects, LogedStudent.Student!.PersonId));

            if (!response.IsSuccessStatusCode)
                return Enumerable.Empty<VSubjectStudent>();

            var content = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<IEnumerable<VSubjectStudent>>(content, _options)!;
        }
        catch (Exception)
        {
            return Enumerable.Empty<VSubjectStudent>();
        }
    }

    public static async Task<string> SubjectSelection(int subjectDetailId)
    {
        try
        {
            using var httpClient = new HttpClient();
            var response = await httpClient.PostAsync(string.Format(subjectSelection, LogedStudent.Student!.PersonId, subjectDetailId), null);

            return await response.Content.ReadAsStringAsync();
        }
        catch (Exception e)
        {
            return e.Message;
        }
    }

    public static async Task<string> SubjectRetirement(int subjectDetailId)
    {
        try
        {
            using var httpClient = new HttpClient();
            var response = await httpClient.PutAsync(string.Format(subjectRetirement, LogedStudent.Student!.PersonId, subjectDetailId), null);

            return await response.Content.ReadAsStringAsync();
        }
        catch (Exception e)
        {
            return e.Message;
        }
    }

    public static async Task<string> SubjectElimination(int subjectDetailId)
    {
        try
        {
            using var httpClient = new HttpClient();
            var response = await httpClient.DeleteAsync(string.Format(subjectElimination, LogedStudent.Student!.PersonId, subjectDetailId));

            return await response.Content.ReadAsStringAsync();

        }
        catch (Exception e)
        {
            return e.Message;
        }
    }

    public static async Task<string> RequestGradeRevision(int subjectDetailId)
    {
        try
        {
            using var httpClient = new HttpClient();
            var response = await httpClient.PostAsync(string.Format(requestGradeRevision, LogedStudent.Student!.PersonId, subjectDetailId), null);

            return await response.Content.ReadAsStringAsync();
        }
        catch (Exception e)
        {
            return e.Message;
        }
    }

    public static async Task<IEnumerable<VSubjectSectionDetail>> GetSelectionSchedule()
    {
        try
        {
            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(string.Format(getSelectionSchedule, LogedStudent.Student!.PersonId));

            if (!response.IsSuccessStatusCode)
                return Enumerable.Empty<VSubjectSectionDetail>();

            var content = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<IEnumerable<VSubjectSectionDetail>>(content, _options)!;
        }
        catch (Exception e)
        {
            return Enumerable.Empty<VSubjectSectionDetail>();
        }
    }

    public static async Task<IEnumerable<VSubjectSectionDetail>> GetSelectionSubjects()
    {
        try
        {
            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(getSelectionSubjects);

            if (!response.IsSuccessStatusCode)
                return Enumerable.Empty<VSubjectSectionDetail>();

            var content = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<IEnumerable<VSubjectSectionDetail>>(content, _options)!;
        }
        catch (Exception e)
        {
            return Enumerable.Empty<VSubjectSectionDetail>();
        }
    }

    public static async Task<IEnumerable<VGradeRevision>> GetPendingGradeRequests()
    {
        try
        {
            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(string.Format(getPendingRevisions, LogedStudent.Student!.PersonId));

            if (!response.IsSuccessStatusCode)
                return Enumerable.Empty<VGradeRevision>();

            var content = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<IEnumerable<VGradeRevision>>(content, _options)!;
        }
        catch (Exception)
        {
            return Enumerable.Empty<VGradeRevision>();
        }
    }
}
