using CoreLayout.Models.Masters;
using CoreLayout.Services.Common;
using CoreLayout.Services.Masters.Dashboard;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreLayout.Controllers
{

    public class CommonController : Controller
    {
        private readonly ILogger<CommonController> _logger;
        private readonly IDashboardService _dashboardService;
        private readonly ICommonService _commonService;
        public CommonController(ILogger<CommonController> logger, IDashboardService dashboardService, ICommonService commonService)
        {
            _logger = logger;
            _dashboardService = dashboardService;
            _commonService = commonService;
        }
        public async  Task<ActionResult> RefereshMenuAsync()
        {
            var role = @User.FindFirst(claim => claim.Type == System.Security.Claims.ClaimTypes.Role)?.Value;
            int userid = (int)HttpContext.Session.GetInt32("UserId");
            int roleid = (int)HttpContext.Session.GetInt32("RoleId");
            if (roleid != 0 && userid != 0)
            {

                //ViewBag.Menu=   await _dashboardService.GetDashboardByRole(role);
                IDashboardService _dashboardService1 = _dashboardService;
                List<DashboardModel> alllevels = await _dashboardService1.GetDashboardByRoleAndUser(roleid, userid);

                List<DashboardModel> level1 = new List<DashboardModel>();
                List<DashboardModel> level2 = new List<DashboardModel>();
                List<DashboardModel> level3 = new List<DashboardModel>();

                foreach (DashboardModel dm in alllevels)
                {
                    if (dm.Level2.Equals("*") && dm.Level3.Equals("*"))
                    {
                        level1.Add(dm);
                    }
                    else if (dm.Level2 != "*" && dm.Level3.Equals("*"))
                    {
                        level2.Add(dm);
                    }
                    else
                    {
                        level3.Add(dm);
                    }
                }

                HttpContext.Session.SetString("Level1List", JsonConvert.SerializeObject(level1));
                HttpContext.Session.SetString("Level2List", JsonConvert.SerializeObject(level2));
                HttpContext.Session.SetString("Level3List", JsonConvert.SerializeObject(level3));

                return RedirectToAction("DashBoard", "Index"); ;
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        
    }
}
