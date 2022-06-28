using DAL.Shared;

using System.Diagnostics;

using UIHelper;

namespace MauiApp11;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    private void ExecuteDAOWithUIResult(UserDAOBase dao, string connectionString, StackLayout results)
    {
        var result = DAOsHelper.ExecuteDAO(dao, connectionString);
        var text = $"{dao.Name}: {result}";
        results.Add(new Label()
        {
            HorizontalOptions = LayoutOptions.Center,
            Text = text,
            Margin = new Thickness(0, 10)
        });
    }

    private void button_Clicked(object sender, EventArgs e)
    {
        results.Children.Clear();

        var progressLabel = new Label()
        {
            HorizontalOptions = LayoutOptions.Center,
            Margin = new Thickness(0, 10)
        };

        results.Children.Add(progressLabel);

        var connectionString = Constants.WindowsConnectionString;
        //skip certificate validation - why is this or Encrypt=false needed in MAUI (even for integrated security = Windows authentication)
        //but not needed in WinForms or Blazor (non-MAUI) at all?
        //this does not help on the Android issue
        //connectionString += "TrustServerCertificate=true;"; 
        
        //with this, Windows authentication and also SQL authentication on Windows
        //does not require a trusted certificate for the (local) SQL Server (Express)
        //on Android, this does not solve the pre-login handshake problem
        connectionString += "Encrypt=false;";

        var DAOs = DAOsHelper.DAOs;

        foreach (var dao in DAOs)
        {
            Trace.WriteLine($"Executing {dao.Name}...");
            ExecuteDAOWithUIResult(dao, connectionString, results);
        }
        
        //these texts should be displayed below the button
        foreach(var label in results.Children)
        {
            Trace.WriteLine((label as Label).Text);
        }
    }
}

