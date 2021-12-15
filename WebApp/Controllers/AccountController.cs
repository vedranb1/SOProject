using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SOProject.ServiceLayer;
using StackOverflowProject.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace WebApp.Controllers
{
    public class AccountController : Controller
    {
        IUserService us;
        public AccountController(IUserService us)
        {
            this.us = us;
        }


        public IActionResult Register()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Register(RegisterViewModel rvm)
        {

                
                int uid = this.us.InsertUser(rvm);
                HttpContext.Session.SetInt32("CurrentUserID", uid);
                HttpContext.Session.SetString("CurrentEmail", rvm.Email);
                HttpContext.Session.SetString("CurrentUserPassword", rvm.Password);
                HttpContext.Session.SetString("CurrentUsername", rvm.Username);
                HttpContext.Session.SetString("CurrentUserIsAdmin", "false");
                return RedirectToAction("Login", "Account");
           

            
        }

        public IActionResult Login()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Login(LoginViewModel lvm)
        {
            if (ModelState.IsValid)
            {
                UserViewModel uvm = this.us.GetUsersByEmailAndPassword(lvm.Email, lvm.Password);
                if(uvm != null)
                {
                    HttpContext.Session.SetInt32("CurrentUserID", uvm.UserID);
                    HttpContext.Session.SetString("CurrentEmail", uvm.Email);
                    HttpContext.Session.SetString("CurrentUserPassword", uvm.Password);
                    HttpContext.Session.SetString("CurrentUsername", uvm.Username);
                    if(uvm.IsAdmin == true)
                    {
                        HttpContext.Session.SetString("CurrentUserIsAdmin", "true");
                        return RedirectToRoute(new { area = "admin", controller = "AdminHome", action = "Index" });
                    }
                    else if(HttpContext.Session.GetInt32("CurrentUserID") == uvm.UserID)
                    {
                        HttpContext.Session.SetString("CurrentUserIsAdmin", "false");
                        return RedirectToAction("Index", "Home");
                    }
                    
                }
                else
                {
                    ModelState.AddModelError("x", "Invalid email/password.");
                }
            }
            else
            {
                ModelState.AddModelError("x", "Invalid data.");
            }

            return View(lvm);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Home", "Index");
        }

        [Authorize]
        public IActionResult ChangeProfile()
        {
            int uid = Convert.ToInt32(HttpContext.Session.GetInt32("CurrentUserID"));
            UserViewModel uvm = this.us.GetUsersByUserID(uid);
            EditUserDetailsViewModel eudvm = new EditUserDetailsViewModel { Email = uvm.Email, Mobile = uvm.Mobile, Username = uvm.Username};
            return View(eudvm);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        
        public IActionResult ChangeProfile(EditUserDetailsViewModel eudvm)
        {
            if (ModelState.IsValid)
            {
                eudvm.UserID = Convert.ToInt32(HttpContext.Session.GetInt32("CurrentUserID"));
                this.us.UpdateUserDetails(eudvm);
                HttpContext.Session.SetString("CurrentUserMobile", eudvm.Mobile);
                HttpContext.Session.SetString("CurrentUserEmail", eudvm.Email);
                HttpContext.Session.SetString("CurrentUsername", eudvm.Username);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("x", "Invalid data.");
                return View(eudvm);
            }
        }

        
        public IActionResult ChangePassword()
        {
            int uid = Convert.ToInt32(HttpContext.Session.GetInt32("CurrentUserID"));
            UserViewModel uvm = this.us.GetUsersByUserID(uid);
            EditUserPasswordViewModel eupvm = new EditUserPasswordViewModel { Email = uvm.Email, Password = "", ConfirmPassword = "", UserID = uvm.UserID };
            return View(eupvm);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        
        public IActionResult ChangePassword(EditUserPasswordViewModel eupvm)
        {
            if(ModelState.IsValid)
            {
                eupvm.UserID = Convert.ToInt32(HttpContext.Session.GetInt32("CurrentUserID"));
                this.us.UpdateUserPassword(eupvm);
                HttpContext.Session.SetString("CurrentUserPassword", eupvm.Password);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("x", "Invalid data.");
                return View(eupvm);
            }
        }

    }
}
