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
    public class DailyPerformsController : Controller
    {
        private Deploy db = new Deploy();

        // GET: DailyPerforms
        public ActionResult Index()
        {
            var dailyPerforms = db.DailyPerforms.Include(d => d.Cluster).Include(d => d.DailyTarget).Include(d => d.Shift);
            return View(dailyPerforms.ToList());
        }

        // GET: DailyPerforms/Details/5
        public ActionResult Details()
        {
            var dailyPerforms = db.DailyPerforms.ToList();

            return View(dailyPerforms);
        }

        // GET: DailyPerforms/Create
        public ActionResult Create()
        {
            
            ViewBag.ClusterId = new SelectList(db.Clusters, "id", "ClusterName");
            ViewBag.DailyTargetId = new SelectList(db.DailyTargets, "id", "UserId");
            ViewBag.TargetZW = new SelectList(db.DailyTargets, "id", "TargetZW");
            ViewBag.TargetUSD = new SelectList(db.DailyTargets, "id", "TargetUSD");
            ViewBag.ShiftId = new SelectList(db.Shifts, "id", "id");
            ViewBag.TotalCountedZW = new SelectList(db.Shifts, "id", "TotalCountedZW");
            ViewBag.TotalCountedUSD = new SelectList(db.Shifts, "id", "TotalCountedUSD");
            return View();
        }

        // POST: DailyPerforms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,ShiftId,UserId,DailyTargetId,TargetZW,TargetUSD,TotalCountedZW,TotalCountedUSD,PerformanceZW,PerformanceUSD,Average,Rating,ClusterId,Audd,Audu,Audp,lu_Audd,lu_Audu,lu_Audp,IsDeleted,IsActive,CreatedAt,UpdatedAt")] DailyPerform dailyPerform)
        {
            if (ModelState.IsValid)
            {
                dailyPerform.CreatedAt = DateTime.Now;
                dailyPerform.UpdatedAt = DateTime.Now;
                dailyPerform.IsDeleted = false;
                dailyPerform.IsActive = true;


                // Calculate performancezw and performanceusd
                dailyPerform.PerformanceZW = (dailyPerform.TotalCountedZW / dailyPerform.TargetZW) * 100;
                dailyPerform.PerformanceUSD = (dailyPerform.TotalCountedUSD / dailyPerform.TargetUSD) * 100;

                // Calculate average
                dailyPerform.Average = (dailyPerform.PerformanceZW + dailyPerform.PerformanceUSD) / 2;

                // Assign rating
                var performances = db.DailyPerforms.ToList();
                var highestAverage = performances.Max(p => p.Average);
                dailyPerform.Rating = performances.Count(p => p.Average > highestAverage) + 1;

                db.DailyPerforms.Add(dailyPerform);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ClusterId = new SelectList(db.Clusters, "id", "ClusterName", dailyPerform.ClusterId);
            ViewBag.DailyTargetId = new SelectList(db.DailyTargets, "id", "UserId", dailyPerform.DailyTargetId);
            ViewBag.TargetZW = new SelectList(db.DailyTargets, "id", "TargetZW", dailyPerform.TargetZW);
            ViewBag.TargetUSD = new SelectList(db.DailyTargets, "id", "TargetUSD", dailyPerform.TargetUSD);
            ViewBag.ShiftId = new SelectList(db.Shifts, "id", "id", dailyPerform.ShiftId);
            ViewBag.TotalCountedZW = new SelectList(db.Shifts, "id", "TotalCountedZW", dailyPerform.TotalCountedZW);
            ViewBag.TotalCountedUSD = new SelectList(db.Shifts, "id", "TotalCountedUSD", dailyPerform.TotalCountedUSD);
            return View(dailyPerform);
        }

        // GET: DailyPerforms/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DailyPerform dailyPerform = db.DailyPerforms.Find(id);
            if (dailyPerform == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClusterId = new SelectList(db.Clusters, "id", "ClusterName", dailyPerform.ClusterId);
            ViewBag.DailyTargetId = new SelectList(db.DailyTargets, "id", "UserId", dailyPerform.DailyTargetId);
            ViewBag.TargetZW = new SelectList(db.DailyTargets, "id", "TargetZW", dailyPerform.TargetZW);
            ViewBag.TargetUSD = new SelectList(db.DailyTargets, "id", "TargetUSD", dailyPerform.TargetUSD);
            ViewBag.ShiftId = new SelectList(db.Shifts, "id", "id", dailyPerform.ShiftId);
            ViewBag.TotalCountedZW = new SelectList(db.Shifts, "id", "TotalCountedZW", dailyPerform.TotalCountedZW);
            ViewBag.TotalCountedUSD = new SelectList(db.Shifts, "id", "TotalCountedUSD", dailyPerform.TotalCountedUSD);
            return View(dailyPerform);
        }

        // POST: DailyPerforms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,ShiftId,UserId,DailyTargetId,TargetZW,TargetUSD,TotalCountedZW,TotalCountedUSD,PerformanceZW,PerformanceUSD,Average,Rating,ClusterId,Audd,Audu,Audp,lu_Audd,lu_Audu,lu_Audp,IsDeleted,IsActive,CreatedAt,UpdatedAt")] DailyPerform dailyPerform)
        {
            if (ModelState.IsValid)
            {
                DateTime existingCreatedAt = (DateTime)db.DailyPerforms.AsNoTracking().Where(c => c.id == dailyPerform.id).Select(c => c.CreatedAt).FirstOrDefault();

                dailyPerform.CreatedAt = existingCreatedAt;
                dailyPerform.UpdatedAt = DateTime.Now;

                db.Entry(dailyPerform).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ClusterId = new SelectList(db.Clusters, "id", "ClusterName", dailyPerform.ClusterId);
            ViewBag.DailyTargetId = new SelectList(db.DailyTargets, "id", "UserId", dailyPerform.DailyTargetId);
            ViewBag.TargetZW = new SelectList(db.DailyTargets, "id", "TargetZW", dailyPerform.TargetZW);
            ViewBag.TargetUSD = new SelectList(db.DailyTargets, "id", "TargetUSD", dailyPerform.TargetUSD);
            ViewBag.ShiftId = new SelectList(db.Shifts, "id", "id", dailyPerform.ShiftId);
            ViewBag.TotalCountedZW = new SelectList(db.Shifts, "id", "TotalCountedZW", dailyPerform.TotalCountedZW);
            ViewBag.TotalCountedUSD = new SelectList(db.Shifts, "id", "TotalCountedUSD", dailyPerform.TotalCountedUSD);
            return View(dailyPerform);
        }

        // GET: DailyPerforms/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DailyPerform dailyPerform = db.DailyPerforms.Find(id);
            if (dailyPerform == null)
            {
                return HttpNotFound();
            }
            return View(dailyPerform);
        }

        // POST: DailyPerforms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DailyPerform dailyPerform = db.DailyPerforms.Find(id);
            db.DailyPerforms.Remove(dailyPerform);
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
