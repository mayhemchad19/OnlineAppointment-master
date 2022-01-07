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
    public class CustomerAppointmentTestController : Controller
    {
        private OnlineAppointmentContext db = new OnlineAppointmentContext();

        // GET: CustomerAppointmentTest
        public ActionResult Index()
        {
            var appointments = db.Appointments.Include(a => a.AppointmentState).Include(a => a.Product).Include(a => a.Slot).Include(a => a.User);
            return View(appointments.ToList());
        }

        // GET: CustomerAppointmentTest/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appointment appointment = db.Appointments.Find(id);
            if (appointment == null)
            {
                return HttpNotFound();
            }
            return View(appointment);
        }

        // GET: CustomerAppointmentTest/Create
        public ActionResult Create()
        {
            ViewBag.AppointmentStateID = new SelectList(db.AppointmentStates, "AppointmentStateID", "AppointmentStatus");
            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "ProductName");
            ViewBag.SlotID = new SelectList(db.Slots, "SlotsID", "Time");
            ViewBag.UserID = new SelectList(db.Users, "UserID", "Username");
            return View();
        }

        // POST: CustomerAppointmentTest/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AppointmentId,UserID,ProductID,AppointmentDate,SlotID,Reason,AppointmentStateID")] Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                db.Appointments.Add(appointment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AppointmentStateID = new SelectList(db.AppointmentStates, "AppointmentStateID", "AppointmentStatus", appointment.AppointmentStateID);
            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "ProductName", appointment.ProductID);
            ViewBag.SlotID = new SelectList(db.Slots, "SlotsID", "Time", appointment.SlotID);
            ViewBag.UserID = new SelectList(db.Users, "UserID", "Username", appointment.UserID);
            return View(appointment);
        }

        // GET: CustomerAppointmentTest/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appointment appointment = db.Appointments.Find(id);
            if (appointment == null)
            {
                return HttpNotFound();
            }
            ViewBag.AppointmentStateID = new SelectList(db.AppointmentStates, "AppointmentStateID", "AppointmentStatus", appointment.AppointmentStateID);
            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "ProductName", appointment.ProductID);
            ViewBag.SlotID = new SelectList(db.Slots, "SlotsID", "Time", appointment.SlotID);
            ViewBag.UserID = new SelectList(db.Users, "UserID", "Username", appointment.UserID);
            return View(appointment);
        }

        // POST: CustomerAppointmentTest/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AppointmentId,UserID,ProductID,AppointmentDate,SlotID,Reason,AppointmentStateID")] Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(appointment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AppointmentStateID = new SelectList(db.AppointmentStates, "AppointmentStateID", "AppointmentStatus", appointment.AppointmentStateID);
            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "ProductName", appointment.ProductID);
            ViewBag.SlotID = new SelectList(db.Slots, "SlotsID", "Time", appointment.SlotID);
            ViewBag.UserID = new SelectList(db.Users, "UserID", "Username", appointment.UserID);
            return View(appointment);
        }

        // GET: CustomerAppointmentTest/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appointment appointment = db.Appointments.Find(id);
            if (appointment == null)
            {
                return HttpNotFound();
            }
            return View(appointment);
        }

        // POST: CustomerAppointmentTest/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Appointment appointment = db.Appointments.Find(id);
            db.Appointments.Remove(appointment);
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
