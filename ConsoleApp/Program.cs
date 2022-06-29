using DAL.Shared;

using System.Diagnostics;

using UIHelper;

//LocalNetworkConnectionString and WindowsConnectionString works fine (that is OK)

var connectionString = Constants.WindowsConnectionString;
//skip certificate validation
//this or Encrypt=false is required to connect to SQL Server Express on Windows (but didn't used to be required!)
//on both Windows authentication and SQL authentication
//connectionString += "TrustServerCertificate=true;"; 

//connectionString += "Encrypt=false;";

//29 June 2022 none of these properties are mysteriously required for Windows with LocalNetworkConnectionString
//maybe because the first one di dnot fail and they are cached/reused from pool?

var DAOs = DAOsHelper.DAOs;

foreach (var dao in DAOs)
{
    Trace.WriteLine($"Executing {dao.Name}...");
    var result = DAOsHelper.ExecuteDAO(dao, connectionString);
    Trace.WriteLine($"{dao.Name}: {result}");
}