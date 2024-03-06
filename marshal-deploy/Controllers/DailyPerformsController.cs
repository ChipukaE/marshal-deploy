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
        public async Task<ActionResult> Create([Bind(Include = "id,ShiftId,UserId,DailyTargetId,Target,Total,Performance,Rating,ClusterId,Audd,Audu,Audp,lu_Audd,lu_Audu,lu_Audp,IsDeleted,IsActive,CreatedAt,UpdatedAt")] DailyPerform dailyPerform)
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
                        Target = userTarget.Target,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        IsDeleted = false,
                        IsActive = true
                    };

                    var shift = shifts.FirstOrDefault(s => s.UserId == userId);
                    if (shift != null)
                    {
                        dailyPerform1.Total = ((shift.TotalCollectedUSD - shift.CollectedEnforcementUSD)) + ((shift.TotalCollectedZW - shift.CollectedEnforcementZW) / 17300);
                        dailyPerform1.Performance = (dailyPerform1.Total / userTarget.Target) * 100;
                    }

                    dailyPerforms.Add(dailyPerform1);
                }

                // Sort the dailyPerforms list based on the Average property in descending order
                dailyPerforms = dailyPerforms.OrderByDescending(dp => dp.Performance).ToList();

                // Assign ratings based on the index of each dailyPerform in the sorted list
                for (int i = 0; i < dailyPerforms.Count; i++)
                {
                    dailyPerforms[i].Rating = i + 1;
                    dailyPerforms[i].ClusterId = (i < 110) ? 1 : 3;
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
        public ActionResult Edit([Bind(Include = "id,ShiftId,UserId,DailyTargetId,Target,Total,Performance,Rating,ClusterId,Audd,Audu,Audp,lu_Audd,lu_Audu,lu_Audp,IsDeleted,IsActive,CreatedAt,UpdatedAt")] DailyPerform dailyPerform)
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
            ViewBag.UserId = new SelectList(db.DailyTargets, "UserId", "UserId", dailyPerform.UserId);
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
