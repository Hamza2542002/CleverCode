using CleverCode.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace CleverCode.Data.Configurations
{
    public class ServiceConfiguration : IEntityTypeConfiguration<Service>
        {
            public void Configure(EntityTypeBuilder<Service> builder)
            {
                builder.HasKey(s => s.Service_ID);

                builder.Property(s => s.Title)
                    .IsRequired()
                    .HasMaxLength(100);

                builder.Property(s => s.Icon)
                    .HasMaxLength(200);

                builder.Property(s => s.Description)
                    .HasMaxLength(500);

                builder.Property(s => s.Pricing)
                    .HasColumnType("decimal(18,2)");

                builder.Property(s => s.Feature)
                    .HasMaxLength(200);

                builder.Property(s => s.Category)
                    .HasMaxLength(50);

                builder.Property(s => s.TimeLine)
                    .HasMaxLength(50);
            }
        }
}

