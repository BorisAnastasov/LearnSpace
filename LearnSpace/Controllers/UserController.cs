using LearnSpace.Core.Models.Account;
using static LearnSpace.Common.RoleConstants;
using LearnSpace.Infrastructure.Database.Entities.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using LearnSpace.Common;
using Microsoft.EntityFrameworkCore;
using LearnSpace.Infrastructure.Database.Repository;

namespace LearnSpace.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;

        private readonly SignInManager<ApplicationUser> signInManager;

        private readonly RoleManager<IdentityRole<Guid>> roleManager;
        private readonly IRepository repository;

        public UserController(
            UserManager<ApplicationUser> _userManager,
            SignInManager<ApplicationUser> _signInManager,
            RoleManager<IdentityRole<Guid>> _roleManager,
			IRepository _repository)
        {
            userManager = _userManager;
            signInManager = _signInManager;
            roleManager = _roleManager;
            repository = _repository;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            if (User?.Identity?.IsAuthenticated ?? false)
            {
                return RedirectToAction("Index", "Home");
            }

            return View("StudentOrTeacher");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult RegisterTeacher()
        {
            if (User?.Identity?.IsAuthenticated ?? false)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult RegisterStudent()
        {
            if (User?.Identity?.IsAuthenticated ?? false)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterStudent(RegisterStudentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }


            var user = new ApplicationUser()
            {
				FirstName = model.FirstName,
				LastName = model.LastName,
				NormalizedEmail = model.Email.ToUpper(),
				Email = model.Email,
				UserName = model.Email,
				NormalizedUserName = model.Email.ToUpper(),
			};

            var result = await userManager.CreateAsync(user, model.Password);


            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "Student");

				var student = new Student
				{
					ApplicationUserId = user.Id,
					ApplicationUser = user
				};
				await repository.AddAsync(student);
				await repository.SaveChangesAsync();

				return RedirectToAction("Login", "User");
            }


			foreach (var item in result.Errors)
            {
                ModelState.AddModelError("", item.Description);
            }

            return View(model);
        }

		[HttpPost]
		[AllowAnonymous]
		public async Task<IActionResult> RegisterTeacher(RegisterTeacherViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}


			var user = new ApplicationUser()
			{
				FirstName = model.FirstName,
				LastName = model.LastName,
				NormalizedEmail = model.Email.ToUpper(),
				Email = model.Email,
				UserName = model.Email,
				NormalizedUserName = model.Email.ToUpper(),
			};

			var result = await userManager.CreateAsync(user, model.Password);


			if (result.Succeeded)
			{
				await userManager.AddToRoleAsync(user, "Teacher");

				var teacher = new Teacher
				{
					ApplicationUserId = user.Id,
					ApplicationUser = user,
                    SubjectSpecialization = model.SubjectSpecialization
				};
				await repository.AddAsync(teacher);
				await repository.SaveChangesAsync();

				return RedirectToAction("Login", "User");
			}

			foreach (var item in result.Errors)
			{
				ModelState.AddModelError("", item.Description);
			}

			return View(model);
		}

		[HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            if (User?.Identity?.IsAuthenticated ?? false)
            {
                return RedirectToAction("Index", "Home");
            }

            var model = new LogInViewModel();


            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LogInViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await userManager.FindByEmailAsync(model.Email);

            if (user != null)
            {
                var result = await signInManager.PasswordSignInAsync(user, model.Password, false, false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home", new { area = "" });
                }
            }

            ModelState.AddModelError("", "Invalid login");

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> CreateRoles()
        {
            await roleManager.CreateAsync(new IdentityRole<Guid>(RoleConstants.AdminRoleName));

            return RedirectToAction("Index", "Home");
        }
        
        public async Task<IActionResult> AddUserToRoles()
        {
            string email1 = "admin@gmail.com";

            var user = await userManager.FindByEmailAsync(email1);

            await userManager.AddToRoleAsync(user, RoleConstants.AdminRoleName);

            return RedirectToAction("Index", "Home");
        }


    }
}
