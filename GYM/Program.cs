using GYM.BLL.Interfaces;
using GYM.BLL.Mapping;
using GYM.BLL.Services;
using GYM.DAL.Data;
using GYM.DAL.Data.Contexts;
using GYM.DAL.Interfaces;
using GYM.DAL.Repositories;
using GymManagementDAL.Repositories.Classes;
using GymManagementDAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GYM
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            //Context Injection

            builder.Services.AddDbContext<GYMDbContext>(options=>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            //Repository and Unit of Work Injection

            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.AddScoped<IPlanRepository, PlanRepository>();
            builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();

            builder.Services.AddScoped<ISessionRepository, SessionRepository>();

            builder.Services.AddAutoMapper(x => x.AddProfile(new MappingProfiles()));


            var app = builder.Build();

            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<GYMDbContext>();
            var pendingMigrations = context.Database.GetPendingMigrations();
            if(pendingMigrations.Any())
                context.Database.Migrate();

            DataSeeding.SeedData(context);

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }


            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
