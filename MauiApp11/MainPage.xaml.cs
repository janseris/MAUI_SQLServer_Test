using MauiApp11.Models;

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

using System.Diagnostics;

namespace MauiApp11;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        usersCountLabel.Text = "Click the button to load users";
    }

    private const string AndroidSqlClientBugMessage = "A connection was successfully established with the server, but then an error occurred during the pre-login handshake. (provider: TCP Provider, error: 35 - An internal exception was caught)";
    private const string ConnectionFailedAndroidMessage = "A network-related or instance-specific error occurred while establishing a connection to SQL Server. The server was not found or was not accessible. Verify that the instance name is correct and that SQL Server is configured to allow remote connections. (provider: TCP Provider, error: 40 - Could not open a connection to SQL Server: Could not open a connection to SQL Server)";
    private const string ConnectionFailedWindowsMessage = "A network-related or instance-specific error occurred while establishing a connection to SQL Server. The server was not found or was not accessible. Verify that the instance name is correct and that SQL Server is configured to allow remote connections. (provider: Named Pipes Provider, error: 40 - Could not open a connection to SQL Server)";

    //received when ":" is used as a delimiter for port instead of "," (SqlException is Class 20, number 87)
    private const string InvalidConnectionStringPortDelimiterMessage = "A network-related or instance-specific error occurred while establishing a connection to SQL Server. The server was not found or was not accessible. Verify that the instance name is correct and that SQL Server is configured to allow remote connections. (provider: SNI_PN11, error: 25 - Connection string is not valid)";

    //wrong username/password -> SqlException Class 14, Number 18456
    //user is not enabled -> SqlException Class 14, Number 18470

    private void OnCounterClicked(object sender, EventArgs e)
    {
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

        var connectionString = "Server=192.168.0.234;Database=Ordinace;User Id=sa;Password=sa;";

        DbContextOptionsBuilder<OrdinaceContext> builder = new DbContextOptionsBuilder<OrdinaceContext>();
        builder.UseSqlServer(connectionString);
        var parameters = builder.Options;
        try
        {

            using (var db = new OrdinaceContext(parameters))
            {
                var users = (from user in db.USER select user).ToList();
                usersCountLabel.Text = $"Loaded {users.Count} users";
            }
        }
        catch (SqlException ex)
        {
            if(ex.Message == AndroidSqlClientBugMessage)
            {
                Trace.WriteLine("The connection string is OK but Android SqlClient bug occurred.");
            }
            usersCountLabel.Text = $"{ex.GetType()} {ex.Message}";
        }
        catch (Exception ex)
        {
            usersCountLabel.Text = $"{ex.GetType()} {ex.Message}";
        }
    }
}

