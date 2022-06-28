﻿using DAL.Shared;

using Microsoft.Data.SqlClient;

namespace DAL.SqlClient.Microsoft.Preview
{
    public class UserDAO : UserDAOBase
    {
        public int GetUsersCount(string connectionString)
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
