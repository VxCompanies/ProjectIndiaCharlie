using ProjectIndiaCharlie.WebAdministrator.Models;
using System.Text;
using System.Text.Json;

namespace ProjectIndiaCharlie.WebAdministrator.Service
{
    public class CareerService
    {
        private readonly static string baseUrl = Program.Configuration.GetConnectionString("AcademicsAPI");

        private const string mediaType = "application/json";

        private static string createCareerURL = $"{baseUrl}/Admin/CareerRegistration";
        private static string getSchedule = $"{baseUrl}/Student/Schedule?studentId={{0}}";

        private static string registerStudentUrl = $"{baseUrl}/Student/Registration";
        private static string getUnresolvedRevisions = $"{baseUrl}/Admin/GetUnsolvedRevisions";
        private static string getGrades = $"{baseUrl}/Academic/GradesList";
        private static string processRevisionURL = $"{baseUrl}/Admin/ProcessGradeRevisions?studentID={{0}}&subjectDetailID={{1}}&modifiedgradeId={{2}}&adminId={{3}}";

        private static readonly JsonSerializerOptions _options = new() { PropertyNameCaseInsensitive = true };

        public static async Task<NewCareer?> RegisterCareer(NewCareer newCareer)
        {
            try
            {
                using var httpClient = new HttpClient();

                var json = JsonSerializer.Serialize(newCareer);
                var content1 = new StringContent(json, Encoding.UTF8, mediaType);

                var response = await httpClient.PostAsync(createCareerURL, content1);

                if (!response.IsSuccessStatusCode)
                    return null;

                var content2 = await response.Content.ReadAsStringAsync();
                var createdCareer = JsonSerializer.Deserialize<NewCareer>(content2, _options);

                return createdCareer!;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
