using LearnSpace.Core.Interfaces;
using LearnSpace.Web.Controllers;
using LearnSpace.Web.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearnSpace.Web.Areas.Student.Controllers
{
    public class StudentController : BaseController
    {
        private readonly IStudentService studentService;

        public StudentController(IStudentService _studentService)
        {
            studentService = _studentService;
        }

        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            var student = await studentService.GetStudentDashboardInformationAsync(GetUserId());

            return View(student);
        }

    }
}
