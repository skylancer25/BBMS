using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using BBMS_1.Models;
namespace BBMS_1.Data
{
    public class UserRepository : IUserInterface
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public int ValidateUser(string username, string password)
        {
            bool isValidUser = false;
            int result = 0;
            var connection = _context.Database.GetDbConnection();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "ValidateUserLogin";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@Username", SqlDbType.NVarChar) { Value = username });
                command.Parameters.Add(new SqlParameter("@Password", SqlDbType.NVarChar) { Value = password });

                // Add output parameter to get return value
                var returnParameter = new SqlParameter();
                returnParameter.ParameterName = "@ReturnValue";
                returnParameter.SqlDbType = SqlDbType.Int;
                returnParameter.Direction = ParameterDirection.Output;
                command.Parameters.Add(returnParameter);

                connection.Open();
                command.ExecuteNonQuery(); // Execute the stored procedure
                connection.Close();

                // Retrieve return value (1 = Success, 0 = Failure)
                result = (int)returnParameter.Value;
                isValidUser = (result > 0);
            }
            if (isValidUser)
            {
                return result;
            }
            else
            {
                return -1;
            }
        }

    }
}
