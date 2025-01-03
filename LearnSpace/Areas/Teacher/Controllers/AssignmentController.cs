using LearnSpace.Core.Interfaces;
using LearnSpace.Core.Models.Assignment;
using Microsoft.AspNetCore.Mvc;

namespace LearnSpace.Web.Areas.Teacher.Controllers
{
    public class AssignmentController : BaseController
    {
        private readonly IAssignmentService assignmentService;

        public AssignmentController(IAssignmentService _assignmentService)
        {
            assignmentService = _assignmentService;
        }

        [HttpGet]
        public async Task<IActionResult> AllAssignmentsClassTeacher(int classId)
        {
            if (!(await assignmentService.ClassExistsByIdAsync(classId)))
            {
                return RedirectToAction("Error404", "Error");
            }
            var assignments = await assignmentService.GetAllAssignmentsByClassForTeacherAsync(GetUserId(), classId);

            return View(assignments);
        }

        [HttpGet]
        public async Task<IActionResult> AssignmentInfoForTeacher(int id)
        {
            if (!(await assignmentService.ExistsByIdAsync(id)))
            {
                return RedirectToAction("Error404", "Error");
            }
            var assignment = await assignmentService.GetAssignmentInfoForTeacherAsync(id);

            return View(assignment);
        }

        [HttpGet]
        public async Task<IActionResult> CreateAssignment(int classId)
        {
            if (!(await assignmentService.ClassExistsByIdAsync(classId)))
            {
                return RedirectToAction("Error404", "Error");
            }
            var model = new CreateAssignmentFormModel
            {
                ClassId = classId
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAssignment(CreateAssignmentFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            await assignmentService.CreateAssignment(model);

            return RedirectToAction(nameof(AllAssignmentsClassTeacher), new { classId = model.ClassId });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAssignment(int id)
        {
            if (!(await assignmentService.ExistsByIdAsync(id)))
            {
                return RedirectToAction("Error404", "Error");
            }
            var classId = await assignmentService.DeleteAssignment(id);

            return RedirectToAction(nameof(AllAssignmentsClassTeacher), new { classId = classId });
        }
    }
}
