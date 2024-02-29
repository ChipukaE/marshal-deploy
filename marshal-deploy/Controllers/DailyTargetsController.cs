using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
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
            ViewBag.UserId = new SelectList(db.Shifts, "UserId", "UserId");
            return View();
        }

        // POST: DailyTargets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "id,ShiftId,UserId,TargetZW,TargetUSD,Audd,Audu,Audp,lu_Audd,lu_Audu,lu_Audp,IsDeleted,IsActive,CreatedAt,UpdatedAt")] DailyTarget dailyTarget)
        {
            if (ModelState.IsValid)
            {
                dailyTarget.CreatedAt = DateTime.Now;
                dailyTarget.UpdatedAt = DateTime.Now;
                dailyTarget.IsDeleted = false;
                dailyTarget.IsActive = true;

                db.DailyTargets.Add(dailyTarget);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ShiftId = new SelectList(db.Shifts, "id", "id", dailyTarget.ShiftId);
            ViewBag.UserId = new SelectList(db.Shifts, "UserId", "UserId", dailyTarget.UserId);

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
            ViewBag.UserId = new SelectList(db.Shifts, "UserId", "UserId", dailyTarget.UserId);
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
            ViewBag.UserId = new SelectList(db.Shifts, "UserId", "UserId", dailyTarget.UserId);
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


        //retrieving all users
        // GET: /DailyTargets/FetchAllUserIds
        public async Task<JsonResult> FetchAllUserIds() { 
            var userIds = db.DailyTargets.Select(target => target.UserId).Distinct().ToList(); 
            return Json(new { userIds = userIds }, JsonRequestBehavior.AllowGet); 
        }


        //retrieving targets
        public async Task<ActionResult> FetchTargetValues(string userId)
        {
            var dailyTarget = db.DailyTargets.FirstOrDefault(t => t.UserId.Equals(userId));

            if (dailyTarget != null)
            {
                var targetValues = new
                {
                    TargetZW = decimal.Round((decimal)dailyTarget.TargetZW, 2),
                    TargetUSD = decimal.Round((decimal)dailyTarget.TargetUSD, 2)
                };

                return Json(targetValues, JsonRequestBehavior.AllowGet);
            }

            return Json(null, JsonRequestBehavior.AllowGet);
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
