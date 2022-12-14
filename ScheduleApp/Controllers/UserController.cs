using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;

namespace ScheduleApp.Controllers {
    public class UserController : BaseController {
        //GET: UserController
        public IActionResult Index() {
            //Should create a list of Users to display on the User Index
            List<User> lst = DAL.GetUsers();
            return View(lst);
        }
        #region GET
        //GET: UserController/Details
        public ActionResult Details(int id) {
            //Uses the User ID to pull the details of the user
            User usr = DAL.GetUser(id);
            //int usr = 1; //hardcoded for potential testing; To Be Deleted
            return View(usr);
        }

        //GET: UserController/Create
        public ActionResult Create(int? roleid) {
            User newUser = new User();

            if(roleid != null) {
                newUser.Role = DAL.GetRole((int)roleid);
            }
            return View();
        }

        //GET: UserController/Edit
        public ActionResult Edit(int id) {
            User usr = DAL.GetUser(id);
            //int usr = id; //For testing
            return View(usr);
        }

        //GET: UserController/Delete
        //Will be unimplemented until the very end
        #endregion

        #region POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(User usr) {
            if(usr.Password != null && usr.Password != "") {
                if(usr.Password == usr.ConfirmPassword) {
                    try {
                        //space for potential salt/hash
                        int usrID = usr.dbSave();

                        List<User> users = DAL.GetUsers();

                        return RedirectToAction("Index");
                    }catch {
                        //Error creating user -- Needs Error Message
                        return View();
                    }
                } else {
                    //Passwords do not match -- needs error message
                    return View();
                }
            } else {
                //Password was not specified -- needs error message
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, User usr) {

            try {
                User oldUser = DAL.GetUser(usr.ID);
                string password = usr.Password;

                int usrID = usr.dbSave();
                return RedirectToAction("Index");
            } catch {
                //Error with editing -- Error Messages To be Implemented at future date
                return View();
            }
        }
        #endregion


        #region Login

        public IActionResult Login() {
            if(CurrentUser.ID > 0) {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password) {
            //Checks the email and password against users saved in the database
            if(email != null && password != null) {
                User usr = DAL.GetUser(email, password);
                if(usr != null) {
                    LogUserIn(usr);
                    return RedirectToAction("Index", "Home");
                } else {
                    return View();
                }
            } else {
                //Failed
            }
            return RedirectToAction("Login", "User");
        }
        //Creates the session and assigns the cookie to the user logging in
        private void LogUserIn(User usr) {
            HttpContext.Session.SetInt32("UserID", usr.ID);
            Response.Cookies.Append("user", DAL.GetCookie(usr));
        }

        public ActionResult Logout() {
            Response.Cookies.Delete("user");
            return RedirectToAction("Index", "Home");
        }
        #endregion
    }
}
