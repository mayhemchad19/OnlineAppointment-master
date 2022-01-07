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
    public class CustomerSalesController : Controller
    {
        private OnlineAppointmentContext db = new OnlineAppointmentContext();

        public Boolean CheckSession()
        {
            if (Session["id"] == null)
            {
                Session["id"] = 0;
            }

            if (int.Parse(Session["id"].ToString()) != 4)
            {
                return false;
            }

            else
            {
                Session.Timeout = 60;
                return true;
            }
        }

        // GET: CustomerSales
        public ActionResult Index()
        {




            if (CheckSession())
            {
                var uID = int.Parse(Session["UserID"].ToString());


                //return View(users.Where(u => u.UserID == uID).ToList());
                // FUNCTIONS
                var sales = db.Sales.Include(s => s.DiscountType).Include(s => s.PaymentType).Include(s => s.User);
                return View(sales.Where(u => u.UserID == uID).ToList());

            }
            else return RedirectToAction("Login", "Logs");

            //var sales = db.Sales.Include(s => s.DiscountType).Include(s => s.PaymentType).Include(s => s.User);
            //return View(sales.ToList());
        }

        // GET: CustomerSales/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sale sale = db.Sales.Find(id);
            if (sale == null)
            {
                return HttpNotFound();
            }
            Session["SaleID"] = id;

            return RedirectToAction("Index","CustomerSaleDetails");
        }

        // GET: CustomerSales/Create
        public ActionResult Create()
        {
            ViewBag.DiscountTypeID = new SelectList(db.DiscountTypes, "DiscountTypeID", "DiscountTypeName");
            ViewBag.PaymentTypeID = new SelectList(db.PaymentTypes, "PaymentTypeID", "PaymentTypeName");
            ViewBag.UserID = new SelectList(db.Users, "UserID", "Username");
            return View();
        }

        // POST: CustomerSales/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SaleID,PaymentTypeID,UserID,OrderNumber,OrderDate,DiscountTypeID,FinalTotal,DiscountedTotal,isPaid")] Sale sale)
        {
            if (ModelState.IsValid)
            {
                db.Sales.Add(sale);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DiscountTypeID = new SelectList(db.DiscountTypes, "DiscountTypeID", "DiscountTypeName", sale.DiscountTypeID);
            ViewBag.PaymentTypeID = new SelectList(db.PaymentTypes, "PaymentTypeID", "PaymentTypeName", sale.PaymentTypeID);
            ViewBag.UserID = new SelectList(db.Users, "UserID", "Username", sale.UserID);
            return View(sale);
        }

        // GET: CustomerSales/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sale sale = db.Sales.Find(id);
            if (sale == null)
            {
                return HttpNotFound();
            }
            ViewBag.DiscountTypeID = new SelectList(db.DiscountTypes, "DiscountTypeID", "DiscountTypeName", sale.DiscountTypeID);
            ViewBag.PaymentTypeID = new SelectList(db.PaymentTypes, "PaymentTypeID", "PaymentTypeName", sale.PaymentTypeID);
            ViewBag.UserID = new SelectList(db.Users, "UserID", "Username", sale.UserID);
            return View(sale);
        }

        // POST: CustomerSales/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SaleID,PaymentTypeID,UserID,OrderNumber,OrderDate,DiscountTypeID,FinalTotal,DiscountedTotal,isPaid")] Sale sale)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sale).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DiscountTypeID = new SelectList(db.DiscountTypes, "DiscountTypeID", "DiscountTypeName", sale.DiscountTypeID);
            ViewBag.PaymentTypeID = new SelectList(db.PaymentTypes, "PaymentTypeID", "PaymentTypeName", sale.PaymentTypeID);
            ViewBag.UserID = new SelectList(db.Users, "UserID", "Username", sale.UserID);
            return View(sale);
        }

        // GET: CustomerSales/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sale sale = db.Sales.Find(id);
            if (sale == null)
            {
                return HttpNotFound();
            }
            return View(sale);
        }

        // POST: CustomerSales/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Sale sale = db.Sales.Find(id);
            db.Sales.Remove(sale);
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
