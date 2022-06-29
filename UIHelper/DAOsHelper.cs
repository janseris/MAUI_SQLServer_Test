using DAL.Shared;

using System.Diagnostics;

namespace UIHelper
{
    public class DAOsHelper
    {
        public static readonly DAOBase EFCore6OlderDAO = new DAL.EFCore6.Older.DAO() { Name = "Microsoft.Data.SqlClient 2.1.4 (EF Core 6.0.1)" };
        public static readonly DAOBase EFCore6CurrentDAO = new DAL.EFCore6.Current.DAO() { Name = "Microsoft.Data.SqlClient 2.1.4 (EF Core 6.0.6)" };
        public static readonly DAOBase EFCore7PreviewDAO = new DAL.EFCore.Preview.DAO() { Name = "Microsoft.Data.SqlClient 5.0.0-preview2.22096.2 (EF Core 7.0.0-preview.5.22302.2)" };

        public static readonly DAOBase MicrosoftSqlClientOldVersionDAO = new DAL.SqlClient.Microsoft.OldVersion.DAO() { Name = "Microsoft.Data.SqlClient 4.1.0" };
        public static readonly DAOBase MicrosoftSqlClientCurrentVersionDAO = new DAL.SqlClient.Microsoft.DAO() { Name = "Microsoft.Data.SqlClient 2.0.0" };
        public static readonly DAOBase MicrosoftSqlClientPreviewVersionDAO = new DAL.SqlClient.Microsoft.Preview.DAO() { Name = "Microsoft.Data.SqlClient 5.0.0-preview3.22168.1" };

        public static readonly DAOBase SystemSqlClientVersionDAO = new DAL.SqlClient.Microsoft.Preview.DAO() { Name = "System.Data.SqlClient 4.8.3" };


        public static List<DAOBase> DAOs
        {
            get => new List<DAOBase>
        {
            EFCore6OlderDAO, EFCore6CurrentDAO, EFCore7PreviewDAO,
            MicrosoftSqlClientOldVersionDAO, MicrosoftSqlClientCurrentVersionDAO, MicrosoftSqlClientPreviewVersionDAO,
            SystemSqlClientVersionDAO
        };


        }


        /// <summary>
        /// Executes a DB call and gets the result as a formatted string (success or error message if an <see cref="Exception"/> was thrown)
        /// <br>Duration in milliseconds is prepended to the string.</br>
        /// </summary>
        /// <param name="dao"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static string ExecuteDAO(DAOBase dao, string connectionString)
        {
            Stopwatch s = Stopwatch.StartNew();
            try
            {
                var result = dao.GetDBCallResult(connectionString);
                var duration = StopAndGetDuration(s);
                return $"{duration} success [connection ID: {result.Item2}]";
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