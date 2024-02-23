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
    public class ClustersController : Controller
    {
        private Deploy db = new Deploy();

        // GET: Clusters
        public ActionResult Index()
        {
            return View(db.Clusters.ToList());
        }

        // GET: Clusters/Details/5
        public ActionResult Details()
        {
            var cluster = db.Clusters.ToList();

            return View(cluster);
        }

        // GET: Clusters/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Clusters/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,ClusterName,TarriffID,Audd,Audu,Audp,lu_Audd,lu_Audu,lu_Audp,IsDeleted,IsActive,CreatedAt,UpdatedAt")] Cluster cluster)
        {
            if (ModelState.IsValid)
            {
                cluster.CreatedAt = DateTime.Now;
                cluster.UpdatedAt = DateTime.Now;
                cluster.IsDeleted = false;
                cluster.IsActive = true;

                db.Clusters.Add(cluster);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cluster);
        }

        // GET: Clusters/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cluster cluster = db.Clusters.Find(id);
            if (cluster == null)
            {
                return HttpNotFound();
            }
            return View(cluster);
        }

        // POST: Clusters/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,ClusterName,TarriffID,Audd,Audu,Audp,lu_Audd,lu_Audu,lu_Audp,IsDeleted,IsActive,CreatedAt,UpdatedAt")] Cluster cluster)
        {
            if (ModelState.IsValid)
            {
                DateTime existingCreatedAt = (DateTime)db.Clusters.AsNoTracking().Where(c => c.id == cluster.id).Select(c => c.CreatedAt).FirstOrDefault();

                cluster.CreatedAt = existingCreatedAt;
                cluster.UpdatedAt = DateTime.Now;

                db.Entry(cluster).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cluster);
        }

        // GET: Clusters/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cluster cluster = db.Clusters.Find(id);
            if (cluster == null)
            {
                return HttpNotFound();
            }
            return View(cluster);
        }

        // POST: Clusters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Cluster cluster = db.Clusters.Find(id);
            db.Clusters.Remove(cluster);
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
