using DAL.Shared;

namespace MauiApp11;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        usersCountLabel.Text = "Click the button to load users 7 different ways";
    }

    
    private void OnCounterClicked(object sender, EventArgs e)
    {
        IUserDAO EFCore6DAO = new DAL.EFCore.UserDAO(); //Microsoft.Data.SqlClient 2.1.4
        IUserDAO EFCore7PreviewDAO = new DAL.EFCore.Preview.UserDAO(); // Microsoft.Data.SqlClient 5.0.0-preview2.22096.2

        IUserDAO microsoftSqlClientOldVersionDAO = new DAL.SqlClient.Microsoft.OldVersion.UserDAO();
        IUserDAO microsoftSqlClientCurrentVersionDAO = new DAL.SqlClient.Microsoft.UserDAO();
        IUserDAO microsoftSqlClientPreviewVersionDAO = new DAL.SqlClient.Microsoft.Preview.UserDAO();
        
        IUserDAO systemSqlClientVersionDAO = new DAL.SqlClient.Microsoft.Preview.UserDAO(); 
    
        
    }
}

