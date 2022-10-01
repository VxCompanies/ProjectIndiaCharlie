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
        private static string getCareersURL = $"{baseUrl}/Admin/CareersList";

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
        public static async Task<IEnumerable<VAvailableCareer>> GetCareers()
        {
            try
            {
                using var httpClient = new HttpClient();
                var response = await httpClient.GetAsync(string.Format(getCareersURL));

                if (!response.IsSuccessStatusCode)
                    return Enumerable.Empty<VAvailableCareer>();

                var content = await response.Content.ReadAsStringAsync();

                return JsonSerializer.Deserialize<IEnumerable<VAvailableCareer>>(content, _options)!;
            }
            catch (Exception)
            {
                return Enumerable.Empty<VAvailableCareer>();
            }
        }
    }
}
