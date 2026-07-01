using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TodoApp.Api.Model;

namespace TodoApp.Api.DataAccess.Configurations
{
    public partial class TodoTaskConfiguration
    {
        public class UserConfiguration : IEntityTypeConfiguration<User>
        {
            public void Configure(EntityTypeBuilder<User> builder)
            {
                builder.ToTable("Users");

                builder.HasKey(u => u.Id);

                builder.Property(u => u.Id)
                    .ValueGeneratedNever();

                builder.Property(u => u.FirstName)
                    .HasMaxLength(64)
                    .IsRequired();

                builder.Property(u => u.LastName)
                    .HasMaxLength(64)
                    .IsRequired();

                builder.Property(u => u.Email)
                    .HasMaxLength(128)
                    .IsRequired();

                builder.Property(u => u.Position)
                    .HasMaxLength(128)
                    .IsRequired();

                builder.Property(u => u.CreatedAt)
                    .HasColumnType("datetimeoffset(3)")
                    .IsRequired();

                builder.Property(u => u.IsActive)
                    .IsRequired();

                builder.Ignore(u => u.FullName);

                builder.HasIndex(u => u.Email)
                    .IsUnique();
            }
        }
    }
}
