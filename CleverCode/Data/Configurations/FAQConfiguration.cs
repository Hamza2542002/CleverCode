using CleverCode.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleverCode.Data.Configurations
{
    public class FAQConfiguration : IEntityTypeConfiguration<FAQ>
    {
        public void Configure(EntityTypeBuilder<FAQ> builder)
        {
            builder.HasKey(f => f.FAQ_ID);

            builder.Property(f => f.QuestionsEn).HasMaxLength(300);
            builder.Property(f => f.QuestionsAr).HasMaxLength(300);

            builder.Property(f => f.AnswerEn).HasMaxLength(500);
            builder.Property(f => f.AnswerAr).HasMaxLength(500);
        }
    }
}
