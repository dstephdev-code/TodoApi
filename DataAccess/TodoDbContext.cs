using Microsoft.EntityFrameworkCore;
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
            modelBuilder.Entity<TaskAssignment>().HasKey(x => new { x.TaskId, x.UserId });
        }
    }
}
