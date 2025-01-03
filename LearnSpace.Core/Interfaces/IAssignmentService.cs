using LearnSpace.Core.Models.Assignment;

namespace LearnSpace.Core.Interfaces
{
    public interface IAssignmentService
    {
        Task<AssignmentsClassViewModel> GetAllAssignmentsByClassForStudentAsync(string userId, int classId);
        Task<AllAssignmentsForTeacherViewModel> GetAllAssignmentsByClassForTeacherAsync(string userId, int classId);
        Task<AllAssignmentsViewModel> GetAllAssignmentsAsync(string userId);
        Task<AssignmentInfoViewModel> GetAssignmentInfoAsync(string userId, int assignmentId);
        Task<AssignmentsInfoForTeacherViewModel> GetAssignmentInfoForTeacherAsync(int assignmentId);
        Task CreateAssignment(CreateAssignmentFormModel model);
        Task<bool> ExistsByIdAsync(int id);
        Task<int> DeleteAssignment(int id);
        Task<bool> ClassExistsByIdAsync(int id);
    }
}
