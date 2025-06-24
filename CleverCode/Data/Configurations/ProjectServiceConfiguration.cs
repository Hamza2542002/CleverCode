using CleverCode.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace CleverCode.Data.Configurations
{
    public class ProjectServiceConfiguration : IEntityTypeConfiguration<ProjectService>
    {
        public void Configure(EntityTypeBuilder<ProjectService> builder)
        {
            builder.HasKey(ps => new { ps.Project_ID, ps.Service_ID });

            builder.HasOne(ps => ps.Project)
                .WithMany(p => p.ProjectServices)
                .HasForeignKey(ps => ps.Project_ID);

            builder.HasOne(ps => ps.Service)
                .WithMany(s => s.ProjectServices)
                .HasForeignKey(ps => ps.Service_ID);
        }
    }
}
