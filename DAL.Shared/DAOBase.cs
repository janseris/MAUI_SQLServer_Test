namespace DAL.Shared
{
    public abstract class DAOBase
    {
        public string Name { get; set; }
        public abstract int GetDBCallResult(string connectionString);
    }
}
