using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TodoApi.Model;

namespace TodoApi.DataAccess.Configurations
{
    public partial class TodoTaskConfiguration
    {
        public class TaskAssignmentConfiguration : IEntityTypeConfiguration<TaskAssignment>
        {
            public void Configure(EntityTypeBuilder<TaskAssignment> builder)
            {
                builder.ToTable("TaskAssignments");

                builder.HasKey(o => new { o.TaskId, o.UserId });

                builder.HasOne(p => p.Task)
                    .WithMany(p => p.TaskAssignments)
                    .HasForeignKey(fk => fk.TaskId);

                builder.HasOne(p => p.User)
                    .WithMany(p => p.TaskAssignments)
                    .HasForeignKey(fk => fk.UserId);
            }
        }
    }
}
