using DAL.Shared;

using System.Diagnostics;

namespace UIHelper
{
    public class DAOsHelper
    {
        public static readonly UserDAOBase EFCore6OlderDAO = new DAL.EFCore6.Older.UserDAO() { Name = "Microsoft.Data.SqlClient 2.1.4 (EF Core 6.0.1)" };
        public static readonly UserDAOBase EFCore6CurrentDAO = new DAL.EFCore6.Current.UserDAO() { Name = "Microsoft.Data.SqlClient 2.1.4 (EF Core 6.0.6)" };
        public static readonly UserDAOBase EFCore7PreviewDAO = new DAL.EFCore.Preview.UserDAO() { Name = "Microsoft.Data.SqlClient 5.0.0-preview2.22096.2 (EF Core 7.0.0-preview.5.22302.2)" };

        public static readonly UserDAOBase microsoftSqlClientOldVersionDAO = new DAL.SqlClient.Microsoft.OldVersion.UserDAO() { Name = "Microsoft.Data.SqlClient 4.1.0" };
        public static readonly UserDAOBase microsoftSqlClientCurrentVersionDAO = new DAL.SqlClient.Microsoft.UserDAO() { Name = "Microsoft.Data.SqlClient 2.0.0" };
        public static readonly UserDAOBase microsoftSqlClientPreviewVersionDAO = new DAL.SqlClient.Microsoft.Preview.UserDAO() { Name = "Microsoft.Data.SqlClient 5.0.0-preview3.22168.1" };

        public static readonly UserDAOBase systemSqlClientVersionDAO = new DAL.SqlClient.Microsoft.Preview.UserDAO() { Name = "System.Data.SqlClient 4.8.3" };


        public static List<UserDAOBase> DAOs
        {
            get => new List<UserDAOBase>
        {
            EFCore6OlderDAO, EFCore6CurrentDAO, EFCore7PreviewDAO,
            microsoftSqlClientOldVersionDAO, microsoftSqlClientCurrentVersionDAO, microsoftSqlClientPreviewVersionDAO,
            systemSqlClientVersionDAO
        };


        }


        /// <summary>
        /// Executes a DB call and gets the result as a formatted string (success or error message if an <see cref="Exception"/> was thrown)
        /// <br>Duration in milliseconds is prepended to the string.</br>
        /// </summary>
        /// <param name="dao"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static string ExecuteDAO(UserDAOBase dao, string connectionString)
        {
            Stopwatch s = Stopwatch.StartNew();
            try
            {
                dao.GetUsersCount(connectionString);
                var duration = StopAndGetDuration(s);
                return $"{duration} success";
            }
            catch (Exception ex)
            {
                var duration = StopAndGetDuration(s);
                if (ex.Message == Constants.AndroidSqlClientBugMessage)
                {
                    return $"{duration} Android connection bug";
                }
                if (ex.Message == Constants.ConnectionFailedAndroidMessage || ex.Message == Constants.ConnectionFailedWindowsMessage)
                {
                    return $"{duration} Connection failed, check internet and connection string and server configuration (accessibility from outside world)";
                }
                return $"{duration} {GetExceptionMessage(ex)}";
            }
        }

        private static string GetExceptionMessage(Exception ex)
        {
            if (ex is Microsoft.Data.SqlClient.SqlException)
            {
                var sex = ex as Microsoft.Data.SqlClient.SqlException;
                return $"{sex.GetType()} Class: {sex.Class}, Number: {sex.Number}, Message: {sex.Message}";
            }
            if (ex is System.Data.SqlClient.SqlException) //why not available on Android?
            {
                var sex = ex as Microsoft.Data.SqlClient.SqlException;
                return $"{sex.GetType()} Class: {sex.Class}, Number: {sex.Number}, Message: {sex.Message}";
            }
            return $"{ex.GetType()}, {ex.Message}";
        }

        private static string StopAndGetDuration(Stopwatch s)
        {
            s.Stop();
            return $"[{s.ElapsedMilliseconds} ms]";
        }

    }
}