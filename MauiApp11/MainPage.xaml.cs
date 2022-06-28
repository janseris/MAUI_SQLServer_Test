using DAL.Shared;

namespace MauiApp11;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();

        DAOs = new List<UserDAOBase>
        {
            EFCore6DAO, EFCore7PreviewDAO,
            microsoftSqlClientOldVersionDAO, microsoftSqlClientCurrentVersionDAO, microsoftSqlClientPreviewVersionDAO,
            systemSqlClientVersionDAO
        };
    }

    private readonly UserDAOBase EFCore6DAO = new DAL.EFCore.UserDAO() { Name = "Microsoft.Data.SqlClient 2.1.4 (EF Core 6.0.6)" };
    private readonly UserDAOBase EFCore7PreviewDAO = new DAL.EFCore.Preview.UserDAO() { Name = "Microsoft.Data.SqlClient 5.0.0-preview2.22096.2 (EF Core 7.0.0-preview.5.22302.2)" };

    private readonly UserDAOBase microsoftSqlClientOldVersionDAO = new DAL.SqlClient.Microsoft.OldVersion.UserDAO() { Name = "Microsoft.Data.SqlClient 4.1.0" };
    private readonly UserDAOBase microsoftSqlClientCurrentVersionDAO = new DAL.SqlClient.Microsoft.UserDAO() { Name = "Microsoft.Data.SqlClient 2.0.0" };
    private readonly UserDAOBase microsoftSqlClientPreviewVersionDAO = new DAL.SqlClient.Microsoft.Preview.UserDAO() { Name = "Microsoft.Data.SqlClient 5.0.0-preview3.22168.1" };

    private readonly UserDAOBase systemSqlClientVersionDAO = new DAL.SqlClient.Microsoft.Preview.UserDAO() { Name = "System.Data.SqlClient 4.8.3" };

    public List<UserDAOBase> DAOs { get; }

    private string ExecuteDAO(UserDAOBase dao, string connectionString)
    {
        try
        {
            dao.GetUsersCount(connectionString);
            return "success";
        }
        catch (Exception ex)
        {
            if (ex.Message == Constants.AndroidSqlClientBugMessage)
            {
                return "Android connection bug";
            }
            if (ex.Message == Constants.ConnectionFailedAndroidMessage || ex.Message == Constants.ConnectionFailedWindowsMessage)
            {
                return "Connection failed, check internet and connection string and server configuration (accessibility from outside world)";
            }
            return GetExceptionMessage(ex);
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

    private void button_Clicked(object sender, EventArgs e)
    {
        results.Children.Clear();

        var connectionString = Constants.LocalNetworkConnectionString;
        connectionString += "TrustServerCertificate=true"; //skip certificate validation

        foreach (var dao in DAOs)
        {
            var result = ExecuteDAO(dao, connectionString);
            var text = $"{dao.Name}: {result}";
            results.Add(new Label()
            {
                HorizontalOptions = LayoutOptions.Center,
                Text = text,
                Margin = new Thickness(0, 10)
            });
        }
    }
}

