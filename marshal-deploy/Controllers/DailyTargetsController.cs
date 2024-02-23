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
    public class DailyTargetsController : Controller
    {
        private Deploy db = new Deploy();

        // GET: DailyTargets
        public ActionResult Index()
        {
            var dailyTargets = db.DailyTargets.Include(d => d.Shift);
            return View(dailyTargets.ToList());
        }


        //get users
        public ActionResult GetUserIds(int shiftId)
        {
            using (var dbContext = new Deploy())
            {
                var userIds = dbContext.Shifts
                    .Where(s => s.id == shiftId)
                    .Select(s => s.UserId)
                    .ToList();

                var userIdOptions = userIds.Select(u => new SelectListItem
                {
                    Value = u,
                    Text = u
                });

                return Json(userIdOptions, JsonRequestBehavior.AllowGet);
            }
        }


        // GET: DailyTargets/Details/5
        public ActionResult Details()
        {
            var dailyTarget = db.DailyTargets.ToList();

            return View(dailyTarget);
        }

        // GET: DailyTargets/Create
        public ActionResult Create()
        {
            ViewBag.ShiftId = new SelectList(db.Shifts, "id", "id");
            ViewBag.UserId = new SelectList(db.Shifts, "id", "UserId");
            return View();
        }

        // POST: DailyTargets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,ShiftId,UserId,TargetZW,TargetUSD,Audd,Audu,Audp,lu_Audd,lu_Audu,lu_Audp,IsDeleted,IsActive,CreatedAt,UpdatedAt")] DailyTarget dailyTarget)
        {
            if (ModelState.IsValid)
            {
                dailyTarget.CreatedAt = DateTime.Now;
                dailyTarget.UpdatedAt = DateTime.Now;
                dailyTarget.IsDeleted = false;
                dailyTarget.IsActive = true;

                // Retrieve the associated UserId based on the selected ShiftId
                int selectedShiftId = (int)dailyTarget.ShiftId;
                var userId = db.Shifts.Where(s => s.id == selectedShiftId).Select(s => s.UserId).FirstOrDefault();
                dailyTarget.UserId = userId;

                db.DailyTargets.Add(dailyTarget);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ShiftId = new SelectList(db.Shifts, "id", "id", dailyTarget.ShiftId);
            ViewBag.UserId = new SelectList(db.Shifts, "id", "UserId", dailyTarget.UserId);

            return View(dailyTarget);
        }

        // GET: DailyTargets/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DailyTarget dailyTarget = db.DailyTargets.Find(id);
            if (dailyTarget == null)
            {
                return HttpNotFound();
            }
            ViewBag.ShiftId = new SelectList(db.Shifts, "id", "id", dailyTarget.ShiftId);
            ViewBag.UserId = new SelectList(db.Shifts, "id", "UserId", dailyTarget.UserId);
            return View(dailyTarget);
        }

        // POST: DailyTargets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,ShiftId,UserId,TargetZW,TargetUSD,Audd,Audu,Audp,lu_Audd,lu_Audu,lu_Audp,IsDeleted,IsActive,CreatedAt,UpdatedAt")] DailyTarget dailyTarget)
        {
            if (ModelState.IsValid)
            {
                DateTime existingCreatedAt = (DateTime)db.DailyTargets.AsNoTracking().Where(c => c.id == dailyTarget.id).Select(c => c.CreatedAt).FirstOrDefault();

                dailyTarget.CreatedAt = existingCreatedAt;
                dailyTarget.UpdatedAt = DateTime.Now;

                db.Entry(dailyTarget).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ShiftId = new SelectList(db.Shifts, "id", "id", dailyTarget.ShiftId);
            ViewBag.UserId = new SelectList(db.Shifts, "id", "UserId", dailyTarget.UserId);
            return View(dailyTarget);
        }

        // GET: DailyTargets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DailyTarget dailyTarget = db.DailyTargets.Find(id);
            if (dailyTarget == null)
            {
                return HttpNotFound();
            }
            return View(dailyTarget);
        }

        // POST: DailyTargets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DailyTarget dailyTarget = db.DailyTargets.Find(id);
            if (dailyTarget == null)
            {
                return HttpNotFound();
            }

            dailyTarget.IsDeleted = true;
            dailyTarget.IsActive = false;
            dailyTarget.UpdatedAt = DateTime.Now; // Update the 'UpdatedAt' field

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
