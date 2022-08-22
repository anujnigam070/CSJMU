using CoreLayout.Models.Masters;
using CoreLayout.Models.UserManagement;
using CoreLayout.Services.Masters.Role;
using CoreLayout.Services.Registration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayout.Controllers.UserManagement
{
    [Authorize(Roles = "Administrator")]
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly IRegistrationService _registrationService;
        private readonly IRoleService _roleService;
        private static string errormsg = "";

        public UserController(ILogger<UserController> logger,  IRegistrationService registrationService, IRoleService roleService)
        {
            _roleService = roleService;
            _logger = logger;
            _registrationService = registrationService;
        }



        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (errormsg != "")
            {
                ViewBag.errormsg = errormsg;
            }
            return View(await _registrationService.GetAllRegistrationAsync());
        }

        public static string CreateSalt()
        {
            // Define salt sizes
            int minSaltSize = 4;
            int maxSaltSize = 8;
            byte[] saltBytes;

            // Generate a random number for the size of the salt.
            Random random = new Random();
            int saltSize = random.Next(minSaltSize, maxSaltSize);
            saltBytes = new byte[saltSize];
            // Initialize a random number generator.
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            // Fill the salt with cryptographically strong byte values.
            rng.GetNonZeroBytes(saltBytes);
            return Convert.ToBase64String(saltBytes);
        }
        public static string ComputeSaltedHash(string password, string salt)
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException("password");
            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException("salt");
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            byte[] saltBytes = Encoding.UTF8.GetBytes(salt);

            HashAlgorithm hash = new SHA1Managed();

            List<byte> passwordWithSaltBytes = new List<byte>(passwordBytes);
            passwordWithSaltBytes.AddRange(saltBytes);

            byte[] hashBytes = hash.ComputeHash(passwordWithSaltBytes.ToArray());

            return Convert.ToBase64String(hashBytes);
        }

        //Create Get Action Method
        public ActionResult Create()
        {
            //var RoleList = (from role in _roleService.GetAllRoleAsync().Result
            //                   select new SelectListItem()
            //                   {
            //                       Text = role.RoleName,
            //                       Value = role.RoleID.ToString(),
            //                   }).ToList();
            //ViewBag.RoleList = RoleList;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RegistrationModel registrationModel)
        {
            try
            {
                if (HttpContext.Session.GetString("Name") != null)
                {
                    registrationModel.CreatedBy = HttpContext.Session.GetInt32("UserId").ToString();
                    //registrationModel.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

                    string salt = CreateSalt();
                    string saltedHash = ComputeSaltedHash(registrationModel.Password, salt);
                    registrationModel.Salt = salt;
                    registrationModel.SaltedHash = saltedHash;

                    if (ModelState.IsValid)
                    {
                        //registrationModel.Id = (int)HttpContext.Session.GetInt32("Id");
                        var res = await _registrationService.CreateRegistrationAsync(registrationModel);
                        if (res.ToString().Equals("1"))
                        {
                            TempData["success"] = "User has been created";
                        }
                        else
                        {
                            TempData["error"] = "User has not been created";
                        }
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        TempData["error"] = "Some thing went wrong!";
                        return RedirectToAction(nameof(Index));
                    }
                }
                else
                {
                    TempData["error"] = "Session is null!";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                errormsg = ex.StackTrace.ToString();
                TempData["error"] = errormsg;
                return RedirectToAction(nameof(Index));
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (HttpContext.Session.GetString("Name") != null)
            {
                return View(await _roleService.GetRoleByIdAsync(id));
            }
            else
            {
                return RedirectToAction("Login", "Home");

            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, RegistrationModel registrationModel)
        {
            try
            {
                if (HttpContext.Session.GetString("Name") != null)
                {
                    registrationModel.ModifiedBy = HttpContext.Session.GetString("Name");
                    registrationModel.RoleName = id.ToString();
                    if (ModelState.IsValid)
                    {
                        var dbRole = await _roleService.GetRoleByIdAsync(id);
                        if (await TryUpdateModelAsync<RoleModel>(dbRole))
                        {
                            registrationModel.UserID = (int)HttpContext.Session.GetInt32("Id");
                            var res = await _registrationService.UpdateRegistrationAsync(registrationModel);
                            if (res.ToString().Equals("1"))
                            {
                                TempData["success"] = "User has been updated";
                            }
                            else
                            {
                                TempData["error"] = "User has not been updated";
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
            return View(registrationModel);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {

            if (HttpContext.Session.GetString("Name") != null)
            {
                try
                {
                    var dbRole = await _registrationService.GetRegistrationByIdAsync(id);
                    if (dbRole != null)
                    {
                        var res = await _registrationService.DeleteRegistrationAsync(dbRole);

                        if (res.ToString().Equals("1"))
                        {
                            TempData["error"] = "User has been deleted";
                        }
                        else
                        {
                            TempData["error"] = "User has not been deleted";
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
