using CoreLayout.Models.Masters;
using CoreLayout.Models.UserManagement;
using CoreLayout.Services.Common;
using CoreLayout.Services.UserManagement.ParentMenu;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class ParentMenuController : Controller
    {
        private readonly ILogger<ParentMenuController> _logger;
        private readonly IParentMenuService _parentMenuService;
        private readonly ICommonService _commonService;
        public ParentMenuController(ILogger<ParentMenuController> logger, ICommonService commonService, IParentMenuService parentMenuService)
        {
            _logger = logger;
            _commonService = commonService;
            _parentMenuService = parentMenuService;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _parentMenuService.GetAllParentMenuAsync());
        }
       
        public async Task<IActionResult> Details(int id)
        {
            if (HttpContext.Session.GetString("Name") != null)
            {
                return View(await _parentMenuService.GetParentMenuByIdAsync(id));
            }
            else
            {
                return RedirectToAction("Login", "Home");

            }
        }
        //Create Get Action Method
        public ActionResult Create()
        {
            return View();
        }

        //Create Post Action Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ParentMenuModel parentMenuModel)
        {
            if (HttpContext.Session.GetString("Name") != null)
            {
                parentMenuModel.CreatedBy = HttpContext.Session.GetString("Name");
                //parentMenuModel.RoleId = (int)HttpContext.Session.GetInt32("RoleId");
                parentMenuModel.UserId = (int)HttpContext.Session.GetInt32("UserId");
                //countryModel.CreatedDate = System.DateTime.Now;


                if (ModelState.IsValid)
                {
                    var res = await _parentMenuService.CreateParentMenuAsync(parentMenuModel);
                    if (res.ToString().Equals("1"))
                    {
                        TempData["success"] = "Parent Menu has been saved";
                    }
                    else
                    {
                        TempData["error"] = "Parent Menu has not been saved";
                    }
                    return RedirectToAction(nameof(Index));
                }
                return View(parentMenuModel);
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
                return View(await _parentMenuService.GetParentMenuByIdAsync(id));
            }
            else
            {
                return RedirectToAction("Login", "Home");

            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ParentMenuModel parentMenuModel)
        {
            try
            {
                if (HttpContext.Session.GetString("Name") != null)
                {
                    parentMenuModel.ModifiedBy = HttpContext.Session.GetString("Name");
                    parentMenuModel.UserId = (int)HttpContext.Session.GetInt32("UserId");
                    //countryModel.ModifiedDate = System.DateTime.Now;
                    if (ModelState.IsValid)
                    {
                        var dbMenu = await _parentMenuService.GetParentMenuByIdAsync(id);
                        if (await TryUpdateModelAsync<ParentMenuModel>(dbMenu))
                        {
                            parentMenuModel.ParentMenuId = (int)HttpContext.Session.GetInt32("Id");
                            var res = await _parentMenuService.UpdateParentMenuAsync(dbMenu);
                            if (res.ToString().Equals("1"))
                            {
                                TempData["success"] = "Parent Menu has been updated";
                            }
                            else
                            {
                                TempData["error"] = "Parent Menu has not been updated";
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
            return View(parentMenuModel);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {

            if (HttpContext.Session.GetString("Name") != null)
            {
                try
                {
                    var dbMenu = await _parentMenuService.GetParentMenuByIdAsync(id);
                    if (dbMenu != null)
                    {
                        var res = await _parentMenuService.DeleteParentMenuAsync(dbMenu);

                        if (res.ToString().Equals("1"))
                        {
                            TempData["error"] = "Parent Menu has been deleted";
                        }
                        else
                        {
                            TempData["error"] = "Parent Menu has not been deleted";
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
