using CleverCode.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace CleverCode.Data.Configurations
{
    public class TeamMemberConfiguration : IEntityTypeConfiguration<TeamMember>
    {
        public void Configure(EntityTypeBuilder<TeamMember> builder)
        {
            builder.HasKey(t => t.TeamMember_ID);

            builder.Property(t => t.Name)
                .HasMaxLength(100);

            builder.Property(t => t.Role)
                .HasMaxLength(100);

            builder.Property(t => t.Description)
                .HasMaxLength(300);

            builder.Property(t => t.LinkedInUrl)
                .HasMaxLength(200);

            builder.Property(t => t.PhotoUrl)
                .HasMaxLength(200);

            builder.Property(t => t.BIO)
                .HasMaxLength(500);
        }
    }
}
