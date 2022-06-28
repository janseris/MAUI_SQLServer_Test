namespace DAL.Shared
{
    public interface IUsersDAO
    {
        int GetUsersCount(string connectionString);
        int GetUsersCount();
    }
}
