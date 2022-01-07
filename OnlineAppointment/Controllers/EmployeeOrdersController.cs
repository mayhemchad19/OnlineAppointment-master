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
    public class EmployeeOrdersController : Controller
    {
        private OnlineAppointmentContext db = new OnlineAppointmentContext();

        // GET: EmployeeOrders
        public ActionResult Index()
        {
            var orders = db.Orders.Include(o => o.DiscountType).Include(o => o.PaymentType).Include(o => o.User);
            return View(orders.ToList());
        }

        // GET: EmployeeOrders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // GET: EmployeeOrders/Create
        public ActionResult Create()
        {
            ViewBag.DiscountTypeID = new SelectList(db.DiscountTypes, "DiscountTypeID", "DiscountTypeName");
            ViewBag.PaymentTypeID = new SelectList(db.PaymentTypes, "PaymentTypeID", "PaymentTypeName");
            ViewBag.UserID = new SelectList(db.Users, "UserID", "Username");
            return View();
        }

        // POST: EmployeeOrders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OrderID,PaymentTypeID,UserID,OrderNumber,OrderDate,DiscountTypeID,FinalTotal,DiscountedTotal")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Orders.Add(order);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DiscountTypeID = new SelectList(db.DiscountTypes, "DiscountTypeID", "DiscountTypeName", order.DiscountTypeID);
            ViewBag.PaymentTypeID = new SelectList(db.PaymentTypes, "PaymentTypeID", "PaymentTypeName", order.PaymentTypeID);
            ViewBag.UserID = new SelectList(db.Users, "UserID", "Username", order.UserID);
            return View(order);
        }

        // GET: EmployeeOrders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.DiscountTypeID = new SelectList(db.DiscountTypes, "DiscountTypeID", "DiscountTypeName", order.DiscountTypeID);
            ViewBag.PaymentTypeID = new SelectList(db.PaymentTypes, "PaymentTypeID", "PaymentTypeName", order.PaymentTypeID);
            ViewBag.UserID = new SelectList(db.Users, "UserID", "Username", order.UserID);
            return View(order);
        }

        // POST: EmployeeOrders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrderID,PaymentTypeID,UserID,OrderNumber,OrderDate,DiscountTypeID,FinalTotal,DiscountedTotal")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DiscountTypeID = new SelectList(db.DiscountTypes, "DiscountTypeID", "DiscountTypeName", order.DiscountTypeID);
            ViewBag.PaymentTypeID = new SelectList(db.PaymentTypes, "PaymentTypeID", "PaymentTypeName", order.PaymentTypeID);
            ViewBag.UserID = new SelectList(db.Users, "UserID", "Username", order.UserID);
            return View(order);
        }

        // GET: EmployeeOrders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: EmployeeOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
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
