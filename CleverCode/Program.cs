using CleverCode.Data;
using CleverCode.Helpers;
using CleverCode.Interfaces;
using CleverCode.Middlewares;
using CleverCode.Services;
using Microsoft.EntityFrameworkCore;


namespace CleverCode
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();
            builder.Services.AddScoped<IComplaintService, ComplaintService>();

            builder.Services.AddScoped<ICompanyInformationService, CompanyInformationService>();
            builder.Services.AddScoped<IFAQService, FAQService>();
            builder.Services.AddScoped<IMessageService, MessageService>();

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddAutoMapper(m => m.AddProfile(new MappingProfiles()));
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();
            app.UseMiddleware<ExceptionMiddleware>();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
