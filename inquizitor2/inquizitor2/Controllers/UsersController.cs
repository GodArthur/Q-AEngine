using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using inquizitor2.Models; 
using System.Web.Security;
using System.Security.Cryptography;
using System.Runtime.InteropServices.WindowsRuntime;

namespace inquizitor2.Controllers
{
    public class UsersController : Controller
    {
        private dbqaEntities db = new dbqaEntities();


        public ActionResult Login(string ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string userName, string password, string ReturnUrl)
        {
            if (ModelState.IsValid)
            {
                var query = (from user in db.Users
                             where user.UserName == userName && user.Password == password
                             select user).SingleOrDefault();

                if (query == null)
                {
                    ModelState.AddModelError("", "Invalid username or password");
                    ViewBag.ReturnUrl = ReturnUrl;
                    return View();
                }
                FormsAuthentication.RedirectFromLoginPage(userName, false);
            }

            return View();
            
        }


        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Questions");
        }



        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserName,Password,LastName,FirstName,Email,Country")] User user)
        {
            if (ModelState.IsValid)
            {
                var query = (from u in db.Users
                             where u.UserName.Equals(user.UserName)
                             select u).FirstOrDefault();
                if (query != null)
                {
                    ModelState.AddModelError("", "This username already exists");
                    return View(User);
                }

                db.Users.Add(user);
                db.SaveChanges();
                FormsAuthentication.RedirectFromLoginPage(user.UserName, false);
            }
            else
           
                ModelState.AddModelError("", "This username already exists");
                return View(user);
            
        }

    

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
