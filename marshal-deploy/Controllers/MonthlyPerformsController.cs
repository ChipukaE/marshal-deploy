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
    public class MonthlyPerformsController : Controller
    {
        private Deploy db = new Deploy();

        // GET: MonthlyPerforms
        public ActionResult Index()
        {
            var monthlyPerforms = db.MonthlyPerforms.Include(m => m.Cluster).Include(m => m.MonthlyTarget).Include(m => m.Shift);
            return View(monthlyPerforms.ToList());
        }

        // GET: MonthlyPerforms/Details/5
        public ActionResult Details()
        {
            var monthlyPerforms = db.MonthlyPerforms.ToList();

            return View(monthlyPerforms);
        }

        // GET: MonthlyPerforms/Create
        public ActionResult Create()
        {
            ViewBag.ClusterId = new SelectList(db.Clusters, "id", "ClusterName");
            ViewBag.MonthlyTargetId = new SelectList(db.MonthlyTargets, "id", "UserId");
            ViewBag.MonthlyZW = new SelectList(db.MonthlyTargets, "id", "MonthlyZW");
            ViewBag.MonthlyUSD = new SelectList(db.MonthlyTargets, "id", "MonthlyUSD");
            ViewBag.ShiftId = new SelectList(db.Shifts, "id", "id");

            return View();
        }

        // POST: MonthlyPerforms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,ShiftId,UserId,MonthlyTargetId,MonthlyZW,MonthlyUSD,CollectedZW,CollectedUSD,PerformanceZW,PerformanceUSD,Average,Rating,ClusterId,Audd,Audu,Audp,lu_Audd,lu_Audu,lu_Audp,IsDeleted,IsActive,CreatedAt,UpdatedAt")] MonthlyPerform monthlyPerform)
        {
            if (ModelState.IsValid)
            {
                monthlyPerform.CreatedAt = DateTime.Now;
                monthlyPerform.UpdatedAt = DateTime.Now;
                monthlyPerform.IsDeleted = false;
                monthlyPerform.IsActive = true;

                // Find the selected MonthlyTarget
                MonthlyTarget selectedTarget = db.MonthlyTargets.Find(monthlyPerform.MonthlyTargetId);
                if (selectedTarget != null)
                {
                    // Set MonthlyZW and MonthlyUSD based on the selected MonthlyTarget
                    monthlyPerform.MonthlyZW = selectedTarget.MonthlyZW;
                    monthlyPerform.MonthlyUSD = selectedTarget.MonthlyUSD;
                }

                db.MonthlyPerforms.Add(monthlyPerform);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ClusterId = new SelectList(db.Clusters, "id", "ClusterName", monthlyPerform.ClusterId);
            ViewBag.MonthlyTargetId = new SelectList(db.MonthlyTargets, "id", "UserId", monthlyPerform.MonthlyTargetId);
            ViewBag.MonthlyZW = new SelectList(db.MonthlyTargets, "id", "MonthlyZW", monthlyPerform.MonthlyZW);
            ViewBag.MonthlyUSD = new SelectList(db.MonthlyTargets, "id", "MonthlyUSD", monthlyPerform.MonthlyUSD);
            ViewBag.ShiftId = new SelectList(db.Shifts, "id", "id", monthlyPerform.ShiftId);
            return View(monthlyPerform);
        }

        // GET: MonthlyPerforms/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MonthlyPerform monthlyPerform = db.MonthlyPerforms.Find(id);
            if (monthlyPerform == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClusterId = new SelectList(db.Clusters, "id", "ClusterName", monthlyPerform.ClusterId);
            ViewBag.MonthlyTargetId = new SelectList(db.MonthlyTargets, "id", "UserId", monthlyPerform.MonthlyTargetId);
            ViewBag.MonthlyZW = new SelectList(db.MonthlyTargets, "id", "MonthlyZW", monthlyPerform.MonthlyZW);
            ViewBag.MonthlyUSD = new SelectList(db.MonthlyTargets, "id", "MonthlyUSD", monthlyPerform.MonthlyUSD);
            ViewBag.ShiftId = new SelectList(db.Shifts, "id", "id", monthlyPerform.ShiftId);
            return View(monthlyPerform);
        }

        // POST: MonthlyPerforms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,ShiftId,UserId,MonthlyTargetId,MonthlyZW,MonthlyUSD,CollectedZW,CollectedUSD,PerformanceZW,PerformanceUSD,Average,Rating,ClusterId,Audd,Audu,Audp,lu_Audd,lu_Audu,lu_Audp,IsDeleted,IsActive,CreatedAt,UpdatedAt")] MonthlyPerform monthlyPerform)
        {
            if (ModelState.IsValid)
            {
                DateTime existingCreatedAt = (DateTime)db.MonthlyPerforms.AsNoTracking().Where(c => c.id == monthlyPerform.id).Select(c => c.CreatedAt).FirstOrDefault();

                monthlyPerform.CreatedAt = existingCreatedAt;
                monthlyPerform.UpdatedAt = DateTime.Now;

                db.Entry(monthlyPerform).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ClusterId = new SelectList(db.Clusters, "id", "ClusterName", monthlyPerform.ClusterId);
            ViewBag.MonthlyTargetId = new SelectList(db.MonthlyTargets, "id", "UserId", monthlyPerform.MonthlyTargetId);
            ViewBag.MonthlyZW = new SelectList(db.MonthlyTargets, "id", "MonthlyZW", monthlyPerform.MonthlyZW);
            ViewBag.MonthlyUSD = new SelectList(db.MonthlyTargets, "id", "MonthlyUSD", monthlyPerform.MonthlyUSD);
            ViewBag.ShiftId = new SelectList(db.Shifts, "id", "id", monthlyPerform.ShiftId);
            return View(monthlyPerform);
        }

        // GET: MonthlyPerforms/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MonthlyPerform monthlyPerform = db.MonthlyPerforms.Find(id);
            if (monthlyPerform == null)
            {
                return HttpNotFound();
            }
            return View(monthlyPerform);
        }

        // POST: MonthlyPerforms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MonthlyPerform monthlyPerform = db.MonthlyPerforms.Find(id);
            db.MonthlyPerforms.Remove(monthlyPerform);
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
