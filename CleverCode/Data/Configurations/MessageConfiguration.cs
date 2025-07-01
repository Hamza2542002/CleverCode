using CleverCode.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleverCode.Data.Configurations
{
    public class MessageConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.HasKey(m => m.Message_ID);

            builder.Property(m => m.MessageTextAr)
                .HasMaxLength(500)
                .IsRequired(false);

            builder.Property(m => m.MessageTextEn)
                .HasMaxLength(500)
                .IsRequired(false);

            builder.Property(m => m.Name)
                .HasMaxLength(100)
                .IsRequired(false);

            builder.Property(m => m.Date)
                .HasColumnType("datetime")
                .IsRequired();

            builder.Property(m => m.Company)
                .HasMaxLength(100)
                .IsRequired(false);

            builder.Property(m => m.Email)
                .HasMaxLength(150)
                .IsRequired(false);

            builder.HasOne(m => m.Service)
                .WithMany(s => s.Messages)
                .HasForeignKey(m => m.Service_ID)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
