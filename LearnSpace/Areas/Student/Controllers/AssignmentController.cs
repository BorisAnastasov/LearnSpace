using LearnSpace.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LearnSpace.Web.Areas.Student.Controllers
{
    public class AssignmentController : BaseController
    {
        private readonly IAssignmentService assignmentService;

        public AssignmentController(IAssignmentService _assignmentService)
        {
            assignmentService = _assignmentService;
        }

        [HttpGet]
        public async Task<IActionResult> AllAssignments() 
        {
            var assignments = await assignmentService.GetAllAssignmentsAsync(UserId);

            return View(assignments);
        }

        [HttpGet]
        public async Task<IActionResult> AllAssignmentsClassStudent(int classId)
        {
            if (!(await assignmentService.ClassExistsByIdAsync(classId))) 
            {
                return RedirectToAction("Error404", "Error"); 
            }
            var assignments = await assignmentService.GetAllAssignmentsByClassForStudentAsync(UserId, classId);

            return View(assignments);
        }

        [HttpGet]
        public async Task<IActionResult> AssignmentInfo(int id) 
        {
            if (!(await assignmentService.ExistsByIdAsync(id)))
            {
                return RedirectToAction("Error404", "Error");
            }

            var assignment = await assignmentService.GetAssignmentInfoAsync(UserId,id);

            return View(assignment);
        }

        private string UserId => GetUserId();
    }
}
