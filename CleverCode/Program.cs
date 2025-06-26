using CleverCode.Data;
using CleverCode.Helpers;
using CleverCode.Interfaces;
using CleverCode.Middlewares;
using CleverCode.Models;
using CleverCode.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace CleverCode
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services
            builder.Services.AddControllers();
            builder.Services.Configure<JWT>(builder.Configuration.GetSection("JWT"));

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o =>
            {
                o.RequireHttpsMetadata = false;
                o.SaveToken = false;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = builder.Configuration["JWT:IssuerIP"],
                    ValidAudience = builder.Configuration["JWT:AudienceIP"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(builder.Configuration["JWT:SecurityKey"]!)
                    ),
                };
            });

            // Identity + Roles
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddRoles<IdentityRole>() // ✅ Add this
                .AddEntityFrameworkStores<ApplicationDbContext>();

            // Custom services
            builder.Services.AddScoped<IComplaintService, ComplaintService>();
            builder.Services.AddScoped<ICompanyInformationService, CompanyInformationService>();
            builder.Services.AddScoped<IFAQService, FAQService>();
            builder.Services.AddScoped<IMessageService, MessageService>();
            builder.Services.AddScoped<IAuthServices, AuthServices>();
            builder.Services.AddScoped<ITeamMemberService, TeamMemberService>();
            builder.Services.AddScoped<IServicesService, ServicesService>();
            builder.Services.AddScoped<IReviewService, ReviewService>();
            builder.Services.AddScoped<IProjectService, Services.ProjectService>();

            // DB context
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddAutoMapper(m => m.AddProfile(new MappingProfiles()));
            builder.Services.AddOpenApi();

            var app = builder.Build();

            // 🔄 Create Roles if not exist
            using (var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                string[] roles = { "Admin", "User" };

                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                        await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }
            app.UseRouting();
            app.UseHttpsRedirection();
            app.UseMiddleware<ExceptionMiddleware>();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
