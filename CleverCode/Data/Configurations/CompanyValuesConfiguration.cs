using CleverCode.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleverCode.Data.Configurations
{
    public class CompanyValuesConfiguration : IEntityTypeConfiguration<CompanyValues>
    {
        public void Configure(EntityTypeBuilder<CompanyValues> builder)
        {
            builder.HasKey(cv => cv.Id);
        }
    }
}
