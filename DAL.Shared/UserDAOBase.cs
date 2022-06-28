namespace DAL.Shared
{
    public abstract class UserDAOBase
    {
        public string Name { get; set; }
        public abstract int GetUsersCount(string connectionString);
    }
}
