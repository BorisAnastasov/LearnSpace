using LearnSpace.Core.Interfaces;
using LearnSpace.Core.Models.Admin;
using LearnSpace.Infrastructure.Database.Entities;
using LearnSpace.Infrastructure.Database.Entities.Account;
using LearnSpace.Infrastructure.Database.Repository;
using Microsoft.AspNetCore.Identity;

namespace LearnSpace.Core.Services
{
    public class AdminService : IAdminService
    {
        private readonly IRepository repository;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole<Guid>> roleManager;

        public AdminService(
                UserManager<ApplicationUser> _userManager,
                IRepository _repository,
                RoleManager<IdentityRole<Guid>> roleManager)
        {
            userManager = _userManager;
            repository = _repository;
            this.roleManager = roleManager;

        }

        public async Task AddRoleAsync(string userId, string role)
        {
            var user = await repository.GetByIdAsync<ApplicationUser>(Guid.Parse(userId));

            await userManager.AddToRoleAsync(user, role);
        }

        public async Task DeleteRoleAsync(string userId, string role)
        {
            var user = await repository.GetByIdAsync<ApplicationUser>(Guid.Parse(userId));

            await userManager.RemoveFromRoleAsync(user, role);
        }

        public async Task DeleteUserAsync(string userId)
        {
            var user = await repository.GetByIdAsync<ApplicationUser>(Guid.Parse(userId));
            if (user.Student != null) 
            {
                var student = await repository.GetStudentAsync(userId);
                var studentCourses = student.StudentCourses;
                repository.DeleteRange<StudentCourse>(studentCourses);
                await repository.DeleteAsync<Student>(student.Id);
            }
            else if (user.Teacher != null)
            {
                var teacher = await repository.GetTeacherAsync(userId);
                var CourseStudents = teacher.Courses.SelectMany(c => c.CourseStudents);
                repository.DeleteRange<StudentCourse>(CourseStudents);
                await repository.DeleteAsync<Teacher>(teacher.Id);
            }

            await repository.DeleteAsync<ApplicationUser>(Guid.Parse(userId));

            await repository.SaveChangesAsync();
		}

		public async Task<List<UserViewModel>> GetAllUsersAsync()
        {
            var users = repository.AllReadOnly<ApplicationUser>()
                                    .Select(a=>new UserViewModel 
                                    {
                                        Id= a.Id.ToString().ToLower(),
                                        UserName = a.UserName,
                                        FullName = a.FirstName + " " + a.LastName,
                                    }).ToList();

            foreach (var a in users) 
            {
                var user = await repository.GetByIdAsync<ApplicationUser>(Guid.Parse(a.Id));

                var roles = await userManager.GetRolesAsync(user);

                a.Roles = roles.ToList();
            }
            
            return users;
        }

        public async Task<bool> RoleExistsByNameAsync(string role)
        {
            if (string.IsNullOrWhiteSpace(role))
            {
                throw new ArgumentException("Role name cannot be null or whitespace.");
            }

            return await roleManager.RoleExistsAsync(role);
        }

        public async Task<bool> UserExistsByIdAsync(string userId)
        {
            var user = await repository.GetByIdAsync<ApplicationUser>(Guid.Parse(userId));

            return user != null;
        }
    }
}
