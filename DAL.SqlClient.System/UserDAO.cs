using DAL.Shared;

using System.Data.SqlClient;

namespace DAL.SqlClient.System
{
    public class UserDAO : UserDAOBase
    {
        public override int GetUsersCount(string connectionString)
        {
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
