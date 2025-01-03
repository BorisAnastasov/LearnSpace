using LearnSpace.Core.Models.Submission;
using Microsoft.AspNetCore.Http;

namespace LearnSpace.Core.Interfaces
{
    public interface ISubmissionService
    {
        Task CreateSubmissionAsync(string userId, int assignmentId, IFormFile file);
        Task<SubmissionsViewModel> GetAllSubmissionsForAssignmentAsync(int assignmentId);
        Task<SubmissionFileModel> GetFileBySubmissionIdAsync(int submissionId);
        Task DeleteSubmissionIdAsync(int submissionId);
        Task<SubmissionsViewModel> GetAllSubmissionsForTeacherAsync(string userId);
        Task<bool> ExistsByIdAsync(int id);
        Task<bool> AssignmentExistsByIdAsync(int id);
        bool ContainsOnlyAllowedFileType(IFormFile file);
        bool SizeIsNotTooBig(IFormFile file);
    }
}
