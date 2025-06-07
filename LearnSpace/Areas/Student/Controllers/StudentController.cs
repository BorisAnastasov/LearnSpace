using LearnSpace.Core.Interfaces;
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
            var studentModel = await studentService.GetStudentDashboardInformationAsync(GetUserId());

            return View(studentModel);
        }

    }
}
