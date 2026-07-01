using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TodoApp.Api.Model.TaskAssignment;

namespace TodoApp.Api.DataAccess.Configurations
{
    public partial class TodoTaskConfiguration
    {
        public class TaskAssignmentConfiguration : IEntityTypeConfiguration<TaskAssignment>
        {
            public void Configure(EntityTypeBuilder<TaskAssignment> builder)
            {
                builder.ToTable("TaskAssignments");

                builder.HasKey(a => a.Id);

                builder.Property(a => a.Id)
                    .ValueGeneratedNever();

                builder.Property(a => a.TaskId)
                    .IsRequired();

                builder.Property(a => a.UserId)
                    .IsRequired();

                builder.Property(a => a.AssignedByUserId)
                    .IsRequired();

                builder.Property(a => a.AssignedAt)
                    .HasColumnType("datetimeoffset(3)")
                    .IsRequired();

                builder.Property(a => a.Comment)
                    .HasMaxLength(1024)
                    .IsRequired();

                builder.HasIndex(a => new 
                {
                    a.TaskId,
                    a.UserId
                }).IsUnique();

                builder.HasOne(a => a.Task)
                    .WithMany(t => t.TaskAssignments)
                    .HasForeignKey(a => a.TaskId)
                    .OnDelete(DeleteBehavior.Restrict);

                builder.HasOne(a => a.User)
                    .WithMany(u => u.TaskAssignments)
                    .HasForeignKey(a => a.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                builder.HasOne(a => a.AssignedByUser)
                    .WithMany()
                    .HasForeignKey(a => a.AssignedByUserId)
                    .OnDelete(DeleteBehavior.Restrict);
            }
        }
    }
}
