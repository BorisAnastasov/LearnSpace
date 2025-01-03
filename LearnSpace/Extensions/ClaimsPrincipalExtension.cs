using LearnSpace.Infrastructure.Database.Entities.Account;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using static LearnSpace.Common.RoleConstants;

namespace LearnSpace.Web.Extensions
{
    public static class ClaimsPrincipalExtension
    {
        public static string Id(this ClaimsPrincipal user)
        {
            return user.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public static bool IsUser(this ClaimsPrincipal user)
        {
            return user.Identity.IsAuthenticated;
        }
        public static bool IsStudent(this ClaimsPrincipal user)
        {

            return user.IsInRole(StudentRoleName);
        }
        public static bool IsTeacher(this ClaimsPrincipal user)
        {
            return user.IsInRole(TeacherRoleName);
        }

        public static bool IsAdmin(this ClaimsPrincipal user)
        {
            return user.IsInRole(AdminRoleName);
        }
    }
}
