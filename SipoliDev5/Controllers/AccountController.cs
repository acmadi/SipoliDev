using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SipoliDev5.Models;
using WebMatrix.WebData;
using System.Web.Security;

namespace SipoliDev5.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Login()
        {
            return View("Login","_LoginMaster");
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Login logindata, string ReturnUrl)
        {
            if (ModelState.IsValid)
            {
                if (WebSecurity.Login(logindata.Username, logindata.Password))
                {
                    if (ReturnUrl != null)
                    {
                        return Redirect(ReturnUrl);
                    }
                    return RedirectToAction("Index", "Home");

                }
                else
                {
                    ModelState.AddModelError("", "Sorry, Username or Password is invalid.");
                    return View(logindata);
                }

            }
            ModelState.AddModelError("", "Sorry, Username or Password is invalid.");
            return View(logindata);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [Authorize(Roles="Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Register registerdata, string role)
        {
            if (ModelState.IsValid)
            {
                if (!Roles.RoleExists(role))
                {
                    Roles.CreateRole(role);
                }

                try
                {
                    WebSecurity.CreateUserAndAccount(registerdata.Username, registerdata.Password);
                    Roles.AddUserToRole(registerdata.Username, role);
                    return RedirectToAction("Index", "Home");
                }
                catch (MembershipCreateUserException exception)
                {
                    ModelState.AddModelError("", "Sorry, username already exists");
                    return View(registerdata);
                }
            }
            ModelState.AddModelError("", "Sorry, username already exists");
            return View(registerdata);
        }

        public ActionResult Logout()
        {
            WebSecurity.Logout();
            return RedirectToAction("Index", "Home");
        }
    }
}