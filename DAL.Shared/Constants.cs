namespace DAL.Shared
{
    public class Constants
    {
        public const string AndroidSqlClientBugMessage = "A connection was successfully established with the server, but then an error occurred during the pre-login handshake. (provider: TCP Provider, error: 35 - An internal exception was caught)";
        public const string ConnectionFailedAndroidMessage = "A network-related or instance-specific error occurred while establishing a connection to SQL Server. The server was not found or was not accessible. Verify that the instance name is correct and that SQL Server is configured to allow remote connections. (provider: TCP Provider, error: 40 - Could not open a connection to SQL Server: Could not open a connection to SQL Server)";
        public const string ConnectionFailedWindowsMessage = "A network-related or instance-specific error occurred while establishing a connection to SQL Server. The server was not found or was not accessible. Verify that the instance name is correct and that SQL Server is configured to allow remote connections. (provider: Named Pipes Provider, error: 40 - Could not open a connection to SQL Server)";

        #region notes

        //received when ":" is used as a delimiter for port instead of "," (SqlException is Class 20, number 87)
        private const string InvalidConnectionStringPortDelimiterMessage = "A network-related or instance-specific error occurred while establishing a connection to SQL Server. The server was not found or was not accessible. Verify that the instance name is correct and that SQL Server is configured to allow remote connections. (provider: SNI_PN11, error: 25 - Connection string is not valid)";

        //wrong username/password -> SqlException Class 14, Number 18456
        //user is not enabled -> SqlException Class 14, Number 18470


        //sa = system admin, the account must be enabled and password must be changed to sa for this to work
        //10.0.2.2 works for Visual Studio Android emulator (allows acccess of the master machine localhost from emulator)
        //192.168.0.234 needs to be read from: cmd.exe -> ipconfig -> IPv4 address (but works without any internet connection as well, strange and even works after DNS cache flushing (even more strange))

        //https://www.youtube.com/watch?v=xNmIdFjXzl4 perfect tutorial to set up SQL Server Express with Android and Visual Studio

        //WARNING: port is delimited by "," in connection string to SQL Server (not ":")

        //works if 1433 port is enabled in SQL Express TCP/IP connections
        //works without local/private network
        //var connectionString = $"Server=192.168.0.234;Database=Ordinace;User Id=sa;Password=sa;";
        //var connectionString = $"Server=10.0.2.2;Database=Ordinace;User Id=sa;Password=sa;";

        //Windows - works without 1433 port allowed inbound rule in Firewall, without TCP/IP connection IPAll 1433 and without Named Pipes provider.
        //var connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=Ordinace;Integrated Security=True;";

        //Windows - works without 1433 port allowed inbound rule in Firewall, but TCP/IP connection on SqlServer must have IPAll 1433, works without named pipes provider
        //192.168.0.234:1433 doesn't work even when TCP/IP IPAll 1433 is configured
        //192.168.0.234\\SQLEXPRESS doesn't work even when TCP/IP IPAll 1433 is configured
        //var connectionString = "Data Source=192.168.0.234;Initial Catalog=Ordinace;User Id=sa;Password=sa;";

        //Windows cannot connect using the 10.0.2.2 address
        //plain "localhost" works only if TCP/IP IPAll is configured to 1433, otherwise only "localhost\\{InstanceName}" (that is, localhost\\SQLEXPRESS) works
        //var connectionString = "Data Source=localhost\\SQLEXPRESS;Initial Catalog=Ordinace;User Id=sa;Password=sa;";
        //Integrated Security=True; works over localhost even with TCP/IP 1433 (e.g.: this works: "Server=.;Database=Ordinace;Integrated Security=True;")

        //"Server=localhost,1433\\SQLEXPRESS;Database=Ordinace;User Id=sa;Password=sa;"; OK
        //"Server=.,1433\\SQLEXPRESS;Database=Ordinace;User Id=sa;Password=sa;"; OK

        //to allow connection from a device from local network, the inbound communication must be enabled in Windows Firewall

        #endregion

        //the SQL Server instance must have a database called Ordinace and there must be an enabled account called "sa" with password "sa"

        //the 192.168.0.234 address must be probably changed for every network because it is a private IP randomly assigned by the router

        //1433 inbound rule permitting TCP traffic must be enabled in firewall on the device where SQL Server is running
        //SQL Server must allow TCP connection
        //SQL Server must allow SQL authentication
        public const string LocalNetworkConnectionString = "Server=192.168.0.234;Database=Ordinace;User Id=sa;Password=sa;";
        
        //1433 firewall rule is not required
        //SQL Server must allow TCP connection
        //SQL Server must allow SQL authentication
        public const string AndroidEmulatorConnectionString = "Server=10.0.2.2;Database=Ordinace;User Id=sa;Password=sa;";
        
        //does not use SQL authentication - only possible on the same machine where SQL Server Express is hosted
        public const string WindowsConnectionString ="Server=.\\SQLEXPRESS;Database=Ordinace;Integrated Security=True;";

        public const string SQLQuery = "SELECT COUNT(*) FROM [Ordinace].[dbo].[USER]";
    }
}
