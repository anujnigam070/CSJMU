using CoreLayout.Models.UserManagement;
using CoreLayout.Services.Masters.Role;
using CoreLayout.Services.Registration;
using CoreLayout.Services.UserManagement.AssignRole;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AssignRoleController : Controller
    {
        private readonly ILogger<AssignRoleController> _logger;
        private readonly IAssignRoleService _assignRoleService;
        private readonly IRegistrationService _registrationService;
        private readonly IRoleService _roleService;

        public AssignRoleController(ILogger<AssignRoleController> logger, IAssignRoleService assignRoleService, IRegistrationService registrationService, IRoleService roleService)
        {
            _logger = logger;
            _assignRoleService = assignRoleService;
            _registrationService = registrationService;
            _roleService = roleService;
        }
        public async Task<IActionResult> Index()
        {
          
            return View(await _assignRoleService.GetAllRoleAssignAsync());
        }

        //Create Get Action Method
        public ActionResult Create()
        {
            var UserList = (from user in _registrationService.GetAllRegistrationAsync().Result
                            select new SelectListItem()
                            {
                                Text = user.UserName,
                                Value = user.UserID.ToString(),
                            }).ToList();

            UserList.Insert(0, new SelectListItem()
            {
                Text = "----Select----",
                Value = string.Empty
            });

            var RoleList = (from role in _roleService.GetAllRoleAsync().Result
                            select new SelectListItem()
                            {
                                Text = role.RoleName,
                                Value = role.RoleID.ToString(),
                            }).ToList();

            RoleList.Insert(0, new SelectListItem()
            {
                Text = "----Select----",
                Value = string.Empty
            });
            ViewBag.UserList = UserList;
            ViewBag.RoleList = RoleList;
            return View();
        }

        //Create Post Action Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RegistrationRoleMapping roleModel)
        {
            if (HttpContext.Session.GetString("Name") != null)
            {
                roleModel.CreatedBy = HttpContext.Session.GetString("Name");
                roleModel.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
               // roleModel.RoleUserId = // Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
                //countryModel.CreatedDate = System.DateTime.Now;
                if (ModelState.IsValid)
                {

                    var res = await _assignRoleService.CreateRoleAssignAsync(roleModel);
                    if (res.ToString().Equals("1"))
                    {
                        TempData["success"] = "Role Assign has been saved";
                    }
                    else
                    {
                        TempData["error"] = "Role Assign has not been saved";
                    }
                    return RedirectToAction(nameof(Index));
                }
                return View(roleModel);
            }
            else
            {
                TempData["error"] = "Some thing went wrong!";
                return RedirectToAction(nameof(Index));

            }
        }
        public async Task<IActionResult> Details(int id)
        {
            if (HttpContext.Session.GetString("Name") != null)
            {
                var UserList = (from user in _registrationService.GetAllRegistrationAsync().Result
                                select new SelectListItem()
                                {
                                    Text = user.UserName,
                                    Value = user.UserID.ToString(),
                                }).ToList();

                UserList.Insert(0, new SelectListItem()
                {
                    Text = "----Select----",
                    Value = string.Empty
                });

                var RoleList = (from role in _roleService.GetAllRoleAsync().Result
                                select new SelectListItem()
                                {
                                    Text = role.RoleName,
                                    Value = role.RoleID.ToString(),
                                }).ToList();

                RoleList.Insert(0, new SelectListItem()
                {
                    Text = "----Select----",
                    Value = string.Empty
                });
                ViewBag.UserList = UserList;
                ViewBag.RoleList = RoleList;
                return View(await _assignRoleService.GetRoleAssignByIdAsync(id));
            }
            else
            {
                return RedirectToAction("Login", "Home");

            }
        }
        public async Task<IActionResult> Edit(int id)
        {
            if (HttpContext.Session.GetString("Name") != null)
            {
                var UserList = (from user in _registrationService.GetAllRegistrationAsync().Result
                                select new SelectListItem()
                                {
                                    Text = user.UserName,
                                    Value = user.UserID.ToString(),
                                }).ToList();

                UserList.Insert(0, new SelectListItem()
                {
                    Text = "----Select----",
                    Value = string.Empty
                });

                var RoleList = (from role in _roleService.GetAllRoleAsync().Result
                                select new SelectListItem()
                                {
                                    Text = role.RoleName,
                                    Value = role.RoleID.ToString(),
                                }).ToList();

                RoleList.Insert(0, new SelectListItem()
                {
                    Text = "----Select----",
                    Value = string.Empty
                });
                ViewBag.UserList = UserList;
                ViewBag.RoleList = RoleList;
                return View(await _assignRoleService.GetRoleAssignByIdAsync(id));
            }
            else
            {
                return RedirectToAction("Login", "Home");

            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, RegistrationRoleMapping roleModel)
        {
            try
            {
                if (HttpContext.Session.GetString("Name") != null)
                {
                    roleModel.ModifiedBy = HttpContext.Session.GetString("Name");
                    roleModel.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
                    if (ModelState.IsValid)
                    {
                        var dbRole = await _assignRoleService.GetRoleAssignByIdAsync(id);
                        if (await TryUpdateModelAsync<RegistrationRoleMapping>(dbRole))
                        {
                            roleModel.UserId = (int)HttpContext.Session.GetInt32("UserId");
                            var res = await _assignRoleService.UpdateRoleAssignAsync(roleModel);
                            if (res.ToString().Equals("1"))
                            {
                                TempData["success"] = "Role Assign has been updated";
                            }
                            else
                            {
                                TempData["error"] = "Role Assign has not been updated";
                            }
                            return RedirectToAction(nameof(Index));
                        }
                    }
                }
                else
                {
                    return RedirectToAction("Login", "Home");

                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }
            return View(roleModel);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {

            if (HttpContext.Session.GetString("Name") != null)
            {
                try
                {
                    var dbRole = await _assignRoleService.GetRoleAssignByIdAsync(id);
                    if (dbRole != null)
                    {
                        var res = await _assignRoleService.DeleteRoleAssignAsync(dbRole);

                        if (res.ToString().Equals("1"))
                        {
                            TempData["error"] = "Role Assign has been deleted";
                        }
                        else
                        {
                            TempData["error"] = "Role Assign has not been deleted";
                        }
                    }
                    else
                    {
                        TempData["error"] = "Some thing went wrong!";
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.ToString());
                }

                return RedirectToAction(nameof(Index));
            }
            else
            {
                return RedirectToAction("Login", "Home");

            }
        }
    }
}

