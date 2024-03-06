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
    public class MonthlyPerformsController : Controller
    {
        private Deploy db = new Deploy();

        // GET: MonthlyPerforms
        public ActionResult Index()
        {
            var monthlyPerforms = db.MonthlyPerforms.Include(m => m.Cluster).Include(m => m.DailyPerform).Include(m => m.MonthlyTarget).Include(m => m.Shift);
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
            ViewBag.UserId = new SelectList(db.MonthlyTargets, "UserId", "UserId");
            ViewBag.ShiftId = new SelectList(db.Shifts, "id", "id");

            return View();
        }

        // POST: MonthlyPerforms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "id,ShiftId,UserId,DailyPerformId,MonthlyTargetId,Target,Collected,Performance,Rating,ClusterId,Audd,Audu,Audp,lu_Audd,lu_Audu,lu_Audp,IsDeleted,IsActive,CreatedAt,UpdatedAt")] MonthlyPerform monthlyPerform)
        {
            if (ModelState.IsValid)
            {
                var userTargets = await db.MonthlyTargets.ToListAsync();
                var dailyPerforms = await db.DailyPerforms.ToListAsync();
                var monthlyPerforms = new List<MonthlyPerform>();

                foreach (var userTarget in userTargets)
                {
                    var userId = userTarget.UserId;

                    // Retrieve daily performs for the current user for the past 30 days inclusively
                    var startDate = DateTime.Now.Date.AddDays(-29); // Get the start date 30 days ago
                    var userDailyPerforms = dailyPerforms.Where(dp => dp.UserId == userId && dp.CreatedAt >= startDate).ToList();

                    // Calculate the collected value by summing the Total values
                    var collected = userDailyPerforms.Sum(dp => dp.Total);

                    // Calculate the average performance
                    var performance = userDailyPerforms.Average(dp => dp.Performance);

                    MonthlyPerform monthlyPerform1 = new MonthlyPerform
                    {
                        UserId = userId,
                        Target = userTarget.Target,
                        Collected = collected,
                        Performance = performance,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        IsDeleted = false,
                        IsActive = true
                    };

                    monthlyPerforms.Add(monthlyPerform1);
                }

                monthlyPerforms = monthlyPerforms.OrderByDescending(m => m.Performance).ToList();

                for (int i = 0; i < monthlyPerforms.Count; i++)
                {
                    monthlyPerforms[i].Rating = i + 1;
                    monthlyPerforms[i].ClusterId = (i < 110) ? 1 : 3;
                }

                db.MonthlyPerforms.AddRange(monthlyPerforms);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ClusterId = new SelectList(db.Clusters, "id", "ClusterName", monthlyPerform.ClusterId);
            ViewBag.UserId = new SelectList(db.MonthlyTargets, "UserId", "UserId", monthlyPerform.UserId);
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
            ViewBag.UserId = new SelectList(db.MonthlyTargets, "UserId", "UserId", monthlyPerform.UserId);
            ViewBag.ShiftId = new SelectList(db.Shifts, "id", "id", monthlyPerform.ShiftId);
            return View(monthlyPerform);
        }

        // POST: MonthlyPerforms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,ShiftId,UserId,DailyPerformId,MonthlyTargetId,Target,Collected,Performance,Rating,ClusterId,Audd,Audu,Audp,lu_Audd,lu_Audu,lu_Audp,IsDeleted,IsActive,CreatedAt,UpdatedAt")] MonthlyPerform monthlyPerform)
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
            ViewBag.UserId = new SelectList(db.MonthlyTargets, "UserId", "UserId", monthlyPerform.UserId);
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
