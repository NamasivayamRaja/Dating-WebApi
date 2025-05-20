using Microsoft.EntityFrameworkCore;

namespace API.Models
{
    public class DataContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<AppUser> Users { get; set; }
    }
}
