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

namespace EmployeeWorkTime.Controllers
{
    public class WorkTimesController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

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
            return View();
        }

        // POST: WorkTimes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ClockIn,ClockOut,WorkDate")] WorkTime workTime)
        {
            if (ModelState.IsValid)
            {
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
            return View(workTime);
        }

        // POST: WorkTimes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ClockIn,ClockOut,WorkDate")] WorkTime workTime)
        {
            if (ModelState.IsValid)
            {
                db.Entry(workTime).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(workTime);
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
            WorkTime today;

            if (db.WorkTimes.Any(e => e.WorkDate == DateTime.Today))
                today = db.WorkTimes.First(e => e.WorkDate == DateTime.Today);
            else
            {
                today = new WorkTime();
                today.WorkDate = DateTime.Today;
                db.WorkTimes.Add(today);
                db.SaveChanges();
            }

            return today;
        }
    }
}
