using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OnlineAppointment.Models;

namespace OnlineAppointment.Controllers
{
    public class AppointmentStatesController : Controller
    {
        private OnlineAppointmentContext db = new OnlineAppointmentContext();

        // GET: AppointmentStates
        public ActionResult Index()
        {
            return View(db.AppointmentStates.ToList());
        }

        // GET: AppointmentStates/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AppointmentState appointmentState = db.AppointmentStates.Find(id);
            if (appointmentState == null)
            {
                return HttpNotFound();
            }
            return View(appointmentState);
        }

        // GET: AppointmentStates/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AppointmentStates/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AppointmentStateID,AppointmentStatus")] AppointmentState appointmentState)
        {
            if (ModelState.IsValid)
            {
                db.AppointmentStates.Add(appointmentState);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(appointmentState);
        }

        // GET: AppointmentStates/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AppointmentState appointmentState = db.AppointmentStates.Find(id);
            if (appointmentState == null)
            {
                return HttpNotFound();
            }
            return View(appointmentState);
        }

        // POST: AppointmentStates/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AppointmentStateID,AppointmentStatus")] AppointmentState appointmentState)
        {
            if (ModelState.IsValid)
            {
                db.Entry(appointmentState).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(appointmentState);
        }

        // GET: AppointmentStates/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AppointmentState appointmentState = db.AppointmentStates.Find(id);
            if (appointmentState == null)
            {
                return HttpNotFound();
            }
            return View(appointmentState);
        }

        // POST: AppointmentStates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AppointmentState appointmentState = db.AppointmentStates.Find(id);
            db.AppointmentStates.Remove(appointmentState);
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
