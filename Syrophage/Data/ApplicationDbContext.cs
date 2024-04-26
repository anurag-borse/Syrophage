using Microsoft.EntityFrameworkCore;
using Syrophage.Models;

namespace Syrophage.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {
        }
        public DbSet<Users> Users { get; set; }

    }
}
