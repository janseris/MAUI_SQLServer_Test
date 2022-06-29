using DAL.Shared;

using System.Diagnostics;

using UIHelper;

namespace MauiApp11;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        InitializeConnectionStringComboBox();
        InitializeConnectionStringTextEdit();

        useCustomConnectionStringCheckBox.CheckedChanged += UseCustomConnectionStringCheckBox_CheckedChanged;
    }

    private void UseCustomConnectionStringCheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        customConnectionStringTextEdit.IsEnabled = e.Value;
        connectionStringComboBox.IsEnabled = !e.Value;
    }

    private void InitializeConnectionStringComboBox()
    {
        container.Add(connectionStringComboBox);
        connectionStringComboBox.ItemsSource = new List<string> { 
            Constants.WindowsConnectionString,
            Constants.AndroidEmulatorConnectionString,
            Constants.LocalNetworkConnectionString
        };
        connectionStringComboBox.SelectedIndexChanged += ConnectionStringComboBox_SelectedIndexChanged;
    }

    private void InitializeConnectionStringTextEdit()
    {
        customConnectionStringTextEdit.IsEnabled = false;
        container.Add(customConnectionStringTextEdit);
    }

    private void ConnectionStringComboBox_SelectedIndexChanged(object sender, EventArgs e)
    {
        var comboBox = sender as Picker;
        SelectedConnectionString = comboBox.SelectedItem as string;
    }

    private string SelectedConnectionString { get; set; }
    private readonly Picker connectionStringComboBox = new Picker { Title = "Select a connection string" };
    private readonly Editor customConnectionStringTextEdit = new Editor { 
        Placeholder = "custom connection string",
        HeightRequest = 80
    };


    private void ExecuteDAOWithUIResult(DAOBase dao, string connectionString, StackLayout results)
    {
        var result = DAOsHelper.ExecuteDAO(dao, connectionString);
        var text = $"{dao.Name}: {result}";
        results.Add(new Label()
        {
            HorizontalOptions = LayoutOptions.Center,
            Text = text,
            Margin = new Thickness(0, 5)
        });
    }

    private void button_Clicked(object sender, EventArgs e)
    {
        results.Children.Clear();

        //use custom or from combobox if using custom is not checked
        var connectionString = useCustomConnectionStringCheckBox.IsChecked ? customConnectionStringTextEdit.Text : SelectedConnectionString;

        //for appended parts to work, the connection string must end with ";"
        if (encryptFalseCheckBox.IsChecked)
        {
            connectionString += "Encrypt=false;";
        }
        if (encryptTrueCheckBox.IsChecked)
        {
            connectionString += "Encrypt=true;";
        }
        if (trustServerCertificateCheckBox.IsChecked)
        {
            connectionString += "TrustServerCertificate=true;";
        }
        if (disableConnectionPoolingCheckBox.IsChecked)
        {
            connectionString += "Pooling=false;";
        }

        Trace.WriteLine($"Calling the database with connection string: {connectionString}");

        var DAOs = DAOsHelper.DAOs;

        foreach (var dao in DAOs)
        {
            Trace.WriteLine($"Executing {dao.Name}...");
            ExecuteDAOWithUIResult(dao, connectionString, results);
        }

        //these texts should be displayed below the button
        foreach (var label in results.Children)
        {
            Trace.WriteLine((label as Label).Text);
        }
    }
}

