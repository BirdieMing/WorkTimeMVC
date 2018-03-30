using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EmployeeWorkTime.DAL;
using EmployeeWorkTime.Models;
using EmployeeWorkTime.Util;

namespace EmployeeWorkTime.Controllers
{
    public class WorkTimesController : Controller
    {
        private DatabaseContext db = new DatabaseContext();
        private SessionContext s = new SessionContext();

        [ChildActionOnly]
        public ActionResult WorkTimesNavbar()
        {
            Employee user = s.GetUserData();
            return PartialView("_NavbarPartial", user);
        }

        // GET: WorkTimes
        public ActionResult Index()
        {
            return View(db.WorkTimes.ToList());
        }

        // GET: WorkTimes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WorkTime workTime = db.WorkTimes.Find(id);
            if (workTime == null)
            {
                return HttpNotFound();
            }
            return View(workTime);
        }

        // GET: WorkTimes/Create
        public ActionResult Create()
        {
            ViewBag.Users = new SelectList(db.Users.ToList(), "Id", "UserName");
            return View();
        }

        // POST: WorkTimes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ClockIn,ClockOut,WorkDate,EmployeeID")] WorkTime workTime)
        {
            if (ModelState.IsValid)
            {
                ValidateModel(workTime);
                db.WorkTimes.Add(workTime);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(workTime);
        }

        // GET: WorkTimes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WorkTime workTime = db.WorkTimes.Find(id);
            if (workTime == null)
            {
                return HttpNotFound();
            }

            ViewBag.Users = new SelectList(db.Users.ToList(), "Id", "UserName");
            return View(workTime);
        }

        // POST: WorkTimes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ClockIn,ClockOut,WorkDate,EmployeeID")] WorkTime workTime)
        {
            if (ModelState.IsValid)
            {
                ValidateModel(workTime);
                db.Entry(workTime).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Users = new SelectList(db.Users.ToList(), "Id", "UserName");
            return View(workTime);
        }

        private void ValidateModel(WorkTime workTime)
        {
            if (workTime.ClockIn.HasValue)
            {
                workTime.ClockIn = new DateTime(workTime.WorkDate.Year, workTime.WorkDate.Month, workTime.WorkDate.Day, workTime.ClockIn.Value.Hour, workTime.ClockIn.Value.Second, 0);
            }

            if (workTime.ClockOut.HasValue)
            {
                workTime.ClockOut = new DateTime(workTime.WorkDate.Year, workTime.WorkDate.Month, workTime.WorkDate.Day, workTime.ClockOut.Value.Hour, workTime.ClockOut.Value.Second, 0);
            }
        }

        // GET: WorkTimes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WorkTime workTime = db.WorkTimes.Find(id);
            if (workTime == null)
            {
                return HttpNotFound();
            }
            return View(workTime);
        }

        // POST: WorkTimes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            WorkTime workTime = db.WorkTimes.Find(id);
            db.WorkTimes.Remove(workTime);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult Clock()
        {
            Employee e = s.GetUserData();

            if (e == null)
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);

            WorkTime today = GetTodayWorkTime();
            return View(today);
        }

        [HttpPost]
        public ActionResult Clock(WorkTime w, string submit)
        {
            switch (submit)
            {
                case "Clock In":
                    return ClockIn();
                case "Clock Out":
                    return ClockOut();
                default:
                    return Clock();
            }
        }

        public ActionResult ClockOut()
        {
            WorkTime today = GetTodayWorkTime();
            if (!today.ClockIn.HasValue)
            {
                ModelState.AddModelError("ClockOut", "Must Clock In First.");
            } else
            if (!today.ClockOut.HasValue)
            {
                today.ClockOut = DateTime.Now;
                db.Entry(today).State = EntityState.Modified;
                db.SaveChanges();
            }
            else
            {
                ModelState.AddModelError("ClockOut", "Already Clocked Out Today.");
            }

            return View(today);
        }

        public ActionResult ClockIn()
        {
            WorkTime today = GetTodayWorkTime();
            if (!today.ClockIn.HasValue)
            {
                today.ClockIn = DateTime.Now;
                db.Entry(today).State = EntityState.Modified;
                db.SaveChanges();
            }
            else
            { 
                ModelState.AddModelError("ClockIn", "Already Clocked In Today.");
            }

            return View(today);
        }

        private WorkTime GetTodayWorkTime()
        {
            SessionContext c = new SessionContext();
            Employee employee = c.GetUserData(); 
            WorkTime today;

            if (db.WorkTimes.Any(e => e.WorkDate == DateTime.Today))
                today = db.WorkTimes.First(e => e.WorkDate == DateTime.Today);
            else
            {
                today = new WorkTime();
                today.WorkDate = DateTime.Today;
                today.EmployeeID = employee.Id;
                db.WorkTimes.Add(today);
                db.SaveChanges();
            }

            return today;
        }
    }
}
