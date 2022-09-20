using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProjectIndiaCharlie.Core.Models;
using System.Data;

namespace ProjectIndiaCharlie.Core.Data;

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

        await Database.ExecuteSqlInterpolatedAsync($"SELECT {passwordSalt} = Person.F_GetPasswordSalt({personId})");

        return passwordSalt.Value
            .ToString() ??
            string.Empty;
    }

    public async Task<VStudentDetail?> StudentLogin(int studentId, string passwordHash) => await VStudentDetails
        .FromSqlInterpolated($"SELECT * FROM Academic.F_StudentLogin({studentId}, {passwordHash})")
        .FirstOrDefaultAsync();

    public async Task<VProfessorDetail?> ProfessorLogin(int professorId, string passwordHash) => await VProfessorDetails
        .FromSqlInterpolated($"SELECT * FROM Academic.F_ProfessorLogin({professorId}, {passwordHash})")
        .FirstOrDefaultAsync();

    public async Task<VAdministratorDetail?> AdminLogin(int adminId, string passwordHash) => await VAdministratorDetails
        .FromSqlInterpolated($"SELECT * FROM Academic.F_AdminLogin({adminId}, {passwordHash})")
        .FirstOrDefaultAsync();

    public async Task PersonRegistration(NewPerson newperson) => await Database.ExecuteSqlInterpolatedAsync($"Person.SP_PersonRegistration {newperson.DocNo}, {newperson.FirstName}, {newperson.MiddleName}, {newperson.FirstSurname}, {newperson.SecondSurname}, {newperson.Gender}, {newperson.BirthDate}, {newperson.Email}");

    public async Task StudentRegistration(NewPerson newStudent) => await Database.ExecuteSqlInterpolatedAsync($"Academic.SP_StudentRegistration {newStudent.PersonId}, {newStudent.CareerId}");

    public async Task ProfessorRegistration(NewPerson newProfessor) => await Database.ExecuteSqlInterpolatedAsync($"Academic.SP_ProfessorRegistration {newProfessor.PersonId}, {newProfessor.CareerId}");

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

    public async Task<bool> SubjectSelection(int subjectId, int studentId)
    {
        var flag = new SqlParameter()
        {
            ParameterName = "flag",
            SqlDbType = SqlDbType.Int,
            Direction = ParameterDirection.Output
        };
        await Database.ExecuteSqlInterpolatedAsync($"EXEC {flag} = Academic.SP_SubjectSelection {subjectId}, {studentId}");

        return (bool)flag.Value;
    }

    public async Task<bool> SubjectRetirement(int subjectId, int studentId)
    {
        var flag = new SqlParameter()
        {
            ParameterName = "flag",
            SqlDbType = SqlDbType.Int,
            Direction = ParameterDirection.Output
        };
        await Database.ExecuteSqlInterpolatedAsync($"EXEC {flag} = Academic.SP_SubjectRetirement {studentId}, {subjectId}");

        return (bool)flag.Value;
    }

    public async Task StudentSubjectElimination(int subjectDetailID, int studentID) => await Database.ExecuteSqlInterpolatedAsync($"Academic.SP_SubjectElimination {subjectDetailID}, {studentID}");

    public async Task<IEnumerable<VStudentSubject>> GetStudentSchedule(int studentId) => await VStudentSubjects
        .FromSqlInterpolated($"Academic.SP_GetLastTrimesterStudentsSchedule {studentId}")
        .ToListAsync();

    public async Task<bool> StudentSubjectValidation(int subjectDetailID, int studentID)
    {
        var flag = new SqlParameter()
        {
            ParameterName = "flag",
            SqlDbType = SqlDbType.Bit,
            Direction = ParameterDirection.Output
        };

        await Database.ExecuteSqlInterpolatedAsync($"SELECT {flag} = Academic.F_StudentSubjectValidation({subjectDetailID}, {studentID})");

        return (bool)flag.Value;
    }

    public async Task GetUnsolvedRevisions() => await Database.ExecuteSqlInterpolatedAsync($"Academic.SP_RequestGradeRevision");

    //public async Task<IEnumerable<vGrades>> GetGrades() => await VGrades.ToListAsync();

    public async Task<bool> RequestGradeRevision(int subjectDetailID, int studentID)
    {
        var flag = new SqlParameter()
        {
            ParameterName = "flag",
            SqlDbType = SqlDbType.Bit,
            Direction = ParameterDirection.Output
        };

        await Database.ExecuteSqlInterpolatedAsync($"EXEC {flag} = Academic.SP_RequestGradeRevision {studentID}, {subjectDetailID}");

        return (bool)flag.Value;
    }
}
