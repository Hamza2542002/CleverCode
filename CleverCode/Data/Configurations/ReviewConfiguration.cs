using CleverCode.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace CleverCode.Data.Configurations
{
    public class ReviewConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.HasKey(r => r.Review_ID);

            builder.Property(r => r.Comment)
                .HasMaxLength(500);

            builder.Property(r => r.Rate);

            builder.Property(r => r.Date)
                .HasColumnType("datetime");

            builder.Property(r => r.Name)
                .HasMaxLength(100);

            builder.Property(r => r.Company)
                .HasMaxLength(100);
            builder.HasOne(c => c.Service)
          .WithMany()
          .HasForeignKey(c => c.Service_ID)
          .OnDelete(DeleteBehavior.SetNull); 


        }
    }
}

