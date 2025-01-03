using LearnSpace.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LearnSpace.Web.Areas.Student.Controllers
{
    public class GradeController : BaseController
    {
        private readonly IGradeService gradeService;

        public GradeController(IGradeService _gradeService)
        {
            gradeService = _gradeService;
        }

        [HttpGet]
        public async Task<IActionResult> AllGrades()
        {
            var list = await gradeService.GetAllGradesAsync(GetUserId());

            return View(list);
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

    }
}
