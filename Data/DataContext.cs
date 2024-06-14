using managementapp.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace managementapp.Data;

public class DataContext : DbContext
{
    public DataContext()
    {

    }

    public DbSet<Todolist>? Todolists { get; set; }
    public DbSet<Users>? Users { get; set; }
    public DbSet<Message>? Messages { get; set; }
    public DbSet<UserLogin>? UserLogins { get; set; }
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {

    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Todolist>().HasKey(todos => todos.Id);
        modelBuilder.Entity<Users>().HasKey(users => users.Id);
        modelBuilder.Entity<Message>().HasKey(messages => messages.Id);
        modelBuilder.Entity<UserLogin>().HasKey(userlogins => userlogins.Id);

        base.OnModelCreating(modelBuilder);
    }

}
