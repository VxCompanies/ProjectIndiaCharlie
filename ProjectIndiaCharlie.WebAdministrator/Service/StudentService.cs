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

    private const string mediaType = "application/json";

    private static string loginUrl = $"{baseUrl}/Student/Login";
    private static string getSchedule = $"{baseUrl}/Student/Schedule?studentId={{0}}";

    private static string registerStudentUrl = $"{baseUrl}/Student/Registration";
    private static string getUnresolvedRevisions = $"{baseUrl}/Admin/GetUnsolvedRevisions";
    private static string processRevisionURL = $"{baseUrl}/Admin/ProcessGradeRevisions?studentID={{0}}&subjectDetailID={{1}}&modifiedgradeId={{2}}&adminId={{3}}";

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

    public static async Task<IEnumerable<VGradeRevision>> GetUnresolvedRevisions()
    {
        try
        {
            using var httpClient = new HttpClient();

            var response = await httpClient.GetAsync(getUnresolvedRevisions);

            if (!response.IsSuccessStatusCode)
                return null;

            var content = await response.Content.ReadAsStringAsync();
            var revisions = JsonSerializer.Deserialize<IEnumerable<VGradeRevision>>(content, _options);

            return revisions!;
        }
        catch (Exception)
        {
            return null;
        }
    }

    public static async Task<string> ProcessRevision(VGradeRevision newRevision)
    {
        try
        {
            using var httpClient = new HttpClient();

            var json = JsonSerializer.Serialize(newRevision);

            var response = await httpClient.PutAsync(string.Format(processRevisionURL, newRevision.PersonId, newRevision.SubjectDetailId, newRevision.ModifiedGradeId, newRevision.Admin), null);

            if (!response.IsSuccessStatusCode)
                return "Oooops";

            var content2 = await response.Content.ReadAsStringAsync();
            //var madeRevision = JsonSerializer.Deserialize<string>(content2, _options);

            return content2!;
        }
        catch (Exception e)
        {
            return e.Message;
        }
    }
}
