using LearnSpace.Core.Models.Class;
using LearnSpace.Core.Models.Enumerations;

namespace LearnSpace.Core.Interfaces
{
    public interface IClassService
    {
        Task<AllClassesViewModel> GetAllClassesForStudentAsync(string userId);
        Task<AllClassesViewModel> GetAllClassesForTeacherAsync(string userId);
        Task LeaveClassAsync(string userId, int classId);
        Task<AllClassesViewModel> GetAllClassesAsync(
                                        string userId,
                                        string? searchTerm = null,
                                        ClassSorting sorting = ClassSorting.GroupCapacityDescending,
                                        int currPage = 1,
                                        int classesPerPage = 15);
        Task JoinClassAsync(string userId, int id);

        CreateClassModel GetCreateClassModel(string id);
        Task CreateClassAsync(CreateClassModel model);
        Task DeleteClassAsync(int id);
        Task<bool> ExistsByIdAsync(int id);
    }
}
