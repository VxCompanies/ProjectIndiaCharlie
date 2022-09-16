using ProjectIndiaCharlie.Web.Models;
using System.Text;
using System.Text.Json;

namespace ProjectIndiaCharlie.Web.Service;

public static class PersonService
{
    private const string mediaType = "application/json";
    //private const string baseUrl = "https://05d2-190-80-246-215.ngrok.io/api";
    private const string baseUrl = "https://localhost:7073/api";
    private const string getPeopleUrl = $"{baseUrl}/Academic/SubjectSections";
    private const string loginUrl = $"{baseUrl}/Login";
    private const string registerStudentUrl = $"{baseUrl}/Student/Registration";

    private static readonly JsonSerializerOptions _options = new() { PropertyNameCaseInsensitive = true };

    public static async Task<IEnumerable<VSubjectSectionDetail>> GetSubjectSectionDetailsAsync()
    {
        try
        {
            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(getPeopleUrl);

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
