using LearnSpace.Core.Interfaces;
using LearnSpace.Core.Services;
using LearnSpace.Infrastructure.Database;
using LearnSpace.Infrastructure.Database.Entities.Account;
using LearnSpace.Infrastructure.Database.Repository;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LearnSpace.Web.Extensions
{
    public static class ServiceApplicationExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IStudentService, StudentService>();
            services.AddScoped<IGradeService, GradeService>();
            services.AddScoped<IClassService, ClassService>();
            services.AddScoped<IAssignmentService, AssignmentService>();
            services.AddScoped<ITeacherService, TeacherService>();
            services.AddScoped<ISubmissionService, SubmissionService>();
            services.AddScoped<IAdminService, AdminService>();
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/AccessDenied";
            });

            return services;
        }
		public static IServiceCollection AddApplicationDbContext(this IServiceCollection services, IConfiguration config)
		{
			var connectionString = config.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
			services.AddDbContext<LearnSpaceDbContext>(options =>
			{
				options.UseSqlServer(connectionString);
				options.UseLazyLoadingProxies()
							  .UseSqlServer(connectionString);
			});

			services.AddScoped<IRepository, Repository>();

			services.AddDatabaseDeveloperPageExceptionFilter();

            return services;
        }
        public static IServiceCollection AddApplicationIdentity(this IServiceCollection services, IConfiguration config)
        {
            services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
            })
            .AddEntityFrameworkStores<LearnSpaceDbContext>()
            .AddDefaultTokenProviders();

            return services;
        }
    }
}
