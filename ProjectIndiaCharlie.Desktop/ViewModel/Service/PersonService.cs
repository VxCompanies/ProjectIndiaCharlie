using ProjectIndiaCharlie.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProjectIndiaCharlie.Desktop.ViewModel.Service
{
    public static class PersonService
    {
        private const string baseUrl = "https://localhost:7073/api/People";
        private const string getPeopleUrl = $"{baseUrl}/GetPeople";
        private const string loginStudentUrl = $"https://localhost:7073/api/Student/LoginStudent";

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

        public static async Task<Student?> Login(Student student)
        {
            try
            {
                using var httpClient = new HttpClient();

                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(loginStudentUrl),
                    Content = new StringContent(JsonSerializer.Serialize(student, _options), Encoding.UTF8, MediaTypeNames.Application.Json)
                };

                var response = await httpClient.SendAsync(request);

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
