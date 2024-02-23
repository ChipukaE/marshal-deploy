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
    public class PrecinctsController : Controller
    {
        private Deploy db = new Deploy();

        // GET: Precincts
        public ActionResult Index()
        {
            var precincts = db.Precincts.Include(p => p.Cluster).Include(p => p.Zone);
            return View(precincts.ToList());
        }

        // GET: Precincts/Details/5
        public ActionResult Details()
        {
            var precincts = db.Precincts.ToList();

            return View(precincts);
        }

        // GET: Precincts/Create
        public ActionResult Create()
        {
            ViewBag.ClusterId = new SelectList(db.Clusters, "id", "ClusterName");
            ViewBag.ZoneId = new SelectList(db.Zones, "id", "ZoneName");
            return View();
        }

        // POST: Precincts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,PrecinctName,ZoneId,ClusterId,Target,Lunch,Audd,Audu,Audp,lu_Audd,lu_Audu,lu_Audp,IsDeleted,IsActive,CreatedAt,UpdatedAt")] Precinct precinct)
        {
            if (ModelState.IsValid)
            {
                precinct.CreatedAt = DateTime.Now;
                precinct.UpdatedAt = DateTime.Now;
                precinct.IsDeleted = false;
                precinct.IsActive = true;

                db.Precincts.Add(precinct);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ClusterId = new SelectList(db.Clusters, "id", "ClusterName", precinct.ClusterId);
            ViewBag.ZoneId = new SelectList(db.Zones, "id", "ZoneName", precinct.ZoneId);
            return View(precinct);
        }

        // GET: Precincts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Precinct precinct = db.Precincts.Find(id);
            if (precinct == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClusterId = new SelectList(db.Clusters, "id", "ClusterName", precinct.ClusterId);
            ViewBag.ZoneId = new SelectList(db.Zones, "id", "ZoneName", precinct.ZoneId);
            return View(precinct);
        }

        // POST: Precincts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,PrecinctName,ZoneId,ClusterId,Target,Lunch,Audd,Audu,Audp,lu_Audd,lu_Audu,lu_Audp,IsDeleted,IsActive,CreatedAt,UpdatedAt")] Precinct precinct)
        {
            if (ModelState.IsValid)
            {
                DateTime existingCreatedAt = (DateTime)db.Precincts.AsNoTracking().Where(c => c.id == precinct.id).Select(c => c.CreatedAt).FirstOrDefault();

                precinct.CreatedAt = existingCreatedAt;
                precinct.UpdatedAt = DateTime.Now;

                db.Entry(precinct).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ClusterId = new SelectList(db.Clusters, "id", "ClusterName", precinct.ClusterId);
            ViewBag.ZoneId = new SelectList(db.Zones, "id", "ZoneName", precinct.ZoneId);
            return View(precinct);
        }

        // GET: Precincts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Precinct precinct = db.Precincts.Find(id);
            if (precinct == null)
            {
                return HttpNotFound();
            }
            return View(precinct);
        }

        // POST: Precincts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Precinct precinct = db.Precincts.Find(id);
            db.Precincts.Remove(precinct);
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
