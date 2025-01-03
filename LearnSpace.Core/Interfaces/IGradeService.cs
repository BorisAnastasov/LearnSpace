using LearnSpace.Core.Models.Grade;

namespace LearnSpace.Core.Interfaces
{
    public interface IGradeService
    {
        Task<List<GradeCourseViewModel>> GetAllGradesAsync(string userId);
        Task<GradeInfoViewModel> GetGradeInfoAsync(int id);
        Task<CreateGradeViewModel> GetAddGradeModelAsync(int classId, string userId);
        Task CreateGradeAsync(CreateGradeViewModel model);
        Task<int> DeleteGradeAsync(int id);
        Task<bool> ExistsByIdAsync(int id);
        Task<bool> ClassExistsByIdAsync(int id);
        Task<bool> UserExistsByIdAsync(string userId);

    }
}
