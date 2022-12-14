using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;

namespace ScheduleApp.Controllers {
    public class BaseController : Controller {
        //Defines the CurrentUser
        private User _CurrentUser = null;

        public BaseController() { }

        public BaseController(Microsoft.AspNetCore.Http.IHttpContextAccessor accessor) { }

        //Creates the CurrentUser for permissions and cookie tracking
        public User CurrentUser {
            get {
                if(_CurrentUser != null) {
                    return _CurrentUser;
                } else {
                    Role anonRole = new Role() {
                        ID = -99,
                        Name = "Anonymous"
                    };
                    User anonUser = new User() {
                        ID = -99,
                        FirstName = "Anony",
                        LastName = "Mous",
                        Role = anonRole
                    };
                    return anonUser;
                }
            }
        }
        //Used to help assign cookies to the Current User
        public override void OnActionExecuting(ActionExecutingContext context) {
            base.OnActionExecuting(context);

            string userID = Request.Cookies["user"];

            int uID = 0;
            int.TryParse(userID, out uID);
            User usr = DAL.GetUser(uID);
            _CurrentUser = usr;
            ViewBag.CurrentUser = _CurrentUser;
        }


    }
}
