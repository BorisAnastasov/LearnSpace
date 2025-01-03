using LearnSpace.Core.Interfaces;
using LearnSpace.Core.Models.Assignment;
using Microsoft.AspNetCore.Mvc;

namespace LearnSpace.Web.Areas.Student.Controllers
{
    public class SubmissionController : BaseController
    {
        private readonly ISubmissionService submissionService;

        public SubmissionController(ISubmissionService _submissionService)
        {
            submissionService = _submissionService;
        }
        [HttpPost]
        public async Task<IActionResult> SubmitAssignment(int assignmentId, IFormFile filePath)
        {
			if (!(await submissionService.AssignmentExistsByIdAsync(assignmentId)))
			{
				ModelState.AddModelError("", "The assignment does not exist.");
				return RedirectToAction("AssignmentInfo", "Assignment", new AssignmentInfoViewModel
				{
					Id = assignmentId,
					Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()
				});
			}

			if (filePath == null || filePath.Length == 0)
			{
				ModelState.AddModelError("filePath", "Please upload a file.");
			}

			if (!submissionService.ContainsOnlyAllowedFileType(filePath))
			{
				ModelState.AddModelError("filePath", "Invalid file type. Allowed types are .pdf, .docx, .txt.");
			}

			if (!submissionService.SizeIsNotTooBig(filePath))
			{
				ModelState.AddModelError("filePath", "File size exceeds the maximum allowed limit of 5 MB.");
			}

			if (!ModelState.IsValid)
			{
				var model = new AssignmentInfoViewModel
				{
					Id = assignmentId,
					Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()
				};
				return RedirectToAction("AssignmentInfo", "Assignment", model);
			}

			await submissionService.CreateSubmissionAsync(GetUserId(), assignmentId, filePath);

			return RedirectToAction("AssignmentInfo", "Assignment", new { id = assignmentId });
		}

    }
}
