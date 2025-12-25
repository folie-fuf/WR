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
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Уникальный email
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();
                
            // Уникальный username
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();
                
            // Связь User -> Valera (один ко многим)
            modelBuilder.Entity<User>()
                .HasMany(u => u.Valeras)
                .WithOne(v => v.User)
                .HasForeignKey(v => v.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}