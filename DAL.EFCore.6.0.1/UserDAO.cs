using DAL.EFCore6.Current.Models;
using DAL.Shared;

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace DAL.EFCore6.Older
{
    public class UserDAO : UserDAOBase
    {
        public override int GetUsersCount(string connectionString)
        {
            DbContextOptionsBuilder<OrdinaceContext> builder = new DbContextOptionsBuilder<OrdinaceContext>();
            builder.UseSqlServer(connectionString);
            var parameters = builder.Options;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand(Constants.SQLQuery);
                command.Connection = connection;
                var numberOfUsers = command.ExecuteNonQuery();
                return numberOfUsers;
            }
        }
    }
}
