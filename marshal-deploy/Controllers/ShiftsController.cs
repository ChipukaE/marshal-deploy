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
    public class ShiftsController : Controller
    {
        private Deploy db = new Deploy();

        // GET: Shifts
        public ActionResult Index()
        {
            var shifts = db.Shifts.Include(s => s.Precinct);
            return View(shifts.ToList());
        }

        // GET: Shifts/Details/5
        public ActionResult Details()
        {
            var shifts = db.Shifts.ToList();

            return View(shifts);
        }

        // GET: Shifts/Create
        public ActionResult Create()
        {
            ViewBag.PrecinctId = new SelectList(db.Precincts, "id", "PrecinctName");
            return View();
        }

        // POST: Shifts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,DeviceShiftCode,PrecinctId,UserId,TerminalID,start_datetime,end_datetime,Duration,IsOpen,ticketissued_amount,Epayments,total_arrears,total_prepaid,ShiftDiffCodeId,ShiftDifference,Audd,Audu,Audp,lu_Audd,lu_Audu,lu_Audp,IsDeleted,IsActive,TotalCollectedUSD,TotalCollectedZW,TotalCountedZW,TotalCountedUSD,DeclaredVarianceZW,DeclaredVarianceUSD,CashupVarianceZW,CashupVarianceUSD,CashupZWD,CashupUSD,Total,Cashier,CashupAudd,LastConnection,CollectedEnforcementZW,CollectedEnforcementUSD,CreatedAt,UpdatedAt")] Shift shift)
        {
            if (ModelState.IsValid)
            {
                shift.CreatedAt = DateTime.Now;
                shift.UpdatedAt = DateTime.Now;
                shift.IsDeleted = false;
                shift.IsActive = true;
                shift.IsOpen = true;

                db.Shifts.Add(shift);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PrecinctId = new SelectList(db.Precincts, "id", "PrecinctName", shift.PrecinctId);
            return View(shift);
        }

        // GET: Shifts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Shift shift = db.Shifts.Find(id);
            if (shift == null)
            {
                return HttpNotFound();
            }
            ViewBag.PrecinctId = new SelectList(db.Precincts, "id", "PrecinctName", shift.PrecinctId);
            return View(shift);
        }

        // POST: Shifts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,DeviceShiftCode,PrecinctId,UserId,TerminalID,start_datetime,end_datetime,Duration,IsOpen,ticketissued_amount,Epayments,total_arrears,total_prepaid,ShiftDiffCodeId,ShiftDifference,Audd,Audu,Audp,lu_Audd,lu_Audu,lu_Audp,IsDeleted,IsActive,TotalCollectedUSD,TotalCollectedZW,TotalCountedZW,TotalCountedUSD,DeclaredVarianceZW,DeclaredVarianceUSD,CashupVarianceZW,CashupVarianceUSD,CashupZWD,CashupUSD,Total,Cashier,CashupAudd,LastConnection,CollectedEnforcementZW,CollectedEnforcementUSD,CreatedAt,UpdatedAt")] Shift shift)
        {
            if (ModelState.IsValid)
            {
                DateTime existingCreatedAt = (DateTime)db.Shifts.AsNoTracking().Where(c => c.id == shift.id).Select(c => c.CreatedAt).FirstOrDefault();

                shift.CreatedAt = existingCreatedAt;
                shift.UpdatedAt = DateTime.Now;

                db.Entry(shift).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PrecinctId = new SelectList(db.Precincts, "id", "PrecinctName", shift.PrecinctId);
            return View(shift);
        }

        // GET: Shifts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Shift shift = db.Shifts.Find(id);
            if (shift == null)
            {
                return HttpNotFound();
            }
            return View(shift);
        }

        // POST: Shifts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Shift shift = db.Shifts.Find(id);
            db.Shifts.Remove(shift);
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
