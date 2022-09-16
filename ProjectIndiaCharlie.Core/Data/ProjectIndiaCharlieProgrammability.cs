using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProjectIndiaCharlie.Core.Models;
using System.Data;

namespace ProjectIndiaCharlie.Core.Data
{
    public partial class ProjectIndiaCharlieContext
    {
        public async Task<string> GetPasswordSalt(int personId)
        {
            var passwordSalt = new SqlParameter()
            {
                ParameterName = "passwordSalt",
                SqlDbType = SqlDbType.NVarChar,
                Size = 5,
                Direction = ParameterDirection.Output
            };

            await Database.ExecuteSqlInterpolatedAsync($"SELECT {passwordSalt} = Academic.F_GetPasswordSalt({personId})");

            return passwordSalt.Value
                .ToString() ??
                string.Empty;
        }

        public async Task<VStudentDetail?> StudentLogin(int personId, string passwordHash) => await VStudentDetails
            .FromSqlInterpolated($"SELECT * FROM Academic.F_StudentLogin({personId}, {passwordHash})")
            .FirstOrDefaultAsync();

        public async Task<VProfessorDetail?> ProfessorLogin(int personId, string passwordHash) => await VProfessorDetails
            .FromSqlInterpolated($"SELECT * FROM Academic.F_ProfessorLogin({personId}, {passwordHash})")
            .FirstOrDefaultAsync();

        public async Task PersonRegistration(NewPerson newperson) => await Database.ExecuteSqlInterpolatedAsync($"Person.SP_PersonRegistration {newperson.DocNo}, {newperson.FirstName}, {newperson.MiddleName}, {newperson.FirstSurname}, {newperson.SecondSurname}, {newperson.Gender}, {newperson.BirthDate}, {newperson.Email}");

        public async Task StudentRegistration(NewPerson newStudent) => await Database.ExecuteSqlInterpolatedAsync($"Academic.SP_StudentRegistration {newStudent.PersonId}, {newStudent.CareerId}");

        public async Task PasswordUpsert(int personId, PersonPassword password) => await Database.ExecuteSqlInterpolatedAsync($"Person.SP_PasswordUpsert {personId}, {password.PasswordHash}, {password.PasswordSalt}");

        public async Task<string> SubjectScheduleValidation(int subjectId, int studentId)
        {
            var subjectSchedule = await VSubjectSchedules
                .FirstOrDefaultAsync(s => s.SubjectDetailId == subjectId);

            var subjectValidation = new SqlParameter
            {
                ParameterName = "subjectValidation",
                SqlDbType = SqlDbType.NVarChar,
                Size = 9,
                Direction = ParameterDirection.Output,
                IsNullable = true
            };
            await Database.ExecuteSqlInterpolatedAsync($"SELECT {subjectValidation} = Academic.F_SubjectScheduleValidation({studentId}, {subjectSchedule!.WeekdayId}, {subjectSchedule.StartTime}, {subjectSchedule.EndTime})");

            return subjectValidation.Value.ToString() ??
                string.Empty;
        }

        public async Task SubjectSelection(int subjectId, int studentId) => await Database.ExecuteSqlInterpolatedAsync($"Academic.SP_SubjectSelection {subjectId}, {studentId}");
    }
}
