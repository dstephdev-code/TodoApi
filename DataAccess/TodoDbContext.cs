using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TodoApi.Model;

namespace TodoApi.DataAccess
{
    public class TodoDbContext(DbContextOptions<TodoDbContext> dbContextOptions) : DbContext(dbContextOptions)
    {
        public DbSet<User> Users => Set<User>();

        public DbSet<TodoTask> Tasks => Set<TodoTask>();

        public DbSet<TaskAssignment> TaskAssignments => Set<TaskAssignment>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            var user = modelBuilder.Entity<User>();
            user.HasKey(p => p.Id);
            user.Property(p => p.FirstName)
                .HasMaxLength(64)
                .IsRequired();
            user.Property(p => p.LastName)
                .HasMaxLength(64)
                .IsRequired();

            var taskAssignment = modelBuilder.Entity<TaskAssignment>();
            taskAssignment.HasKey(o => new { o.TaskId, o.UserId });
            taskAssignment.HasOne(p => p.Task)
                .WithMany(p => p.TaskAssignments)
                .HasForeignKey(fk => fk.TaskId);
            taskAssignment.HasOne(p => p.User)
                .WithMany(p => p.TaskAssignments)
                .HasForeignKey(fk => fk.UserId);
        }
    }
}
