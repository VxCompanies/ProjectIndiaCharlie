using ProjectIndiaCharlie.Desktop.Models;
using ProjectIndiaCharlie.Desktop.ViewModels.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProjectIndiaCharlie.Desktop.ViewModels.Service
{
    public class AcademicService
    {
        private const string mediaType = "application/json";
        private const string baseUrl = "https://localhost:7073/api/Academic";
        private const string getStudentSubjectsUrl = $"{baseUrl}/Student/Subjects";

        private static readonly JsonSerializerOptions _options = new() { PropertyNameCaseInsensitive = true };
        public static async Task<IEnumerable<SubjectStudent>> GetStudentSubjects(string personId)
        {
            try
            {
                using var httpClient = new HttpClient();
                var response = await httpClient.GetAsync($"{getStudentSubjectsUrl}?studentId={personId}");

                if (!response.IsSuccessStatusCode)
                    return Enumerable.Empty<SubjectStudent>();

                var content = await response.Content.ReadAsStringAsync();

                var subjectStudent = JsonSerializer.Deserialize<IEnumerable<SubjectStudent>>(content, _options);
                LogedStudent.StudentSubjects = subjectStudent!;
                return subjectStudent!;
            }
            catch (Exception)
            {
                return Enumerable.Empty<SubjectStudent>();
            }
        }
    }
}
