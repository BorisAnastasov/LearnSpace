﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static LearnSpace.Web.Areas.Admin.Constants.AdminConstants;

namespace LearnSpace.Web.Areas.Admin.Controllers
{
    [Area(AdminAreaName)]
	[Authorize(Roles = AdminRoleName)]
	public class BaseController : Controller
    {
        protected string GetUserId()
        {
            return User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
