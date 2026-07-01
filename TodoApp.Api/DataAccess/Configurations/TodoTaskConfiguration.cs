using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TodoApp.Api.Model;

namespace TodoApp.Api.DataAccess.Configurations
{
    public partial class TodoTaskConfiguration : IEntityTypeConfiguration<TodoTask>
    {
        public void Configure(EntityTypeBuilder<TodoTask> builder)
        {
            builder.ToTable("Tasks");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Id)
                .ValueGeneratedNever();

            builder.Property(t => t.Name)
                .HasMaxLength(64)
                .IsRequired()
                .UseCollation("SQL_Latin1_General_CP1_CI_AS"); // Set as default in MSSQL, but not default in every DB, so best to keep it as explicit configuration rule.

            builder.Property(t => t.Description)
                .HasMaxLength(1024)
                .IsRequired()
                .UseCollation("SQL_Latin1_General_CP1_CI_AS"); // Also, in modern time better to use Latin1_General_100_CI_AS, but not in this projects. Way to migrate is to copy-delete existing row, which is tiring.

            builder.Property(t => t.CreatedAt)
                .HasColumnType("datetimeoffset(3)")
                .IsRequired();

            builder.Property(t => t.DueDate)
                .HasColumnType("datetimeoffset(3)")
                .IsRequired();

            builder.Property(t => t.UpdatedAt)
                .HasColumnType("datetimeoffset(3)");

            builder.Property(t => t.Status)
                .HasConversion<string>()
                .HasMaxLength(32)
                .IsRequired();

            builder.Property(t => t.Priority)
                .HasConversion<string>()
                .HasMaxLength(32)
                .IsRequired();

            builder.HasIndex(t => t.Status);

            builder.HasIndex(t => t.DueDate);
        }
    }
}
