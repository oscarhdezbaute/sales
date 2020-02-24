namespace sales.Domain.Models
{
    using System.Data.Entity;
    public class DataContext : DbContext
    {
        public DataContext() : base("DefaultConnection")
        {
        }

        public System.Data.Entity.DbSet<sales.Common.Models.Product> Products { get; set; }
        public System.Data.Entity.DbSet<sales.Common.Models.Category> Categories { get; set; }
    }
}
