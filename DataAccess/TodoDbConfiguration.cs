using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TodoApi.Model;

namespace TodoApi.DataAccess
{
    public class TodoDbConfiguration : IEntityTypeConfiguration<TodoTask>
    {
        public void Configure(EntityTypeBuilder<TodoTask> builder)
        {
            builder.ToTable("Tasks");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                .HasMaxLength(64)
                .IsRequired();

            builder.Property(p => p.Description)
                .HasMaxLength(256)
                .IsRequired();

            builder.Property(p => p.CreatedAt)
                .HasColumnType("datetimeoffset(3)")
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("sysdatetimeoffset()")
                .IsRequired();

            builder.Property(p => p.DueDate)
                .HasColumnType("datetimeoffset(3)")
                .IsRequired();

            builder.Property(p => p.UpdatedAt)
                .HasColumnType("datetimeoffset(3)")
                .IsRequired();

            builder.Property(p => p.Status)
                .HasConversion<string>()
                .HasMaxLength(32)
                .IsRequired();

            builder.Property(p => p.Priority)
                .HasConversion<string>()
                .HasMaxLength(32)
                .IsRequired();

        }
    }
}
