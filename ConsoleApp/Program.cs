using DAL.Shared;

using System.Diagnostics;

using UIHelper;

var connectionString = "Server=LAPTOP-HGEN5Q27\\SQLEXPRESS;Database=Ordinace;Integrated Security=True;Encrypt=false;";

var DAOs = DAOsHelper.DAOs;

foreach (var dao in DAOs)
{
    Trace.WriteLine($"Executing {dao.Name}...");
    var result = DAOsHelper.ExecuteDAO(dao, connectionString);
    Trace.WriteLine($"{dao.Name}: {result}");
}