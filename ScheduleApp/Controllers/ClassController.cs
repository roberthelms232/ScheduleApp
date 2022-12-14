using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScheduleApp.Controllers {
    public class ClassController : BaseController {
        public IActionResult Index() {
            List<Class> lst = DAL.GetClasses();
            return View(lst);
        }

        #region GETS
        public ActionResult Details(int id) {
            Class cls = DAL.GetClass(id);
            return View(cls);
        }

        public ActionResult Create() {
            return View();
        }
        public ActionResult Edit(int id) {
            Class cls = DAL.GetClass(id);
            return View(cls);
        }
        #endregion

        #region POSTS
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Class cls) {
            if(cls.Name != null && cls.Name != "") {
                try {
                    cls.dbSave();
                    return RedirectToAction("Index");
                } catch {
                    //Fail Message
                    return View();
                }
            } else {
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Class cls) {
            try {
                Class oriClass = DAL.GetClass(cls.ID);
                cls.dbSave();
                return RedirectToAction("Index");
            } catch {
                return View();
            }
        }
        #endregion


    }
}
