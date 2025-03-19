using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WebApplication1.Context;
using WebApplication1.Middleware;
using WebApplication1.Models;
using WebApplication1.Repository;

namespace WebApplication1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddSession();
            builder.Services.AddLogging();
            builder.Services.AddScoped<ICoursesRepository,CoursesRepository>();
            builder.Services.AddScoped<IStudentRepository, StudentRepository>();
            builder.Services.AddScoped<IInstructorRepository, InstructorRepository>();
            builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            builder.Services.AddDbContext<SchoolContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("school"));
            });
            builder.Services.AddIdentity <ApplicationUser,IdentityRole>()
                .AddEntityFrameworkStores<SchoolContext>();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            //app.UseMiddleware<GlobalExceptionHandlingMiddleware>();
            app.UseSession();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
