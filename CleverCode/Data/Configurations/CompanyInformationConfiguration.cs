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

            // Name (En & Ar)
            builder.Property(c => c.NameEn).IsRequired().HasMaxLength(100);
            builder.Property(c => c.NameAr).IsRequired().HasMaxLength(100);

            // Mission
            builder.Property(c => c.MissionEn).HasMaxLength(300);
            builder.Property(c => c.MissionAr).HasMaxLength(300);

            // Vision
            builder.Property(c => c.VisionEn).HasMaxLength(300);
            builder.Property(c => c.VisionAr).HasMaxLength(300);

            // Description
            builder.Property(c => c.DescriptionEn).HasMaxLength(500);
            builder.Property(c => c.DescriptionAr).HasMaxLength(500);

            // Story
            builder.Property(c => c.StoryEn).HasMaxLength(500);
            builder.Property(c => c.StoryAr).HasMaxLength(500);

            // Static fields
            builder.Property(c => c.Logo).HasMaxLength(200);
            builder.Property(c => c.SocialLink).HasMaxLength(200);
            builder.Property(c => c.ResponseTime).HasMaxLength(50);

            // Relations
            builder.HasMany(c => c.Values)
                   .WithOne(v => v.CompanyInformation)
                   .HasForeignKey(v => v.Company_ID);
        }
    }
}
