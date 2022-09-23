using Microsoft.AspNetCore.Mvc.Formatters;
using ProjectIndiaCharlie.WebAdministrator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;


namespace ProjectIndiaCharlie.WebAdministrator.Service
{
    public class ProfessorService
    {
        private readonly static string baseUrl = Program.Configuration.GetConnectionString("AcademicsAPI");

        private const string mediaType = "application/json";

        private static string registerProfessorUrl = $"{baseUrl}/Professor/Registration";

        private static readonly JsonSerializerOptions _options = new() { PropertyNameCaseInsensitive = true };

        public static async Task<NewPerson?> RegisterPerson(NewPerson newPerson)
        {
            try
            {
                using var httpClient = new HttpClient();

                var json = JsonSerializer.Serialize(newPerson);
                var content1 = new StringContent(json, Encoding.UTF8, mediaType);

                var response = await httpClient.PostAsync(registerProfessorUrl, content1);

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
    }
}
