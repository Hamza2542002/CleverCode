using CleverCode.Data;
using CleverCode.Extentions;
using CleverCode.Helpers;
using CleverCode.Interfaces;
using CleverCode.Middlewares;
using CleverCode.Models;
using CleverCode.Services;
using medical_app_api.Extentions;
using medical_app_db.Core.Helpers;
using medical_app_db.Core.Interfaces;
using medical_app_db.EF.Services;
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
            builder.Services.Configure<CloudinatuSettings>(builder.Configuration.GetSection("CloudinarySettings"));

            builder.Services.AddCors(options =>
            {
                // Policy for frontend apps that send credentials
                options.AddPolicy("AllowFrontend", policy =>
                {
                    policy.WithOrigins("https://clever-code-co.vercel.app", "http://localhost:5173")
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                });

                // Policy for public APIs (no credentials)
                options.AddPolicy("AllowAny", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });
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

            builder.Services.AddApplicationDbContext(builder.Configuration, builder.Environment);

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
            builder.Services.AddScoped<IProjectService, Services.ProjectServices>();
            builder.Services.AddScoped<IImageService, CloudinaryService>();

            // DB context

            builder.Services.AddAutoMapper(m => m.AddProfile(new MappingProfiles()));
            builder.Services.AddOpenApi();

            var app = builder.Build();

            await app.SeedAsync(app.Services);

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }
            app.UseRouting();
            app.UseCors("AllowFrontend");
            app.UseHttpsRedirection();
            app.UseMiddleware<ExceptionMiddleware>();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
