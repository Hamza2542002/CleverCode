using CleverCode.Data;
using Microsoft.EntityFrameworkCore;

namespace CleverCode.Extentions
{
    public static class InjectApplicationDbContext
    {
        public static IServiceCollection AddApplicationDbContext(this IServiceCollection services, 
            IConfiguration configuration,
            IHostEnvironment hostEnvironment)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                 options.UseSqlServer(configuration
                     .GetConnectionString(hostEnvironment.IsDevelopment() ? "DefaultConnection" : "ProductionConnection")));


            return services;
        }
    }
}
