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
    public class MonthlyTargetsController : Controller
    {
        private Deploy db = new Deploy();

        // GET: MonthlyTargets
        public ActionResult Index()
        {
            var monthlyTargets = db.MonthlyTargets.Include(m => m.Shift);
            return View(monthlyTargets.ToList());
        }

        // GET: MonthlyTargets/Details/5
        public ActionResult Details()
        {
            var monthlyTargets = db.MonthlyTargets.ToList();

            return View(monthlyTargets);
        }

        // GET: MonthlyTargets/Create
        public ActionResult Create()
        {
            ViewBag.ShiftId = new SelectList(db.Shifts, "id", "id");
            ViewBag.UserId = new SelectList(db.Shifts, "id", "UserId");
            return View();
        }

        // POST: MonthlyTargets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,ShiftId,UserId,MonthlyZW,MonthlyUSD,Audd,Audu,Audp,lu_Audd,lu_Audu,lu_Audp,IsDeleted,IsActive,CreatedAt,UpdatedAt")] MonthlyTarget monthlyTarget)
        {
            if (ModelState.IsValid)
            {
                monthlyTarget.CreatedAt = DateTime.Now;
                monthlyTarget.UpdatedAt = DateTime.Now;
                monthlyTarget.IsDeleted = false;
                monthlyTarget.IsActive = true;

                // Retrieve the UserId associated with the selected ShiftId
                var shift = db.Shifts.Find(monthlyTarget.ShiftId);
                if (shift != null)
                {
                    monthlyTarget.UserId = shift.UserId;
                }

                db.MonthlyTargets.Add(monthlyTarget);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ShiftId = new SelectList(db.Shifts, "id", "id", monthlyTarget.ShiftId);
            ViewBag.UserId = new SelectList(db.Shifts, "id", "UserId", monthlyTarget.UserId);
            return View(monthlyTarget);
        }

        // GET: MonthlyTargets/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MonthlyTarget monthlyTarget = db.MonthlyTargets.Find(id);
            if (monthlyTarget == null)
            {
                return HttpNotFound();
            }
            ViewBag.ShiftId = new SelectList(db.Shifts, "id", "id", monthlyTarget.ShiftId);
            ViewBag.UserId = new SelectList(db.Shifts, "id", "UserId", monthlyTarget.UserId);
            return View(monthlyTarget);
        }

        // POST: MonthlyTargets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,ShiftId,UserId,MonthlyZW,MonthlyUSD,Audd,Audu,Audp,lu_Audd,lu_Audu,lu_Audp,IsDeleted,IsActive,CreatedAt,UpdatedAt")] MonthlyTarget monthlyTarget)
        {
            if (ModelState.IsValid)
            {
                DateTime existingCreatedAt = (DateTime)db.MonthlyTargets.AsNoTracking().Where(c => c.id == monthlyTarget.id).Select(c => c.CreatedAt).FirstOrDefault();

                monthlyTarget.CreatedAt = existingCreatedAt;
                monthlyTarget.UpdatedAt = DateTime.Now;

                db.Entry(monthlyTarget).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ShiftId = new SelectList(db.Shifts, "id", "id", monthlyTarget.ShiftId);
            ViewBag.UserId = new SelectList(db.Shifts, "id", "UserId", monthlyTarget.UserId);
            return View(monthlyTarget);
        }

        // GET: MonthlyTargets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MonthlyTarget monthlyTarget = db.MonthlyTargets.Find(id);
            if (monthlyTarget == null)
            {
                return HttpNotFound();
            }
            return View(monthlyTarget);
        }

        // POST: MonthlyTargets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MonthlyTarget monthlyTarget = db.MonthlyTargets.Find(id);
            db.MonthlyTargets.Remove(monthlyTarget);
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
