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

        public Task<VStudentDetail?> StudentLogin(int personId, string passwordHash) => Task.FromResult(VStudentDetails.FromSqlInterpolated($"SELECT * FROM Academic.F_StudentLogin({personId}, {passwordHash})")
               .AsEnumerable()
               .FirstOrDefault());
    }
}
