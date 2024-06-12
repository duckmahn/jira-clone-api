using managementapp.Data.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.Common;


namespace managementapp.Data;

public class DataContext : DbContext
{


    public virtual  DbSet<Todolist> Todolists { get; set; }
    public virtual DbSet<Users> Users { get; set; }
    public virtual DbSet<Message> Messages { get; set; }
    public virtual DbSet<UserLogin> UserLogins { get; set;}

    public DataContext(DbContextOptions<DataContext> options) : base(options) {

     
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Todolist>().HasKey(todos => todos.Id);
        modelBuilder.Entity<Todolist>().HasData(
                               new Todolist
                               {
                                   Id = 1,
                                   Title = "First Todo",
                                   Description = "This is the first todo",
                                   IsCompleted = false,
                                   CreatedAt = DateTime.Now,
                                   UpdatedAt = DateTime.Now,
                                   UserId = 1
                               });

        modelBuilder.Entity<Users>().HasKey(users => users.Id);
        modelBuilder.Entity<Users>().HasData(
                                          new Users
                                          {
                                              Id = 1,
                                              Name = "Manh"
                                           },
                                          new Users
                                          {
                                              Id = 2,
                                              Name = "Huy"
                                          });
        modelBuilder.Entity<Message>().HasKey(messages => messages.Id);

        base.OnModelCreating(modelBuilder);

    }
}