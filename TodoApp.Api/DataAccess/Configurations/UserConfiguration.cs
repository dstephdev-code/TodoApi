using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TodoApi.Model;

namespace TodoApi.DataAccess.Configurations
{
    public partial class TodoTaskConfiguration
    {
        public class UserConfiguration : IEntityTypeConfiguration<User>
        {
            public void Configure(EntityTypeBuilder<User> builder)
            {
                builder.ToTable("Users");

                builder.HasKey(p => p.Id);

                builder.Property(p => p.FirstName)
                    .HasMaxLength(64)
                    .IsRequired();

                builder.Property(p => p.LastName)
                    .HasMaxLength(64)
                    .IsRequired();
            }
        }
    }
}
