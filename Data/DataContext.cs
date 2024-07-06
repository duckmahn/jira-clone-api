using managementapp.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace managementapp.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {

    }
    public DbSet<Todolist> Todolists { get; set; }
    public DbSet<Users> Users { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<UserLogin> UserLogins { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<ProjectUser> ProjectUsers { get; set; }
    public DbSet<Kanban> Kanbans { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProjectUser>()
            .HasKey(pu => new { pu.ProjectId, pu.UserId });
        modelBuilder.Entity<ProjectUser>()
            .HasOne(pu => pu.Project)
            .WithMany(p => p.ProjectUsers)
            .HasForeignKey(pu => pu.ProjectId);
        modelBuilder.Entity<ProjectUser>()
            .HasOne(pu => pu.User)
            .WithMany(p => p.ProjectUsers)
            .HasForeignKey(pu => pu.UserId);

        modelBuilder.Entity<Kanban>()
            .HasKey(t => t.Id);
    }

}
