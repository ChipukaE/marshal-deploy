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
    public class PrecinctMonthliesController : Controller
    {
        private Deploy db = new Deploy();

        // GET: PrecinctMonthlies
        public ActionResult Index()
        {
            var precinctMonthlies = db.PrecinctMonthlies.Include(p => p.Cluster).Include(p => p.Precinct).Include(p => p.PrecinctPerformance);
            return View(precinctMonthlies.ToList());
        }

        // GET: PrecinctMonthlies/Details/5
        public ActionResult Details()
        {
            var precinctMonthlies = db.PrecinctMonthlies.ToList();

            return View(precinctMonthlies);
        }

        // GET: PrecinctMonthlies/Create
        public ActionResult Create()
        {
            ViewBag.ClusterId = new SelectList(db.Clusters, "id", "ClusterName");
            ViewBag.PrecinctId = new SelectList(db.PrecinctPerformances, "id", "PrecinctId");
            return View();
        }

        // POST: PrecinctMonthlies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "id,PrecinctPerformanceId,PrecinctId,ClusterId,ZoneId,Collected,Performance,Variance,Rating,Audd,Audu,Audp,lu_Audd,lu_Audu,lu_Audp,IsDeleted,IsActive,CreatedAt,UpdatedAt")] PrecinctMonthly precinctMonthly)
        {
            if (ModelState.IsValid)
            {
                var precinctPerformances = await db.PrecinctPerformances.ToListAsync();
                var precinctMonthlies = new List<PrecinctMonthly>();

                foreach (var precinctPerformance in precinctPerformances)
                {
                    var precinctId = precinctPerformance.PrecinctId;
                    var zoneId = precinctPerformance.ZoneId;

                    var startDate = DateTime.Now.Date.AddDays(-29);
                    var precinctPerforms = precinctPerformances
                        .Where(p => p.PrecinctId == precinctId && p.CreatedAt >= startDate)
                        .ToList();

                    var collected = precinctPerforms.Sum(p => p.Total);
                    var performance = precinctPerforms.Average(p => p.Performance);

                    PrecinctMonthly precinctMonthly1 = new PrecinctMonthly
                    {
                        PrecinctId = precinctId,
                        ZoneId = zoneId,
                        Collected = collected,
                        Performance = performance,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        IsDeleted = false,
                        IsActive = true
                    };

                    precinctMonthlies.Add(precinctMonthly1);
                }

                precinctMonthlies = precinctMonthlies.OrderByDescending(p => p.Performance).ToList();

                for (int i = 0; i < precinctMonthlies.Count; i++)
                {
                    precinctMonthlies[i].Rating = i + 1;
                    precinctMonthlies[i].ClusterId = (i < 110) ? 1 : 3;
                }

                db.PrecinctMonthlies.AddRange(precinctMonthlies);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ClusterId = new SelectList(db.Clusters, "id", "ClusterName", precinctMonthly.ClusterId);
            ViewBag.PrecinctId = new SelectList(db.PrecinctPerformances, "id", "PrecinctId", precinctMonthly.PrecinctId);
            return View(precinctMonthly);
        }

        // GET: PrecinctMonthlies/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PrecinctMonthly precinctMonthly = db.PrecinctMonthlies.Find(id);
            if (precinctMonthly == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClusterId = new SelectList(db.Clusters, "id", "ClusterName", precinctMonthly.ClusterId);
            ViewBag.PrecinctId = new SelectList(db.PrecinctPerformances, "id", "PrecinctId", precinctMonthly.PrecinctId);
            return View(precinctMonthly);
        }

        // POST: PrecinctMonthlies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,PrecinctPerformanceId,PrecinctId,ClusterId,ZoneId,Collected,Performance,Variance,Rating,Audd,Audu,Audp,lu_Audd,lu_Audu,lu_Audp,IsDeleted,IsActive,CreatedAt,UpdatedAt")] PrecinctMonthly precinctMonthly)
        {
            if (ModelState.IsValid)
            {
                DateTime existingCreatedAt = (DateTime)db.PrecinctMonthlies.AsNoTracking().Where(c => c.id == precinctMonthly.id).Select(c => c.CreatedAt).FirstOrDefault();

                precinctMonthly.CreatedAt = existingCreatedAt;
                precinctMonthly.UpdatedAt = DateTime.Now;

                db.Entry(precinctMonthly).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ClusterId = new SelectList(db.Clusters, "id", "ClusterName", precinctMonthly.ClusterId);
            ViewBag.PrecinctId = new SelectList(db.PrecinctPerformances, "id", "PrecinctId", precinctMonthly.PrecinctId);
            return View(precinctMonthly);
        }

        // GET: PrecinctMonthlies/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PrecinctMonthly precinctMonthly = db.PrecinctMonthlies.Find(id);
            if (precinctMonthly == null)
            {
                return HttpNotFound();
            }
            return View(precinctMonthly);
        }

        // POST: PrecinctMonthlies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PrecinctMonthly precinctMonthly = db.PrecinctMonthlies.Find(id);
            db.PrecinctMonthlies.Remove(precinctMonthly);
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
