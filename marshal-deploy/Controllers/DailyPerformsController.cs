using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
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
                var userTargets = await db.DailyTargets.ToListAsync();
                var shifts = await db.Shifts.ToListAsync();
                var dailyPerforms = new List<DailyPerform>();

                foreach (var userTarget in userTargets)
                {
                    var userId = userTarget.UserId;

                    DailyPerform dailyPerform1 = new DailyPerform
                    {
                        UserId = userId,
                        TargetZW = userTarget.TargetZW,
                        TargetUSD = userTarget.TargetUSD,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        IsDeleted = false,
                        IsActive = true
                    };

                    var shift = shifts.FirstOrDefault(s => s.UserId == userId);
                    if (shift != null)
                    {
                        dailyPerform1.TotalCountedZW = shift.TotalCountedZW;
                        dailyPerform1.TotalCountedUSD = shift.TotalCountedUSD;

                        dailyPerform1.PerformanceZW = (dailyPerform1.TotalCountedZW / dailyPerform1.TargetZW) * 100;
                        dailyPerform1.PerformanceUSD = (dailyPerform1.TotalCountedUSD / dailyPerform1.TargetUSD) * 100;

                        dailyPerform1.Average = (dailyPerform1.PerformanceZW + dailyPerform1.PerformanceUSD) / 2;

                        dailyPerform1.Rating = CalculateRating(dailyPerform1.PerformanceZW, dailyPerform1.PerformanceUSD);
                    }

                    dailyPerforms.Add(dailyPerform1);
                }

                db.DailyPerforms.AddRange(dailyPerforms);
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


        private int CalculateRating(decimal? performanceZW, decimal? performanceUSD)
        {
            // Calculate the average of PerformanceZW and PerformanceUSD
            decimal average = (performanceZW.GetValueOrDefault() + performanceUSD.GetValueOrDefault()) / 2;

            // Retrieve all the existing averages from the database
            var existingAverages = db.DailyPerforms
                .Where(d => d.PerformanceZW.HasValue && d.PerformanceUSD.HasValue)
                .Select(d => (d.PerformanceZW.Value + d.PerformanceUSD.Value) / 2)
                .ToList();

            // Sort the existing averages in descending order
            existingAverages.Sort();
            existingAverages.Reverse();

            // Find the index of the average in the sorted list
            var index = existingAverages.IndexOf(average);

            // Assign the rating based on the index
            var rating = index + 1;

            return rating;
        }

        private int CalculateClusterId(int rating)
        {
            if (rating >= 1 && rating <= 110)
            {
                return 1;
            }
            else
            {
                return 2;
            }
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
