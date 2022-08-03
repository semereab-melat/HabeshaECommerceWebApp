using DukanHabeshaModels;
using Microsoft.EntityFrameworkCore;

namespace DukanHabeshaDataAccess.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Origin> Origin { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}
