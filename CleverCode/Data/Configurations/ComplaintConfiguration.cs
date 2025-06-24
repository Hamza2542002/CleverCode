using CleverCode.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace CleverCode.Data.Configurations
{
    public class ComplaintConfiguration : IEntityTypeConfiguration<Complaint>
    {
        public void Configure(EntityTypeBuilder<Complaint> builder)
        {
            builder.HasKey(c => c.Complaint_ID);

            builder.Property(c => c.Type)
                .HasMaxLength(100);

            builder.Property(c => c.Description)
                .HasMaxLength(500);

            builder.Property(c => c.Status)
                .HasMaxLength(50);

            builder.Property(c => c.Priority)
                .HasMaxLength(50);

            builder.Property(c => c.Name)
                .HasMaxLength(100);

            builder.Property(c => c.Date)
                .HasColumnType("datetime");
            builder.HasOne(c => c.Service)
       .WithMany()
       .HasForeignKey(c => c.Service_ID)
       .OnDelete(DeleteBehavior.SetNull); 

        }
    }
}
