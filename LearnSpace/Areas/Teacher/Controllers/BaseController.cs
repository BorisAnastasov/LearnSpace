using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static LearnSpace.Web.Areas.Teacher.Constants.TeacherConstants;

namespace LearnSpace.Web.Areas.Teacher.Controllers
{
    [Area(AreaName)]
    [Authorize(Roles = TeacherRole)]
    public class BaseController : Controller
    {
        protected string GetUserId()
        {
            return User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
