using LearnSpace.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LearnSpace.Web.Areas.Admin.Controllers
{
    public class AdminController : BaseController
    {
        private IAdminService adminService;

        public AdminController(IAdminService _adminService)
        {
                adminService = _adminService;
        }

        [HttpGet]
        public async Task<IActionResult> AllUsers() 
        {
            var model = await adminService.GetAllUsersAsync();

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> AddRole(string userId, string role) 
        {
            if (string.IsNullOrWhiteSpace(role))
            {
                return RedirectToAction("Error404", "Error");
            }
            if (!await adminService.UserExistsByIdAsync(userId)) 
            {
                return RedirectToAction("Error404", "Error");
            }
            if (!await adminService.RoleExistsByNameAsync(role))
            {
                return RedirectToAction("Error404", "Error");
            }
            await adminService.AddRoleAsync(userId, role);

            return RedirectToAction(nameof(AllUsers));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRole(string userId, string role)
        {
            if (string.IsNullOrWhiteSpace(role))
            {
                return RedirectToAction("Error404", "Error");
            }
            if (!await adminService.UserExistsByIdAsync(userId))
            {
                return RedirectToAction("Error404", "Error");
            }
            if (!await adminService.RoleExistsByNameAsync(role))
            {
                return RedirectToAction("Error404", "Error");
            }
            await adminService.DeleteRoleAsync(userId, role);

            return RedirectToAction(nameof(AllUsers));
        }
        [HttpPost]
        public async Task<IActionResult> DeleteUser(string userId) 
        {
            if (!await adminService.UserExistsByIdAsync(userId))
            {
                return RedirectToAction("Error404", "Error");
            }

            await adminService.DeleteUserAsync(userId);

            return RedirectToAction(nameof(AllUsers));
        }
    }
}
