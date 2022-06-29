using DAL.Shared;

using Microsoft.Data.SqlClient;

namespace DAL.SqlClient.Microsoft.OldVersion
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
