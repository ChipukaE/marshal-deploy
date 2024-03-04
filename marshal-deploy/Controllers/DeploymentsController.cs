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
    public class DeploymentsController : Controller
    {
        private Deploy db = new Deploy();

        // GET: Deployments
        public ActionResult Index()
        {
            var deployments = db.Deployments.Include(d => d.DailyPerform).Include(d => d.PrecinctPerformance);
            return View(deployments.ToList());
        }

        // GET: Deployments/Details/5
        public ActionResult Details()
        {
            var deployments = db.Deployments.ToList();

            return View(deployments);
        }

        // GET: Deployments/Create
        public ActionResult Create()
        {
            ViewBag.ShiftId = new SelectList(db.Shifts, "id", "id");
            ViewBag.UserId = new SelectList(db.DailyPerforms, "id", "UserId");
            ViewBag.PrecinctPerformanceId = new SelectList(db.PrecinctPerformances, "id", "id");
            ViewBag.PrecinctId = new SelectList(db.PrecinctPerformances, "id", "PrecinctId");
            ViewBag.ZoneId = new SelectList(db.PrecinctPerformances, "id", "ZoneId");
            return View();
        }

        // POST: Deployments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "id,ShiftId,UserId,DailyPerformId,PrecinctPerformanceId,PrecinctId,ZoneId,Audd,Audu,Audp,lu_Audd,lu_Audu,lu_Audp,IsDeleted,IsActive,CreatedAt,UpdatedAt")] Deployment deployment)
        {
            if (ModelState.IsValid)
            {
                var userTargets = await db.DailyPerforms.ToListAsync();
                var precinctPerformances = await db.PrecinctPerformances.ToListAsync();
                var deployments = new List<Deployment>();

                foreach (var userTarget in userTargets)
                {
                    var UserId = userTarget.UserId;

                    Deployment deployment1 = new Deployment
                    {
                        UserId = UserId,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        IsDeleted = false,
                        IsActive = true
                    };

                    var precinctPerformance = precinctPerformances.FirstOrDefault();
                    if (precinctPerformance != null)
                    {
                        deployment1.PrecinctId = precinctPerformance.PrecinctId;
                        deployment1.ZoneId = precinctPerformance.ZoneId;
                    }
                    deployments.Add(deployment1);
                }

                db.Deployments.AddRange(deployments);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ShiftId = new SelectList(db.DailyPerforms, "id", "id", deployment.ShiftId);
            ViewBag.UserId = new SelectList(db.DailyPerforms, "id", "UserId", deployment.UserId);
            ViewBag.PrecinctPerformanceId = new SelectList(db.PrecinctPerformances, "id", "id", deployment.PrecinctPerformanceId);
            ViewBag.PrecinctId = new SelectList(db.PrecinctPerformances, "id", "PrecinctId", deployment.PrecinctId);
            ViewBag.ZoneId = new SelectList(db.PrecinctPerformances, "id", "ZoneId", deployment.ZoneId);
            return View(deployment);
        }

        // GET: Deployments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Deployment deployment = db.Deployments.Find(id);
            if (deployment == null)
            {
                return HttpNotFound();
            }

            ViewBag.ShiftId = new SelectList(db.DailyPerforms, "id", "id", deployment.ShiftId);
            ViewBag.UserId = new SelectList(db.DailyPerforms, "id", "UserId", deployment.DailyPerformId);
            ViewBag.PrecinctPerformanceId = new SelectList(db.PrecinctPerformances, "id", "id", deployment.PrecinctPerformanceId);
            ViewBag.PrecinctId = new SelectList(db.PrecinctPerformances, "id", "PrecinctId", deployment.PrecinctId);
            ViewBag.ZoneId = new SelectList(db.PrecinctPerformances, "id", "ZoneId", deployment.ZoneId);
            return View(deployment);
        }

        // POST: Deployments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,ShiftId,UserId,DailyPerformId,PrecinctPerformanceId,PrecinctId,ZoneId,Audd,Audu,Audp,lu_Audd,lu_Audu,lu_Audp,IsDeleted,IsActive,CreatedAt,UpdatedAt")] Deployment deployment)
        {
            if (ModelState.IsValid)
            {
                DateTime existingCreatedAt = (DateTime)db.Deployments.AsNoTracking().Where(c => c.id == deployment.id).Select(c => c.CreatedAt).FirstOrDefault();

                deployment.CreatedAt = existingCreatedAt;
                deployment.UpdatedAt = DateTime.Now;

                db.Entry(deployment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ShiftId = new SelectList(db.DailyPerforms, "id", "id", deployment.ShiftId);
            ViewBag.UserId = new SelectList(db.DailyPerforms, "id", "UserId", deployment.DailyPerformId);
            ViewBag.PrecinctPerformanceId = new SelectList(db.PrecinctPerformances, "id", "id", deployment.PrecinctPerformanceId);
            ViewBag.PrecinctId = new SelectList(db.PrecinctPerformances, "id", "PrecinctId", deployment.PrecinctId);
            ViewBag.ZoneId = new SelectList(db.PrecinctPerformances, "id", "ZoneId", deployment.ZoneId);
            return View(deployment);
        }

        // GET: Deployments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Deployment deployment = db.Deployments.Find(id);
            if (deployment == null)
            {
                return HttpNotFound();
            }
            return View(deployment);
        }

        // POST: Deployments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Deployment deployment = db.Deployments.Find(id);
            db.Deployments.Remove(deployment);
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

