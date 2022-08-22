using CoreLayout.Models.UserManagement;
using CoreLayout.Services.Masters.Role;
using CoreLayout.Services.UserManagement.ButtonPermission;
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
    public class ButtonPermissionController : Controller
    {
        private readonly ILogger<ButtonPermissionController> _logger;
        private readonly IButtonPermissionService _buttonPermissionService;
        private readonly IRoleService _roleService;
        public ButtonPermissionController(ILogger<ButtonPermissionController> logger, IButtonPermissionService buttonPermissionService, IRoleService roleService)
        {
            _logger = logger;
            _buttonPermissionService = buttonPermissionService;
            _roleService = roleService;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _buttonPermissionService.GetAllButtonPermissionAsync());
        }

        public void bindRoleDropdown()
        {
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

            ViewBag.RoleList = RoleList;
        }
        public void bindUserDropdown(int RoleId)
        {
           
            var GetUserList = (from user in _buttonPermissionService.GetAllUsersAsync(RoleId).Result
                               select new SelectListItem()
                               {
                                   Text = user.UserName,
                                   Value = user.UserID.ToString(),
                               }).ToList();
            GetUserList.Insert(0, new SelectListItem()
            {
                Text = "----Select----",
                Value = string.Empty
            });
            ViewBag.GetUserList = GetUserList;
        }
        //Create Get Action Method
        public ActionResult Create()
        {
            bindRoleDropdown();
            return View();
        }

        //Create Post Action Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ButtonPermissionModel buttonPermissionModel)
        {
            if (HttpContext.Session.GetString("Name") != null)
            {

                buttonPermissionModel.CreatedBy = HttpContext.Session.GetString("Name");
                if (ModelState.IsValid)
                {
                    var res = await _buttonPermissionService.CreateButtonPermissionAsync(buttonPermissionModel);
                    if (res > 0)
                    {
                        TempData["success"] = "ButtonPermission has been saved";
                    }
                    else
                    {
                        TempData["error"] = "ButtonPermission has not been saved";
                    }
                    return RedirectToAction(nameof(Index));
                }

            }
            return RedirectToAction("Create");
        }

        public JsonResult GetUser(int RoleId)
        {
            var GetUserList = (from user in _buttonPermissionService.GetAllUsersAsync(RoleId).Result
                               select new SelectListItem()
                             {
                                 Text = user.UserName,
                                 Value = user.UserID.ToString(),
                             }).ToList();
            GetUserList.Insert(0, new SelectListItem()
            {
                Text = "----Select----",
                Value = string.Empty
            });

            //var GetUserList = _buttonPermissionService.GetUserByRoleAsync(role);
               
            return Json(GetUserList);
        }

        public async Task<IActionResult> Details(int id)
        {
            if (HttpContext.Session.GetString("Name") != null)
            {
                return View(await _buttonPermissionService.GetButtonPermissionByIdAsync(id));
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
                var list = await _buttonPermissionService.GetButtonPermissionByIdAsync(id);
                bindRoleDropdown();
                bindUserDropdown(list.RoleId);
                return View( list);
            }
            else
            {
                return RedirectToAction("Login", "Home");

            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ButtonPermissionModel buttonPermissionModel)
        {
            try
            {
                if (HttpContext.Session.GetString("Name") != null)
                {
                    buttonPermissionModel.ModifiedBy = HttpContext.Session.GetString("Name");
                    //buttonPermissionModel.RoleId = id;
                    if (ModelState.IsValid)
                    {
                        var dbRole = await _buttonPermissionService.GetButtonPermissionByIdAsync(id);
                        if (await TryUpdateModelAsync<ButtonPermissionModel>(dbRole))
                        {
                            buttonPermissionModel.UserId = (int)HttpContext.Session.GetInt32("Id");
                            var res = await _buttonPermissionService.UpdateButtonPermissionAsync(buttonPermissionModel);
                            if (res.ToString().Equals("1"))
                            {
                                TempData["success"] = "ButtonPermission has been updated";
                            }
                            else
                            {
                                TempData["error"] = "ButtonPermission has not been updated";
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
            return View(buttonPermissionModel);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {

            if (HttpContext.Session.GetString("Name") != null)
            {
                try
                {
                    var value = await _buttonPermissionService.GetButtonPermissionByIdAsync(id);
                    if (value != null)
                    {
                        var res = await _buttonPermissionService.DeleteButtonPermissionAsync(value);

                        if (res.ToString().Equals("1"))
                        {
                            TempData["error"] = "ButtonPermission has been deleted";
                        }
                        else
                        {
                            TempData["error"] = "ButtonPermission has not been deleted";
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