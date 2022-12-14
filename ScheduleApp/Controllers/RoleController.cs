using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScheduleApp.Controllers {
    public class RoleController : BaseController {
        public IActionResult Index() {
            List<Role> lst = DAL.GetRoles();
            return View(lst);
        }
    }

}
