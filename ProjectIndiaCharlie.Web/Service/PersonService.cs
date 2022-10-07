using ProjectIndiaCharlie.Web.Models;
using System.Text;
using System.Text.Json;

namespace ProjectIndiaCharlie.Web.Service;

public static class PersonService
{
    private const string mediaType = "application/json";
    //private const string baseUrl = "https://05d2-190-80-246-215.ngrok.io/api";
    private const string baseUrl = "https://localhost:7073/api/Professor";
    private const string getSubjectsUrl = $"{baseUrl}/SubjectSections?professorId={{0}}";
    private const string loginUrl = $"{baseUrl}/Login";

    private static readonly JsonSerializerOptions _options = new() { PropertyNameCaseInsensitive = true };

    public static async Task<IEnumerable<VSubjectSectionDetail>> GetSubjectSections()
    {
        try
        {
            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(string.Format(getSubjectsUrl, getSubjectsUrl));

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

    public static async Task<VProfessorDetail?> Login(string personId, string password)
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
}
