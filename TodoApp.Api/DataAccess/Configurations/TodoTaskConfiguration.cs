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

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                .HasMaxLength(64)
                .IsRequired()
                .UseCollation("SQL_Latin1_General_CP1_CI_AS"); // Set as default in MSSQL, but not default in every DB, so best to keep it as explicit configuration rule.

            builder.Property(p => p.Description)
                .HasMaxLength(256)
                .IsRequired()
                .UseCollation("SQL_Latin1_General_CP1_CI_AS"); // Also, in modern time better to use Latin1_General_100_CI_AS, but not in this projects. Way to migrate is to copy-delete existing row, which is tiring.

            builder.Property(p => p.CreatedAt)
                .HasColumnType("datetimeoffset(3)")
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("sysdatetimeoffset()")
                .IsRequired();

            builder.Property(p => p.DueDate)
                .HasColumnType("datetimeoffset(3)")
                .IsRequired();

            builder.Property(p => p.UpdatedAt)
                .HasColumnType("datetimeoffset(3)");

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
