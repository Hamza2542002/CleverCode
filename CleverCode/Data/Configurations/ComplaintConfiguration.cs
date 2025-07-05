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

            builder.Property(c => c.Type_AR)
                .HasMaxLength(100);
            builder.Property(c => c.Type_EN)
                .HasMaxLength(100);

            builder.Property(c => c.Description_AR)
                .HasMaxLength(500);
            builder.Property(c => c.Description_EN)
                .HasMaxLength(500);

            builder.Property(c => c.Status_AR)
                .HasMaxLength(50);
            builder.Property(c => c.Status_EN)
                .HasMaxLength(50);

            builder.Property(c => c.Priority_AR)
                .HasMaxLength(50);
            builder.Property(c => c.Priority_EN)
                .HasMaxLength(50);

            builder.Property(c => c.Name_AR)
                .HasMaxLength(100);
            builder.Property(c => c.Name_EN)
                .HasMaxLength(100);

            builder.Property(c => c.Date)
                .HasColumnType("datetime");

            builder.HasOne(c => c.Service)
                   .WithMany(s => s.Complaints)
                   .HasForeignKey(c => c.Service_ID)
                   .OnDelete(DeleteBehavior.SetNull);
            builder.Property(c => c.Email)
       .HasMaxLength(200);

        }
    }
}
