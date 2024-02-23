using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using marshal_deploy.Models;

namespace marshal_deploy.Controllers
{
    public class PrecinctPerformancesController : Controller
    {
        private Deploy db = new Deploy();

        // GET: PrecinctPerformances
        public ActionResult Index()
        {
            var precinctPerformances = db.PrecinctPerformances.Include(p => p.Period).Include(p => p.Precinct).Include(p => p.Shift);
            return View(precinctPerformances.ToList());
        }

        // GET: PrecinctPerformances/Details/5
        public ActionResult Details()
        {
            var precinctPerformances = db.PrecinctPerformances.ToList();

            return View(precinctPerformances);
        }

        // GET: PrecinctPerformances/Create
        public ActionResult Create()
        {
            ViewBag.PeriodId = new SelectList(db.Periods, "id", "PeriodType");
            ViewBag.PrecinctId = new SelectList(db.Precincts, "id", "PrecinctName");
            ViewBag.ClusterId = new SelectList(db.Precincts, "id", "ClusterId");
            ViewBag.ZoneId = new SelectList(db.Precincts, "id", "ZoneId");
            ViewBag.ShiftId = new SelectList(db.Shifts, "id", "id");
            return View();
        }

        // POST: PrecinctPerformances/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,ShiftId,PrecinctId,ClusterId,ZoneId,PeriodId,Performance,Variance,Rating,Audd,Audu,Audp,lu_Audd,lu_Audu,lu_Audp,IsDeleted,IsActive,CreatedAt,UpdatedAt")] PrecinctPerformance precinctPerformance)
        {
            if (ModelState.IsValid)
            {
                precinctPerformance.CreatedAt = DateTime.Now;
                precinctPerformance.UpdatedAt = DateTime.Now;
                precinctPerformance.IsDeleted = false;
                precinctPerformance.IsActive = true;

                db.PrecinctPerformances.Add(precinctPerformance);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PeriodId = new SelectList(db.Periods, "id", "PeriodType", precinctPerformance.PeriodId);
            ViewBag.PrecinctId = new SelectList(db.Precincts, "id", "PrecinctName", precinctPerformance.PrecinctId);
            ViewBag.ClusterId = new SelectList(db.Precincts, "id", "ClusterId", precinctPerformance.ClusterId);
            ViewBag.ZoneId = new SelectList(db.Precincts, "id", "ZoneId", precinctPerformance.ZoneId);
            ViewBag.ShiftId = new SelectList(db.Shifts, "id", "id", precinctPerformance.ShiftId);
            return View(precinctPerformance);
        }

        // GET: PrecinctPerformances/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PrecinctPerformance precinctPerformance = db.PrecinctPerformances.Find(id);
            if (precinctPerformance == null)
            {
                return HttpNotFound();
            }
            ViewBag.PeriodId = new SelectList(db.Periods, "id", "PeriodType", precinctPerformance.PeriodId);
            ViewBag.PrecinctId = new SelectList(db.Precincts, "id", "PrecinctName", precinctPerformance.PrecinctId);
            ViewBag.ClusterId = new SelectList(db.Precincts, "id", "ClusterId", precinctPerformance.ClusterId);
            ViewBag.ZoneId = new SelectList(db.Precincts, "id", "ZoneId", precinctPerformance.ZoneId);
            ViewBag.ShiftId = new SelectList(db.Shifts, "id", "id", precinctPerformance.ShiftId);
            return View(precinctPerformance);
        }

        // POST: PrecinctPerformances/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,ShiftId,PrecinctId,ClusterId,ZoneId,PeriodId,Performance,Variance,Rating,Audd,Audu,Audp,lu_Audd,lu_Audu,lu_Audp,IsDeleted,IsActive,CreatedAt,UpdatedAt")] PrecinctPerformance precinctPerformance)
        {
            if (ModelState.IsValid)
            {
                DateTime existingCreatedAt = (DateTime)db.PrecinctPerformances.AsNoTracking().Where(c => c.id == precinctPerformance.id).Select(c => c.CreatedAt).FirstOrDefault();

                precinctPerformance.CreatedAt = existingCreatedAt;
                precinctPerformance.UpdatedAt = DateTime.Now;

                db.Entry(precinctPerformance).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PeriodId = new SelectList(db.Periods, "id", "PeriodType", precinctPerformance.PeriodId);
            ViewBag.PrecinctId = new SelectList(db.Precincts, "id", "PrecinctName", precinctPerformance.PrecinctId);
            ViewBag.ClusterId = new SelectList(db.Precincts, "id", "ClusterId", precinctPerformance.ClusterId);
            ViewBag.ZoneId = new SelectList(db.Precincts, "id", "ZoneId", precinctPerformance.ZoneId);
            ViewBag.ShiftId = new SelectList(db.Shifts, "id", "id", precinctPerformance.ShiftId);
            return View(precinctPerformance);
        }

        // GET: PrecinctPerformances/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PrecinctPerformance precinctPerformance = db.PrecinctPerformances.Find(id);
            if (precinctPerformance == null)
            {
                return HttpNotFound();
            }
            return View(precinctPerformance);
        }

        // POST: PrecinctPerformances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PrecinctPerformance precinctPerformance = db.PrecinctPerformances.Find(id);
            db.PrecinctPerformances.Remove(precinctPerformance);
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
    }
}
