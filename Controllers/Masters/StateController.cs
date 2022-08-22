using CoreLayout.Models.Masters;
using CoreLayout.Services.Common;
using CoreLayout.Services.Masters.Country;
using CoreLayout.Services.Masters.State;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Extensions;
using CoreLayout.Models.Common;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;

namespace CoreLayout.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class StateController : Controller
    {
        private readonly ILogger<StateController> _logger;
        private readonly IStateService _stateService;
        private readonly ICountryService _countryService;
        private readonly IDataProtector _protector;
        private readonly ICommonService _commonService;
        private IHostingEnvironment _env;
        public StateController(ILogger<StateController> logger, IStateService stateService, ICountryService countryService, IDataProtectionProvider provider, ICommonService commonService, IHostingEnvironment env)
        {
            _logger = logger;
            _stateService = stateService;
            _countryService = countryService;
            _protector = provider.CreateProtector("State.StateController");
            _commonService = commonService;
            _env = env;
        }

        public int CheckPagePermission(int roleid, int userid)
        {
            int result = 0;
            try
            {
                var getPagePermission = _commonService.GetDashboardByRoleAndUser(roleid, userid).Result;
                foreach (var _getPagePermission in getPagePermission)
                {
                    var Controller = _getPagePermission.Controller;
                    var Action = _getPagePermission.Action;
                    var Url = Controller + "/" + Action;
                    var enterUrl = @HttpContext.Request.GetDisplayUrl();
                    //var enterUrl = Directory.GetCurrentDirectory();
                    //var enterUrl = _env.WebRootPath;
                   //string x=  Url.Action("action", "controller");
                    enterUrl = enterUrl.Remove(0, 23);//for local host
                    if (Url == enterUrl)
                    {
                        result = 1;
                    }
                }
            }
            catch (Exception ex)
            {
                result = 2;
            }
            return result;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                if (HttpContext.Session.GetInt32("UserId") != null && HttpContext.Session.GetInt32("RoleId") != null)
                {
                    //check page permission
                    int userid = (int)HttpContext.Session.GetInt32("UserId");
                    int roleid = (int)HttpContext.Session.GetInt32("RoleId");
                    var checkPagePermission = CheckPagePermission(roleid, userid);

                    //check button permission
                    var buttonPermission = await _commonService.GetButtonByRoleAndUser(roleid, userid);
                    ViewBag.ButtonPermission = buttonPermission;
                    foreach (var item in buttonPermission)
                    {
                        //add code for button hide and show
                    }


                    if (checkPagePermission == 1)
                    {
                        var state = await _stateService.GetAllState();
                        foreach (var _state in state)
                        {
                            var stringId = _state.StateId.ToString();
                            _state.EncryptedId = _protector.Protect(stringId);
                        }

                        int maxstateid = 0;
                        foreach (var _states in state)
                        {
                            maxstateid = _states.StateId;
                        }
                        maxstateid = maxstateid + 1;
                        ViewBag.MaxStateId = _protector.Protect(maxstateid.ToString());
                        return View(state);
                    }
                    else
                    {
                        return RedirectToAction("Login", "Home");
                    }
                }
                else
                {
                    return RedirectToAction("Login", "Home");
                }
            }

            catch (Exception ex)
            {
                //ModelState.AddModelError("", ex.ToString());
                TempData["error"] = "Not Permission";
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(string id)
        {
            try
            {
                if (HttpContext.Session.GetInt32("UserId") != null && HttpContext.Session.GetInt32("RoleId") != null)
                {
                    int userid = (int)HttpContext.Session.GetInt32("UserId");
                    int roleid = (int)HttpContext.Session.GetInt32("RoleId");
                    if (roleid != 0 && userid != 0)
                    {
                        var guid_id = _protector.Unprotect(id);
                        var employee = await _stateService.GetStateById(Convert.ToInt32(guid_id));
                        return View(employee);
                    }
                }
                else
                {
                    return RedirectToAction("Login", "Home");
                }
            }
            catch (Exception ex)
            {
                //ModelState.AddModelError("", ex.ToString());
                TempData["error"] = "Not Permission";
            }
            return RedirectToAction(nameof(Index));
        }

        //Create Get Action Method
        public ActionResult Create(string id)
        {
            try
            {
                if (HttpContext.Session.GetInt32("UserId") != null && HttpContext.Session.GetInt32("RoleId") != null)
                {
                    int userid = (int)HttpContext.Session.GetInt32("UserId");
                    int roleid = (int)HttpContext.Session.GetInt32("RoleId");
                    if (roleid != 0 && userid != 0)
                    {
                        var countryList = (from country in _countryService.GetAllCountry().Result
                                           select new SelectListItem()
                                           {
                                               Text = country.CountryName,
                                               Value = country.CountryId.ToString(),
                                           }).ToList();

                        countryList.Insert(0, new SelectListItem()
                        {
                            Text = "----Select----",
                            Value = string.Empty
                        });

                        var guid_id = _protector.Unprotect(id);
                        ViewBag.CountryList = countryList;//roleList.Select(l => l.CountryId).ToList();
                        return View();
                    }
                }
                else
                {
                    return RedirectToAction("Login", "Home");
                }
            }
            catch (Exception ex)
            {
                //ModelState.AddModelError("", ex.ToString());
                TempData["error"] = "Not Permission";
            }
            return RedirectToAction(nameof(Index));
        }

        //Create Post Action Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StateModel stateModel)
        {
            if (HttpContext.Session.GetString("Name") != null)
            {
                stateModel.CreatedBy = HttpContext.Session.GetString("Name");
                if (ModelState.IsValid)
                {
                    var res = await _stateService.CreateStateAsync(stateModel);
                    if (res.ToString().Equals("1"))
                    {
                        TempData["success"] = "State has been saved";
                    }
                    else
                    {
                        TempData["error"] = "State has not been saved";
                    }
                    return RedirectToAction(nameof(Index));
                }
                return View(stateModel);
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }
        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                if (HttpContext.Session.GetInt32("UserId") != null && HttpContext.Session.GetInt32("RoleId") != null)
                {
                    int userid = (int)HttpContext.Session.GetInt32("UserId");
                    int roleid = (int)HttpContext.Session.GetInt32("RoleId");
                    if (roleid != 0 && userid != 0)
                    {

                        var countryList = (from country in _countryService.GetAllCountry().Result
                                           select new SelectListItem()
                                           {
                                               Text = country.CountryName,
                                               Value = country.CountryId.ToString(),
                                           }).ToList();

                        countryList.Insert(0, new SelectListItem()
                        {
                            Text = "----Select----",
                            Value = string.Empty
                        });

                        var guid_id = _protector.Unprotect(id);
                        var employee = await _stateService.GetStateById(Convert.ToInt32(guid_id));

                        ViewBag.CountryList = countryList;
                        return View(employee);

                    }
                }
                else
                {
                    return RedirectToAction("Login", "Home");
                }
            }
            catch (Exception ex)
            {
                //ModelState.AddModelError("", ex.ToString());
                TempData["error"] = "Not Permission";
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int StateId, StateModel stateModel)
        {
            try
            {
                if (HttpContext.Session.GetString("Name") != null)
                {
                    stateModel.ModifiedBy = HttpContext.Session.GetString("Name");
                    if (ModelState.IsValid)
                    {
                        var dbState = await _stateService.GetStateById(StateId);
                        if (await TryUpdateModelAsync<StateModel>(dbState))
                        {
                            stateModel.StateId = (int)HttpContext.Session.GetInt32("Id");
                            stateModel.CountryId = Convert.ToInt32(Request.Form["CountryId"]);//get country id from form collection
                            var res = await _stateService.UpdateStateAsync(dbState);
                            if (res.ToString().Equals("1"))
                            {
                                TempData["success"] = "State has been updated";
                            }
                            else
                            {
                                TempData["error"] = "State has not been updated";
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
            return View(stateModel);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {

            try
            {
                if (HttpContext.Session.GetInt32("UserId") != null && HttpContext.Session.GetInt32("RoleId") != null)
                {
                    int userid = (int)HttpContext.Session.GetInt32("UserId");
                    int roleid = (int)HttpContext.Session.GetInt32("RoleId");
                    if (roleid != 0 && userid != 0)
                    {
                        var guid_id = _protector.Unprotect(id);
                        var dbState = await _stateService.GetStateById(Convert.ToInt32(guid_id));
                        if (dbState != null)
                        {
                            var res = await _stateService.DeleteStateAsync(dbState);
                            if (res.ToString().Equals("1"))
                            {
                                TempData["success"] = "State has been deleted";
                            }
                            else
                            {
                                TempData["error"] = "State has not been deleted";
                            }
                        }
                        else
                        {
                            TempData["error"] = "Some thing went wrong!";
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
                TempData["error"] = "Not Permission!";
            }

            return RedirectToAction(nameof(Index));

        }
    }
}