using Microsoft.EntityFrameworkCore;
using ValeraProject.Models;

namespace ValeraProject.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Valera> Valeras { get; set; }
    }
}