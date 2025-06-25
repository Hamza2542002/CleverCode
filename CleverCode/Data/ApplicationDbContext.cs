using CleverCode.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace CleverCode.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Service> Services { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectService> ProjectServices { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Complaint> Complaints { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<CompanyInformation> CompanyInformations { get; set; }
        public DbSet<TeamMember> TeamMembers { get; set; }
        public DbSet<FAQ> FAQs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CompanyInformation>().OwnsOne(c => c.ContactInfo);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }


    }

}
