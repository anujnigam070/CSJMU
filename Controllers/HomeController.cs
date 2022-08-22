﻿using CoreLayout.Models.Common;
using CoreLayout.Models.UserManagement;
using CoreLayout.Services.Registration;
using CoreLayout.Services.UserManagement.Login;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayout.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ILoginService _userService;
        private readonly IRegistrationService _registrationService;
        private static string errormsg = "";
        public HomeController(ILogger<HomeController> logger, ILoginService userService, IRegistrationService registrationService)
        {
            _logger = logger;
            _userService = userService;
            _registrationService = registrationService;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string ReturnURL)
        {
            HttpContext.Session.Clear();
            if (errormsg != "")
            {
                ViewBag.errormsg = errormsg;
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginModel loginModel)
        {
                try
                {
                    if (ModelState.IsValid)
                    {
                        if (loginModel.LoginID != null && loginModel.Password != null)
                        {
                            var getuser = _userService.GetUserDetailByLoginId(loginModel.LoginID);
                            if (getuser.Result != null)
                            {
                                if (String.Compare(ComputeSaltedHash(loginModel.Password, getuser.Result.Salt), getuser.Result.SaltedHash) == 0)
                                {
                                    if (getuser.Result.IsUserActive == 1 && getuser.Result.IsRoleActive==1)
                                    {
                                        ClaimsIdentity identity = null;
                                        bool isAuthenticated = false;

                                        //Create the identity for the user  
                                        identity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, getuser.Result.LoginID), new Claim(ClaimTypes.Role, getuser.Result.RoleName) }, CookieAuthenticationDefaults.AuthenticationScheme);
                                        isAuthenticated = true;
                                        if (isAuthenticated)
                                        {
                                            var principal = new ClaimsPrincipal(identity);
                                            var login = HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                                            //_userService.InsertLoginInformation(user.Result.UserID, "Login Successfully");
                                            //_logger.LogInformation("Log message in the Index() method", login);//log is not working
                                            HttpContext.Session.SetString("Name", getuser.Result.UserName);
                                            HttpContext.Session.SetInt32("Id", getuser.Result.UserID);
                                            HttpContext.Session.SetInt32("UserId", getuser.Result.UserID);
                                            HttpContext.Session.SetInt32("RoleId", getuser.Result.RoleId);
                                        return RedirectToAction("Index", "Dashboard");
                                    }
                                    }
                                    else
                                    {
                                        ModelState.AddModelError("", "User is not active");
                                        return View();
                                    }
                                }
                            }
                            else
                            {
                                ModelState.AddModelError("", "Invalid UserName or Password");
                                return View();
                            }

                        }
                        else
                        {
                            ModelState.AddModelError("", "Enter UserName or Password");
                            return View();
                        }
                    }
                    else
                    {
                        errormsg = "Model state is invalid!";
                        return RedirectToAction("Login", "Home");
                    }
                }
                catch (Exception ex)
                {
                    errormsg = ex.StackTrace.ToString();
                    return RedirectToAction("Login", "Home");
                }
            errormsg = "some thing went wrong!";
            return RedirectToAction("Login", "Home");
        }

        [HttpGet]
        public IActionResult Registration()
        {
            if (errormsg != "")
            {
                ViewBag.errormsg = errormsg;
            }
            return View();
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registration(RegistrationModel registrationModel)
        {
            try
            {
                string salt = CreateSalt();
                string saltedHash = ComputeSaltedHash(registrationModel.Password, salt);
                registrationModel.Salt = salt;
                registrationModel.SaltedHash = saltedHash;
                var remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress;
                registrationModel.IPAddress = remoteIpAddress.ToString();

                if (ModelState.IsValid)
                {
                    //registrationModel.Id = (int)HttpContext.Session.GetInt32("Id");
                    var res = await _registrationService.CreateRegistrationAsync(registrationModel);
                    if (res.ToString().Equals("-1"))
                    {
                        //success
                        errormsg = "Successfully Saved.";

                    }
                    else {
                        //falure
                        errormsg = "Data Not Saved!";
                    }
                    return RedirectToAction(nameof(Registration));
                }
                else
                {
                    errormsg = "Model state is invalid!";
                    return RedirectToAction("Registration", "Home");
                }
            }
            catch (Exception ex)
            {
                errormsg = ex.StackTrace.ToString();
                return RedirectToAction("Registration", "Home");
            }
        }
        public IActionResult Forget()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Logout()
        {
           // int id = (int)HttpContext.Session.GetInt32("Id");
           // _userService.InsertLogoutInformation(id, "Logout Successfully");
            var login = HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}
