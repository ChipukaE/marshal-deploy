using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Threading.Tasks;
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
            var precinctPerformances = db.PrecinctPerformances.Include(p => p.Period).Include(p => p.Precinct).Include(p => p.Cluster).Include(p => p.Shift);
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
            ViewBag.ClusterId = new SelectList(db.Clusters, "id", "ClusterName");
            ViewBag.ShiftId = new SelectList(db.Shifts, "id", "id");
            return View();
        }

        // POST: PrecinctPerformances/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "id,ShiftId,PrecinctId,ClusterId,ZoneId,PeriodId,Total,Performance,Variance,Rating,Audd,Audu,Audp,lu_Audd,lu_Audu,lu_Audp,IsDeleted,IsActive,CreatedAt,UpdatedAt")] PrecinctPerformance precinctPerformance)
        {
            if (ModelState.IsValid)
            {
                var precinctTargets = await db.Precincts.ToListAsync();
                var shifts = await db.Shifts.ToListAsync();
                var precinctPerformances = new List<PrecinctPerformance>();

                foreach (var precinctTarget in precinctTargets)
                {
                    var precinctId = precinctTarget.id;
                    var target = precinctTarget.Target;

                    PrecinctPerformance performance = new PrecinctPerformance
                    {
                        PrecinctId = precinctId,
                        ZoneId = precinctTarget.ZoneId,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        IsDeleted = false,
                        IsActive = true
                    };

                    var shift = shifts.FirstOrDefault(s => s.PrecinctId == precinctId);
                    if (shift != null)
                    {
                        performance.Total = ((shift.TotalCollectedUSD - shift.CollectedEnforcementUSD)) + ((shift.TotalCollectedZW - shift.CollectedEnforcementZW) / 17300);
                        performance.Performance = (performance.Total / precinctTarget.Target) * 100;
                    }

                    precinctPerformances.Add(performance);
                }

                precinctPerformances = precinctPerformances.OrderByDescending(p => p.Performance).ToList();
                for (int i = 0; i < precinctPerformances.Count; i++)
                {
                    precinctPerformances[i].Rating = i + 1;
                    precinctPerformances[i].ClusterId = (i < 110) ? 1 : 3;
                }

                db.PrecinctPerformances.AddRange(precinctPerformances);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PeriodId = new SelectList(db.Periods, "id", "PeriodType", precinctPerformance.PeriodId);
            ViewBag.PrecinctId = new SelectList(db.Precincts, "id", "PrecinctName", precinctPerformance.PrecinctId);
            ViewBag.ClusterId = new SelectList(db.Clusters, "id", "ClusterName", precinctPerformance.ClusterId);
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
        public ActionResult Edit([Bind(Include = "id,ShiftId,PrecinctId,ClusterId,ZoneId,PeriodId,Total,Performance,Variance,Rating,Audd,Audu,Audp,lu_Audd,lu_Audu,lu_Audp,IsDeleted,IsActive,CreatedAt,UpdatedAt")] PrecinctPerformance precinctPerformance)
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

