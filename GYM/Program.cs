using GYM.BLL.AttachementService;
using GYM.BLL.Interfaces;
using GYM.BLL.Mapping;
using GYM.BLL.Services;
using GYM.BLL.Services.Interfaces;
using GYM.DAL.Data;
using GYM.DAL.Data.Contexts;
using GYM.DAL.Entities;
using GYM.DAL.Interfaces;
using GYM.DAL.Repositories;
using GymManagementBLL.Services.Classes;
using GymManagementDAL.Repositories.Classes;
using GymManagementDAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
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
            builder.Services.AddScoped<IMemberService, MemberService>();
            builder.Services.AddScoped<ITrainerService, TrainerService>();
            builder.Services.AddScoped<IPlanService, PlanService>();
            builder.Services.AddScoped<ISessionService, SessionService>();
            builder.Services.AddScoped<IAttachementService, AttachementService>();
            builder.Services.AddScoped<IAccountService, AccountService>();

            builder.Services.AddAutoMapper(x => x.AddProfile(new MappingProfiles()));

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(conf =>
            {
                conf.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<GYMDbContext>();

            builder.Services.ConfigureApplicationCookie(opt =>
            {
                opt.AccessDeniedPath = "/Account/AccessDenied";
            });

            var app = builder.Build();

            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<GYMDbContext>();
            var roleManager = scope.ServiceProvider.GetRequiredService < RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService < UserManager<ApplicationUser>>();
            var pendingMigrations = context.Database.GetPendingMigrations();
            if(pendingMigrations.Any())
                context.Database.Migrate();

            DataSeeding.SeedData(context);
            IDentityDbContextSeeding.SeedData(roleManager, userManager).Wait();

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
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=Login}/{id?}");

            app.Run();
        }
    }
}
