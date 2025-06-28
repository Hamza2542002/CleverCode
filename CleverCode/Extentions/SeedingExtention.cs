using CleverCode;
using CleverCode.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace medical_app_api.Extentions
{
    public static class SeedingExtention
    {
        public static async Task<IApplicationBuilder> SeedAsync(this IApplicationBuilder app, IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var services = scope.ServiceProvider;
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            var _dbContext = services.GetRequiredService<ApplicationDbContext>();
            var loggerFactory = services.GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger(typeof(Program));
            try
            {
                await _dbContext.Database.MigrateAsync();
                string[] roles = { "Admin", "User" };

                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                        await roleManager.CreateAsync(new IdentityRole(role));
                }
                await _dbContext.SaveChangesAsync();
            }
            catch(Exception ex) 
            {
                logger.LogError(message: ex.Message);
            }
            return app;
        }
    }
}
