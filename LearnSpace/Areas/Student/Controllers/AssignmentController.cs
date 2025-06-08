using LearnSpace.Core.Interfaces;
using LearnSpace.Core.Models.Assignment;
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

        [HttpGet("AssignmentInfo/{id}")]
        public async Task<IActionResult> AssignmentInfo(int id) 
        {
            if (!(await assignmentService.ExistsByIdAsync(id)))
            {
                return RedirectToAction("Error404", "Error");
            }

            var assignment = await assignmentService.GetAssignmentInfoAsync(UserId,id);

            return View(assignment);
        }

		[HttpGet]
		public async Task<IActionResult> AssignmentInfo(AssignmentInfoViewModel model)
		{
			if (!(await assignmentService.ExistsByIdAsync(model.AssignmentId)))
			{
				return RedirectToAction("Error404", "Error");
			}

			var assignment = await assignmentService.GetAssignmentInfoAsync(UserId, model.AssignmentId);
            assignment.Errors = model.Errors;

			return View(assignment);
		}

		private string UserId => GetUserId();
    }
}
