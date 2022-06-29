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

        AddDAOCheckBoxes();

        selectAllDAOsButton.Clicked += (s, e) => SelectDAOs(DAOs);
        selectNoDAOsButton.Clicked += (s, e) => SelectDAOs(new List<DAOBase>());
    }

    private readonly List<DAOBase> DAOs = DAOsHelper.DAOs;
    private readonly Dictionary<DAOBase, CheckBox> DAOCheckBoxes = new Dictionary<DAOBase, CheckBox>();
    private List<DAOBase> SelectedDAOs { get => GetSelectedDAOs(); set => SelectDAOs(value); }

    private void AddDAOCheckBoxes()
    {
        foreach(var DAO in DAOs)
        {
            var label = new Label() { Text = DAO.Name, VerticalOptions = LayoutOptions.Center };
            var checkBox = new CheckBox() { IsChecked = true, VerticalOptions = LayoutOptions.Center };
            var namedCheckBoxContainer = new HorizontalStackLayout();
            namedCheckBoxContainer.Children.Add(checkBox);
            namedCheckBoxContainer.Children.Add(label);
            SqlClientCheckBoxesContainer.Add(namedCheckBoxContainer);
            DAOCheckBoxes.Add(DAO, checkBox);
        }
    }

    private List<DAOBase> GetSelectedDAOs()
    {
        var selected = new List<DAOBase>();
        foreach(var DAO in DAOCheckBoxes.Keys)
        {
            if (DAOCheckBoxes[DAO].IsChecked)
            {
                selected.Add(DAO);
            }
        }
        return selected;
    }

    private void SelectDAOs(List<DAOBase> items)
    {
        foreach (var DAO in this.DAOs)
        {
            var checkbox = DAOCheckBoxes[DAO];
            checkbox.IsChecked = false;
        }

        foreach (var DAO in items)
        {
            var checkbox = DAOCheckBoxes[DAO];
            checkbox.IsChecked = true;
        }
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
        Placeholder = "enter a custom connection string",
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

        foreach (var dao in this.SelectedDAOs)
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

