using CoreLayout.Models.Masters;
using CoreLayout.Services.Masters.Country;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Controllers.Masters
{
    public class ButtonController : Controller
    {
        private readonly ILogger<ButtonController> _logger;
        private readonly IButtonService _buttonService;
        public ButtonController(ILogger<ButtonController> logger, IButtonService buttonService)
        {
            _buttonService = buttonService;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _buttonService.GetAllButton());
        }

        public async Task<IActionResult> Details(int id)
        {
            if (HttpContext.Session.GetString("Name") != null)
            {
                return View(await _buttonService.GetButtonById(id));
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
        public async Task<IActionResult> Create(ButtonModel buttonModel)
        {
            if (HttpContext.Session.GetString("Name") != null)
            {
                buttonModel.CreatedBy = HttpContext.Session.GetString("Name");
                if (ModelState.IsValid)
                {
                    var res = await _buttonService.CreateButtonAsync(buttonModel);
                    if (res.ToString().Equals("1"))
                    {
                        TempData["success"] = "Button has been saved";
                    }
                    else
                    {
                        TempData["error"] = "Button has not been saved";
                    }
                    return RedirectToAction(nameof(Index));
                }
                return View(buttonModel);
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
                return View(await _buttonService.GetButtonById(id));
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int Id, ButtonModel buttonModel)
        {
            try
            {
                if (HttpContext.Session.GetString("Name") != null)
                {
                    buttonModel.ModifiedBy = HttpContext.Session.GetString("Name");
                    if (ModelState.IsValid)
                    {
                        var dbCountry = await _buttonService.GetButtonById(Id);
                        if (await TryUpdateModelAsync<ButtonModel>(dbCountry))
                        {
                            buttonModel.ButtonId = (int)HttpContext.Session.GetInt32("Id");
                            var res = await _buttonService.UpdateButtonAsync(dbCountry);
                            if (res.ToString().Equals("1"))
                            {
                                TempData["success"] = "Button has been updated";
                            }
                            else
                            {
                                TempData["error"] = "Button has not been updated";
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
            return View(buttonModel);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {

            if (HttpContext.Session.GetString("Name") != null)
            {
                try
                {
                    var dbbutton = await _buttonService.GetButtonById(id);
                    if (dbbutton != null)
                    {
                        var res = await _buttonService.DeleteButtonAsync(dbbutton);

                        if (res.ToString().Equals("1"))
                        {
                            TempData["success"] = "Button has been deleted";
                        }
                        else
                        {
                            TempData["error"] = "Button has not been deleted";
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
