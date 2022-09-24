using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProjectIndiaCharlie.Core.Models;
using System.Data;

namespace ProjectIndiaCharlie.Core.Data;

public partial class ProjectIndiaCharlieContext
{
    public async Task PasswordUpsert(int personId, PersonPassword password) => await Database.ExecuteSqlInterpolatedAsync($"Person.SP_PasswordUpsert {personId}, {password.PasswordHash}, {password.PasswordSalt}");

    public async Task PersonRegistration(NewPerson newperson) => await Database.ExecuteSqlInterpolatedAsync($"Person.SP_PersonRegistration {newperson.DocNo}, {newperson.FirstName}, {newperson.MiddleName}, {newperson.FirstSurname}, {newperson.SecondSurname}, {newperson.Gender}, {newperson.BirthDate}, {newperson.Email}");

    public async Task ProcessGradeRevision(int studentID, int subjectDetailID, int modifiedgradeId, int adminId) => await Database.ExecuteSqlInterpolatedAsync($"Academic.SP_ProcessGradeRevision {studentID}, {subjectDetailID}, {modifiedgradeId}, {adminId}");

    public async Task CareerRegistration(NewCareer newCareer) => await Database.ExecuteSqlInterpolatedAsync($"Academic.SP_CareerRegistration {newCareer.Name}, {newCareer.Code}, {newCareer.Subjects}, {newCareer.Credits}, {newCareer.Year}, {newCareer.IsActive}");

    public async Task PublishGrade(int studentId, int subjectDetailId, int gradeId) => await Database.ExecuteSqlInterpolatedAsync($"Academic.SP_PublishGrade {studentId}, {subjectDetailId}, {gradeId}");

    public async Task ProfessorRegistration(NewPerson newProfessor) => await Database.ExecuteSqlInterpolatedAsync($"Academic.SP_ProfessorRegistration {newProfessor.PersonId}");
    
    public async Task StudentRegistration(NewPerson newStudent) => await Database.ExecuteSqlInterpolatedAsync($"Academic.SP_StudentRegistration {newStudent.PersonId}, {newStudent.CareerId}");

    public async Task SubjectClassroomAssigment(int subjectDetailId, int classroomId) => await Database.ExecuteSqlInterpolatedAsync($"Academic.SP_SubjectClassroomAssignment {subjectDetailId}, {classroomId}");

    public async Task StudentSubjectElimination(int studentId, int subjectDetailId) => await Database.ExecuteSqlInterpolatedAsync($"Academic.SP_SubjectElimination {subjectDetailId}, {studentId}");

    public async Task SubjectRetirement(int studentId, int subjectDetailId) => await Database.ExecuteSqlInterpolatedAsync($"Academic.SP_SubjectRetirement {studentId}, {subjectDetailId}");

    public async Task<bool> StudentValidation(int studentId)
    {
        var studentValidation = new SqlParameter
        {
            ParameterName = "studentValidation",
            SqlDbType = SqlDbType.Bit,
            Direction = ParameterDirection.Output
        };
        await Database.ExecuteSqlInterpolatedAsync($"SELECT {studentValidation} = Academic.F_StudentValidation({studentId})");

        return (bool)studentValidation.Value;
    }
    
    public async Task<bool> ProfessorValidation(int professorId)
    {
        var professorValidation = new SqlParameter
        {
            ParameterName = "professorValidation",
            SqlDbType = SqlDbType.Bit,
            Direction = ParameterDirection.Output
        };
        await Database.ExecuteSqlInterpolatedAsync($"SELECT {professorValidation} = Academic.F_ProfessorValidation({professorId})");

        return (bool)professorValidation.Value;
    }

    public async Task<bool> DocNoValidation(string docNo)
    {
        var docNoValidation = new SqlParameter
        {
            ParameterName = "docNoValidation",
            SqlDbType = SqlDbType.Bit,
            Direction = ParameterDirection.Output
        };
        await Database.ExecuteSqlInterpolatedAsync($"SELECT {docNoValidation} = Academic.F_DocNoValidation({docNo})");

        return (bool)docNoValidation.Value;
    }

    public async Task<bool> SubjectSelection(int studentId, int subjectDetailId)
    {
        var flag = new SqlParameter()
        {
            ParameterName = "flag",
            SqlDbType = SqlDbType.Bit,
            Direction = ParameterDirection.Output
        };
        await Database.ExecuteSqlInterpolatedAsync($"EXEC {flag} = Academic.SP_SubjectSelection {subjectDetailId}, {studentId}");

        return (bool)flag.Value;
    }

    public async Task<bool> StudentSubjectValidation(int studentId, int subjectDetailId)
    {
        var flag = new SqlParameter()
        {
            ParameterName = "flag",
            SqlDbType = SqlDbType.Bit,
            Direction = ParameterDirection.Output
        };

        await Database.ExecuteSqlInterpolatedAsync($"SELECT {flag} = Academic.F_StudentSubjectValidation({subjectDetailId}, {studentId})");

        return (bool)flag.Value;
    }

    public async Task<bool> RequestGradeRevision(int studentID, int subjectDetailID)
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

    public async Task<string> SubjectScheduleValidation(int studentId, int subjectDetailId)
    {
        var subjectSchedule = await VSubjectSchedules
            .FirstOrDefaultAsync(s => s.SubjectDetailId == subjectDetailId);

        var scheduleValidation = new SqlParameter
        {
            ParameterName = "scheduleValidation",
            SqlDbType = SqlDbType.NVarChar,
            Size = 9,
            Direction = ParameterDirection.Output,
            IsNullable = true
        };
        await Database.ExecuteSqlInterpolatedAsync($"SELECT {scheduleValidation} = Academic.F_SubjectScheduleValidation({studentId}, {subjectSchedule!.WeekdayId}, {subjectSchedule.StartTime}, {subjectSchedule.EndTime})");

        return scheduleValidation.Value.ToString() ??
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

    public async Task<IEnumerable<VGradeRevision>> GetUnsolvedRevisions() => await VGradeRevisions.FromSqlInterpolated($"SELECT * FROM Academic.F_GetUnsolvedRevisions()")
        .ToListAsync();

    public async Task<IEnumerable<VSubjectSectionDetail>> GetSelectionSchedule(int studentId) => await VSubjectSectionDetails
        .FromSqlInterpolated($"SELECT * FROM Academic.F_GetSelectionSchedule({studentId})")
        .ToListAsync();

    public async Task<IEnumerable<VStudentSubject>> GetStudentSchedule(int studentId) => await VStudentSubjects
        .FromSqlInterpolated($"SELECT * FROM Academic.F_GetStudentCurrentSchedule({studentId})")
        .ToListAsync();

    public async Task<IEnumerable<VSubjectSectionDetail>> GetSubjectsOfProfessor(int professorId) => await VSubjectSectionDetails.FromSqlInterpolated($"SELECT * FROM Academic.F_GetSubjectsOfProfessor({professorId})")
        .ToListAsync();
}
