using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TodoApp.Api.Model;

namespace TodoApp.Api.DataAccess
{
    public class TodoDbContext(DbContextOptions<TodoDbContext> dbContextOptions) : DbContext(dbContextOptions)
    {
        public DbSet<User> Users => Set<User>();

        public DbSet<TodoTask> Tasks => Set<TodoTask>();

        public DbSet<TaskAssignment> TaskAssignments => Set<TaskAssignment>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
