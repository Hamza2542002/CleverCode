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

            builder.Property(m => m.MessageText)
                .HasMaxLength(500);

            builder.Property(m => m.Name)
                .HasMaxLength(100);

            builder.Property(m => m.Date)
                .HasColumnType("datetime");

            builder.Property(m => m.Company)
                .HasMaxLength(100);

            builder.Property(m => m.Email)
                .HasMaxLength(150);
            builder.HasOne(c => c.Service)
       .WithMany(s => s.Messages) // لازم تكون موجودة في Service
       .HasForeignKey(c => c.Service_ID)
       .OnDelete(DeleteBehavior.SetNull);

        }
    }
}
