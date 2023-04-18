using WebAppMvc.Models;
using Microsoft.EntityFrameworkCore;

namespace WebAppMvc.Data
{
    public class ApiDbContext : DbContext
    {
        public ApiDbContext(DbContextOptions options) : base(options)
        {
        }
        // Entities        
        public DbSet<User> Users { get; set; }
        //public DbSet<Page> Pages { get; set; }
    }
}
