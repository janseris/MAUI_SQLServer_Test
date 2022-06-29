using DAL.Shared;

using System.Data.SqlClient;
using System.Diagnostics;

namespace DAL.SqlClient.System
{
    public class DAO : DAOBase
    {
        public override int GetDBCallResult(string connectionString)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                Trace.WriteLine($"Connection: {connection.ClientConnectionId}");
                var command = new SqlCommand(Constants.SQLQuery);
                command.Connection = connection;
                var numberOfUsers = command.ExecuteNonQuery();
                return numberOfUsers;
            }
        }
    }
}
