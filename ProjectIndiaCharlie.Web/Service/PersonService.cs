using ProjectIndiaCharlie.Web.Models;
using ProjectIndiaCharlie.Web.Store;
using System.Text.Json;

namespace ProjectIndiaCharlie.Web.Service;

public static class PersonService
{
    private const string mediaType = "application/json";
    //private const string baseUrl = "https://05d2-190-80-246-215.ngrok.io/api";
    private const string baseUrl = "https://localhost:7073/api/Professor";
    private const string getSubjectsUrl = $"{baseUrl}/SubjectSections?professorId={{0}}";
    private const string loginUrl = $"{baseUrl}/Login";
    private const string getStudentsOfSubjectsUrl = $"{baseUrl}/GetStudentsOfSubjects?subjectDetailId={{0}}";
    private const string publishGradeUrl = $"{baseUrl}/PublishGrade?studentId={{0}}&subjectDetailId={{1}}&grade{{2}}";

    private static readonly JsonSerializerOptions _options = new() { PropertyNameCaseInsensitive = true };

    public static async Task<IEnumerable<VSubjectSectionDetail>> GetSubjectSections()
    {
        try
        {
            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(string.Format(getSubjectsUrl, 1110201));

            if (!response.IsSuccessStatusCode)
                return Enumerable.Empty<VSubjectSectionDetail>();

            var content = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<IEnumerable<VSubjectSectionDetail>>(content, _options)!;
        }
        catch (Exception)
        {
            return Enumerable.Empty<VSubjectSectionDetail>();
        }
    }
    
    public static async Task<IEnumerable<VSubjectStudent>> GetStudentsOfSubjects(int subjectDetailId)
    {
        try
        {
            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(string.Format(getStudentsOfSubjectsUrl, subjectDetailId));

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
    
    //public static async Task<string> GetStudent(int studentId)
    //{
    //    try
    //    {
    //        using var httpClient = new HttpClient();
    //        var response = await httpClient.PostAsync(string.Format(publishGradeUrl, gradeDetail.StudentId, gradeDetail.SubjectDetailId, gradeDetail.Grade), null);

    //        var content = await response.Content.ReadAsStringAsync();

    //        return JsonSerializer.Deserialize<string>(content, _options)!;
    //    }
    //    catch (Exception e)
    //    {
    //        return e.Message;
    //    }
    //}
    
    public static async Task<string> PublishGrade(GradeDetail gradeDetail)
    {
        try
        {
            using var httpClient = new HttpClient();
            var response = await httpClient.PostAsync(string.Format(publishGradeUrl, gradeDetail.StudentId, gradeDetail.SubjectDetailId, gradeDetail.Grade), null);

            return await response.Content.ReadAsStringAsync();
        }
        catch (Exception e)
        {
            return e.Message;
        }
    }

    public static async Task<bool> Login(LoginToken loginToken)
    {
        try
        {
            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync($"{loginUrl}?personId={loginToken.ProfessorId}&password={loginToken.Password}");

            if (!response.IsSuccessStatusCode)
                return false;

            var content = await response.Content.ReadAsStringAsync();

            LogedProfessor.Professor = JsonSerializer.Deserialize<VProfessorDetail>(content, _options);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public static Task Logout() => Task.Run(() => LogedProfessor.Professor = null);
}
