using LearnSpace.Core.Interfaces;
using LearnSpace.Web.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace LearnSpace.Web.Areas.Teacher.Controllers
{
    public class SubmissionController : BaseController
    {
        private readonly ISubmissionService submissionService;

        public SubmissionController(ISubmissionService _submissionService)
        {
            submissionService = _submissionService;
        }
        [HttpGet]
        public async Task<IActionResult> AllSubmissionsForAssignment(int assignmentId)
        {
            if (!(await submissionService.AssignmentExistsByIdAsync(assignmentId))) 
            {
                return RedirectToAction("Error404", "Error");
            }
            var model = await submissionService.GetAllSubmissionsForAssignmentAsync(assignmentId);

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> DownloadFile(int submissionId)
        {
            if (!(await submissionService.ExistsByIdAsync(submissionId)))
            {
                return RedirectToAction("Error404", "Error");
            }

            var model = await submissionService.GetFileBySubmissionIdAsync(submissionId);

            if (model == null)
            {
                return RedirectToAction("Error404", "Error");
            }

            return File(model.FileContent, model.FileType, model.FileName);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteSubmission(int submissionId, int assignmentId)
        {

            if (!(await submissionService.ExistsByIdAsync(submissionId)))
            {
                return RedirectToAction("Error404", "Error");
            }
            await submissionService.DeleteSubmissionIdAsync(submissionId);
            if (assignmentId != 0)
            {
                if (!(await submissionService.AssignmentExistsByIdAsync(assignmentId)))
                {
                    return RedirectToAction("Error404", "Error");
                }
                return RedirectToAction(nameof(AllSubmissionsForAssignment), new { assignmentId });
            }
            else 
            {
                return RedirectToAction(nameof(AllSubmissionsForTeacher));
            }
        }

        [HttpGet]
        public async Task<IActionResult> AllSubmissionsForTeacher()
        {
            var model = await submissionService.GetAllSubmissionsForTeacherAsync(GetUserId());

            return View(model);
        }
    }
}
