using DAL.Shared;

using System.Data.SqlClient;
using System.Diagnostics;

namespace DAL.SqlClient.System
{
    public class DAO : DAOBase
    {
        public override (int, string) GetDBCallResult(string connectionString)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                //TEST
                //SqlConnection.ClearPool(connection);

                connection.Open();
                var command = new SqlCommand(Constants.SQLQuery);
                command.Connection = connection;
                var numberOfUsers = command.ExecuteNonQuery();
                return (numberOfUsers, connection.ClientConnectionId.ToString());
            }
        }
    }
}
