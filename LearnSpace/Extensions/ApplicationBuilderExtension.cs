using LearnSpace.Infrastructure.Database.Entities.Account;
using Microsoft.AspNetCore.Identity;
using static LearnSpace.Common.RoleConstants;

namespace LearnSpace.Web.Extensions
{
    public static class ApplicationBuilderExtension
    {
        public static IApplicationBuilder SeedRoles(this IApplicationBuilder app)
        {
            using var scopedServices = app.ApplicationServices.CreateScope();

            var services = scopedServices.ServiceProvider;

            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

            Task.Run(async ()=> 
            {
                var roles = new[] { AdminRoleName, TeacherRoleName, StudentRoleName };

                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole<Guid>(role));
                    }
                }

                var admin = await userManager.FindByEmailAsync(AdminEmail);

                await userManager.AddToRoleAsync(admin, AdminRoleName);


                var studentEmails = new[] { "student1@abv.bg", "student2@abv.bg", "student3@abv.bg", "student4@abv.bg", "student5@abv.bg" };

                foreach (var email in studentEmails)
                {
                    var user = await userManager.FindByEmailAsync(email);
                    if (user != null && await roleManager.RoleExistsAsync("Student"))
                    {
                        await userManager.AddToRoleAsync(user, "Student");
                    }
                }

                var teacherEmails = new[] { "teacher1@abv.bg", "teacher2@abv.bg" };

                foreach (var email in teacherEmails)
                {
                    ApplicationUser user = await userManager.FindByEmailAsync(email);
                    if (user != null)
                    {
                        await userManager.AddToRoleAsync(user, "Teacher");
                    }
                }
            })
            .GetAwaiter()
            .GetResult();

            return app;
        }
    }
}
