using DAL.Shared;

using Microsoft.Data.SqlClient;

namespace DAL.EFCore.Preview
{
    public class DAO : DAOBase
    {
        public override int GetDBCallResult(string connectionString)
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
