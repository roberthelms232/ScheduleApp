using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScheduleApp.Controllers {
    public class RoomController : BaseController {
        public IActionResult Index() {
            List<Room> lst = DAL.GetRooms();
            return View(lst);
        }

        #region GETS
        //GET: Room/Details
        public ActionResult Details(int id) {
            Room rm = DAL.GetRoom(id);
            return View(rm);
        }

        public ActionResult Create() {
            return View();
        }

        public ActionResult Edit(int id) {
            Room rm = DAL.GetRoom(id);
            return View(rm);
        }
        #endregion

        #region POSTS
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Room rm) {
            if(rm.RoomNum != null && rm.RoomNum > 0) {
                rm.dbSave();
                return RedirectToAction("Index");
            } else {
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Room rm) {
            try {
                Room oriRoom = DAL.GetRoom(rm.ID);
                rm.dbSave();
                return RedirectToAction("Index");
            } catch {
                //Fail message goes here
                return View();
            }
        }
        #endregion
    }
}
