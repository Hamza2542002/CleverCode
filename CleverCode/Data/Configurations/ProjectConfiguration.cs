using CleverCode.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace CleverCode.Data.Configurations
{
    public class ProjectConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.HasKey(p => p.Project_ID);

            builder.Property(p => p.TitleAr)
               
                .HasMaxLength(100);

            builder.Property(p => p.TitleEn)
              
               .HasMaxLength(100);
            builder.Property(p => p.Rate);

            builder.Property(p => p.DescriptionAr)
                .HasMaxLength(500);
            builder.Property(p => p.DescriptionEn)
              .HasMaxLength(500);
            builder.Property(p => p.Tech)
                .HasMaxLength(200);
        }
    }
}