using ProjectIndiaCharlie.Desktop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProjectIndiaCharlie.Desktop.ViewModels.Service
{
    public static class PersonService
    {
        private const string baseUrl = "https://localhost:7073/api/Person";
        private const string getPeopleUrl = $"{baseUrl}/GetPeople";
        private const string loginStudentUrl = $"{baseUrl}/Login";

        private static readonly JsonSerializerOptions _options = new() { PropertyNameCaseInsensitive = true };

        public static async Task<IEnumerable<Person>> GetPeopleAsync()
        {
            try
            {
                using var httpClient = new HttpClient();
                var response = await httpClient.GetAsync(getPeopleUrl);

                if (!response.IsSuccessStatusCode)
                    return Enumerable.Empty<Person>();

                var content = await response.Content.ReadAsStringAsync();

                return JsonSerializer.Deserialize<IEnumerable<Person>>(content, _options)!;
            }
            catch (Exception)
            {
                return Enumerable.Empty<Person>();
            }
        }

        public static async Task<Student?> Login(string personId, string password)
        {
            try
            {
                using var httpClient = new HttpClient();
                var response = await httpClient.GetAsync($"{loginStudentUrl}?personId={personId}&password={password}");

                if (!response.IsSuccessStatusCode)
                    return null;

                var content = await response.Content.ReadAsStringAsync();

                return JsonSerializer.Deserialize<Student>(content, _options);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
