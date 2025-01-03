using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static LearnSpace.Web.Areas.Student.Constants.StudentConstants;

namespace LearnSpace.Web.Areas.Student.Controllers
{
    [Area(StudentAreaName)]
	[Authorize(Roles = StudentAreaName)]
	public class BaseController : Controller
    {
        protected string GetUserId()
        {
            return User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
