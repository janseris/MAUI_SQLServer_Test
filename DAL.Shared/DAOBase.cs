namespace DAL.Shared
{
    public abstract class DAOBase
    {
        public string Name { get; set; }
        
        /// <summary>
        /// Result and connection ID as string
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public abstract (int, string) GetDBCallResult(string connectionString);
    }
}
