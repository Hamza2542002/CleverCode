using CleverCode.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleverCode.Data.Configurations
{
    public class CompanyInformationConfiguration : IEntityTypeConfiguration<CompanyInformation>
    {
        public void Configure(EntityTypeBuilder<CompanyInformation> builder)
        {
            builder.HasKey(c => c.Company_ID);

            builder.Property(c => c.Name).IsRequired().HasMaxLength(100);
            builder.Property(c => c.Mission).HasMaxLength(300);
            builder.Property(c => c.Vision).HasMaxLength(300);
            builder.Property(c => c.Description).HasMaxLength(500);
            builder.Property(c => c.Logo).HasMaxLength(200);
            builder.Property(c => c.SocialLink).HasMaxLength(200);
            builder.Property(c => c.Story).HasMaxLength(500);
            builder.Property(c => c.ResponseTime).HasMaxLength(50);

            builder.HasMany(c => c.Values)
                   .WithOne(v => v.CompanyInformation)
                   .HasForeignKey(v => v.Company_ID);

        }
    }
}
