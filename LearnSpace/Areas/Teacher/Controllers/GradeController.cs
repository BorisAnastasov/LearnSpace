using LearnSpace.Core.Interfaces;
using LearnSpace.Core.Models.Grade;
using Microsoft.AspNetCore.Mvc;

namespace LearnSpace.Web.Areas.Teacher.Controllers
{

    public class GradeController : BaseController
    {
        private readonly IGradeService gradeService;

        public GradeController(IGradeService _gradeService)
        {
            gradeService = _gradeService;
        }

        [HttpGet]
        public async Task<IActionResult> GradeInfo(int id)
        {
            if (!(await gradeService.ExistsByIdAsync(id))) 
            {
                return RedirectToAction("Error404", "Error");
            }
            var grade = await gradeService.GetGradeInfoAsync(id);

            return View(grade);
        }
        [HttpGet]
        public async Task<IActionResult> AddGrade(int classId, string studentId)
        {
            if (!(await gradeService.UserExistsByIdAsync(studentId)))
            {
                return RedirectToAction("Error404", "Error");
            }

            if (!(await gradeService.ClassExistsByIdAsync(classId)))
            {
                return RedirectToAction("Error404", "Error");
            }

            var model = await gradeService.GetAddGradeModelAsync(classId, studentId);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddGrade(CreateGradeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await gradeService.CreateGradeAsync(model);

            return RedirectToAction("GradeBook", "Teacher", new { classId = model.CourseId });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteGrade(int gradeId)
        {
            if (!(await gradeService.ExistsByIdAsync(gradeId)))
            {
                return RedirectToAction("Error404", "Error");
            }
            var classid = await gradeService.DeleteGradeAsync(gradeId);

            return RedirectToAction("GradeBook", "Teacher", new { classId = classid });
        }


    }
}
