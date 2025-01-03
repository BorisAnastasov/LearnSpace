using LearnSpace.Core.Models.Admin;

namespace LearnSpace.Core.Interfaces
{
    public interface IAdminService
    {
        Task<List<UserViewModel>> GetAllUsersAsync();
        Task AddRoleAsync(string userId, string role);
        Task DeleteRoleAsync(string userId, string role);
        Task DeleteUserAsync(string userId);
        Task<bool> RoleExistsByNameAsync(string role);
        Task<bool> UserExistsByIdAsync(string userId);
    }
}
