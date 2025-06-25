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

            // 👇 هنا بنقول إن ContactInfo و Values عبارة عن Owned Types
            builder.OwnsOne(c => c.ContactInfo, ci =>
            {
                ci.Property(p => p.Email).HasMaxLength(100);
                ci.Property(p => p.Phone).HasMaxLength(50);
                ci.Property(p => p.Address).HasMaxLength(200);
            });

            builder.OwnsOne(c => c.Values, v =>
            {
                v.Property(p => p.Name).HasMaxLength(100);
                v.Property(p => p.Description).HasMaxLength(300);
            });
        }
    }
}
