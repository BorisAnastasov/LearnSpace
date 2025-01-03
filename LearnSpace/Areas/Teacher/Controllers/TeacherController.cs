using LearnSpace.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LearnSpace.Web.Areas.Teacher.Controllers
{
    public class TeacherController : BaseController
    {
        private readonly ITeacherService teacherService;

        public TeacherController(ITeacherService _teacherService)
        {
            teacherService = _teacherService;
        }
        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            var model = await teacherService.GetTeacherDashboardInformationAsync(GetUserId());

            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> GradeBook(int classId)
        {
            if (!(await teacherService.ClassExistsByIdAsync(classId))) 
            {
                return RedirectToAction("Error404", "Error");
            }
            var model = await teacherService.GetGradeBookByClassAsync(GetUserId(), classId);

            return View(model);
        }
    }
}
