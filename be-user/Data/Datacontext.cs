using Microsoft.EntityFrameworkCore;
using project.Models;

namespace project.Data
{
    public class Datacontext : DbContext
    {
        public Datacontext(DbContextOptions<Datacontext> opts) : base(opts)
        {

        }
        public DbSet<User> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(user => user.Id);
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Email = "Admin@gmail.com",
                    Username = "admin",
                    Password = "admin",

                },
                new User
                {
                    Id = 2,
                    Email = "user@gmail.com",
                    Username = "user",
                    Password = "user",
                });
            base.OnModelCreating(modelBuilder);
        }
    }
}
