using CoreLayout.Models.Masters;
using CoreLayout.Services.Masters.Country;
using CoreLayout.Services.UserManagement.ButtonPermission;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Controllers
{
    public class CountryController : Controller
    {
        private readonly ILogger<CountryController> _logger;
        private readonly ICountryService _countryService;
        private readonly IButtonPermissionService _buttonPermissionService;
        public CountryController(ILogger<CountryController> logger, ICountryService countryService, IButtonPermissionService buttonPermissionService)
        {
            _logger = logger;
            _countryService = countryService;
            _buttonPermissionService = buttonPermissionService;
        }
        public async Task<IActionResult> Index()
        {
            //var verifyUser = (from user in _buttonPermissionService.GetAllUsersAsync().Result
            //                         where user.Id == Convert.ToInt32(HttpContext.Session.GetString("Id"))
            //                         select new SelectListItem()
            //                         {
            //                             Text = user.UserId,
            //                             Value = user.FirstName.ToString(),
            //                         }).ToList();
            //if (verifyUser.Count > 0)
            //{
                //var btnPermissionList = (from user in _buttonPermissionService.GetAllButtonPermissionAsync().Result
                //                         where user.Registration_Id == Convert.ToInt32(HttpContext.Session.GetString("Id"))
                //                         select user);
                //ViewBag.btnPermissionList = btnPermissionList;

            //}
            return View(await _countryService.GetAllCountry());
        }

        public async Task<IActionResult> Details(int id)
        {
            if (HttpContext.Session.GetString("Name") != null)
            {
                return View(await _countryService.GetCountryById(id));
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
        public async Task<IActionResult> Create(CountryModel countryModel)
        {
            if (HttpContext.Session.GetString("Name") != null)
            {
                countryModel.CreatedBy = HttpContext.Session.GetString("Name");
                if (ModelState.IsValid)
                {
                    var res = await _countryService.CreateCountryAsync(countryModel);
                    if (res.ToString().Equals("1"))
                    {
                        TempData["success"] = "Country has been saved";
                    }
                    else
                    {
                        TempData["error"] = "Country has not been saved";
                    }
                    return RedirectToAction(nameof(Index));
                }
                return View(countryModel);
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
                return View(await _countryService.GetCountryById(id));
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int Id, CountryModel countryModel)
        {
            try
            {
                if (HttpContext.Session.GetString("Name") != null)
                {
                    countryModel.ModifiedBy = HttpContext.Session.GetString("Name");
                    if (ModelState.IsValid)
                    {
                        var dbCountry = await _countryService.GetCountryById(Id);
                        if (await TryUpdateModelAsync<CountryModel>(dbCountry))
                        {
                            countryModel.CountryId = (int)HttpContext.Session.GetInt32("Id");
                            var res = await _countryService.UpdateCountryAsync(dbCountry);
                            if (res.ToString().Equals("1"))
                            {
                                TempData["success"] = "Country has been updated";
                            }
                            else
                            {
                                TempData["error"] = "Country has not been updated";
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
            return View(countryModel);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {

            if (HttpContext.Session.GetString("Name") != null)
            {
                try
                {
                    var dbCountry = await _countryService.GetCountryById(id);
                    if (dbCountry != null)
                    {
                        var res = await _countryService.DeleteCountryAsync(dbCountry);
                         
                        if (res.ToString().Equals("1"))
                        {
                            TempData["success"] = "Country has been deleted";
                        }
                        else
                        {
                            TempData["error"] = "Country has not been deleted";
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
