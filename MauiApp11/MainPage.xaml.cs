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

    private void ExecuteDAOWithUIResult(DAOBase dao, string connectionString, StackLayout results)
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

        //var connectionString = Constants.LocalNetworkConnectionString;
        var connectionString = "Server=LAPTOP-HGEN5Q27\\SQLEXPRESS;Database=Ordinace;Integrated Security=True;";

        //connectionString += "TrustServerCertificate=true;"; 
        //connectionString += "Encrypt=false;";

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

