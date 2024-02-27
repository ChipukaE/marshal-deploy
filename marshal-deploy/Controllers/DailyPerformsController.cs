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
            ViewBag.UserId = new SelectList(db.DailyTargets, "UserId", "UserId");
            ViewBag.ShiftId = new SelectList(db.Shifts, "id", "id");
            return View();
        }

        // POST: DailyPerforms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "id,ShiftId,UserId,DailyTargetId,TargetZW,TargetUSD,TotalCountedZW,TotalCountedUSD,PerformanceZW,PerformanceUSD,Average,Rating,ClusterId,Audd,Audu,Audp,lu_Audd,lu_Audu,lu_Audp,IsDeleted,IsActive,CreatedAt,UpdatedAt")] DailyPerform dailyPerform)
        {
            if (ModelState.IsValid)
            {
                dailyPerform.CreatedAt = DateTime.Now;
                dailyPerform.UpdatedAt = DateTime.Now;
                dailyPerform.IsDeleted = false;
                dailyPerform.IsActive = true;

                // Retrieve the DailyTarget based on the selected UserId
                DailyTarget dailyTarget = db.DailyTargets.FirstOrDefault(t => t.UserId == dailyPerform.UserId);

                if (dailyTarget != null)
                {
                    dailyPerform.TargetZW = dailyTarget.TargetZW;
                    dailyPerform.TargetUSD = dailyTarget.TargetUSD;
                }
                else
                {
                    dailyPerform.TargetZW = 0; 
                    dailyPerform.TargetUSD = 0; 
                }

                // Retrieve the Shift based on the selected ShiftId
                Shift shift = db.Shifts.FirstOrDefault(s => s.id == dailyPerform.ShiftId);

                if (shift != null)
                {
                    dailyPerform.TotalCountedZW = shift.TotalCountedZW;
                    dailyPerform.TotalCountedUSD = shift.TotalCountedUSD;
                }

                db.DailyPerforms.Add(dailyPerform);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ClusterId = new SelectList(db.Clusters, "id", "ClusterName", dailyPerform.ClusterId);
            ViewBag.UserId = new SelectList(db.DailyTargets, "UserId", "UserId", dailyPerform.UserId);
            ViewBag.ShiftId = new SelectList(db.Shifts, "id", "id", dailyPerform.ShiftId);

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
            ViewBag.UserId = new SelectList(db.DailyTargets, "UserId", "UserId", dailyPerform.UserId);
            ViewBag.ShiftId = new SelectList(db.Shifts, "id", "id", dailyPerform.ShiftId);
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
            ViewBag.UserId = new SelectList(db.DailyTargets, "id", "UserId", dailyPerform.UserId);
            ViewBag.ShiftId = new SelectList(db.Shifts, "id", "id", dailyPerform.ShiftId);
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
