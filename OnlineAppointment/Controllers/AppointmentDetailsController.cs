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
    public class AppointmentDetailsController : Controller
    {
        private OnlineAppointmentContext db = new OnlineAppointmentContext();

        // GET: AppointmentDetails
        public ActionResult Index(int id)
        {
            Session["AppointmentId"] = id;
            var appointmentDetails = db.AppointmentDetails.Include(a => a.Appointment).Include(a => a.Product).Where(a => a.AppointmentId == id);
            return View(appointmentDetails.ToList());
        }

        // GET: AppointmentDetails/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AppointmentDetail appointmentDetail = db.AppointmentDetails.Find(id);
            if (appointmentDetail == null)
            {
                return HttpNotFound();
            }
            return View(appointmentDetail);
        }

        // GET: AppointmentDetails/Create
        public ActionResult Create(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ViewBag.ProductID = new SelectList(db.Products.Where(a=> a.ProductTypeID != 4), "ProductID", "ProductName");
            return View();
        }
        decimal? newD;
        // POST: AppointmentDetails/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,AppointmentId,ProductID,Quantity,Discount")] AppointmentDetail appointmentDetail, int? id)
        {
            
            if (ModelState.IsValid)
            {
             
                var product = db.Products.Where(p => p.ProductID == appointmentDetail.ProductID).First<Product>();
                var _checkProduct = db.AppointmentDetails.Where(p => p.AppointmentId == id && p.ProductID == appointmentDetail.ProductID).ToList<AppointmentDetail>();
                if (appointmentDetail.Discount == null)
                {
                    appointmentDetail.Discount = 0;
                }
                else
                {
                    this.newD = appointmentDetail.Discount;
                }
                if (_checkProduct.Count <= 0)
                {
                   
                    appointmentDetail.Price = product.ProductPrice;
                    appointmentDetail.Total = (product.ProductPrice * appointmentDetail.Quantity)-(product.ProductPrice*appointmentDetail.Discount/100);
                    appointmentDetail.AppointmentId = id;
                    db.AppointmentDetails.Add(appointmentDetail);
                    db.SaveChanges();
                }
                else
                {
                    decimal dtot = newD ?? 0;
                    //db.AppointmentDetails.Attach(appointmentDetail);
                    //db.Entry(appointmentDetail.Discount).CurrentValues.SetValues(appointmentDetail.Discount);
                    var _selectedProduct = db.AppointmentDetails.Where(p => p.AppointmentId == id && p.ProductID == appointmentDetail.ProductID).First<AppointmentDetail>();
                    _selectedProduct.Quantity = _selectedProduct.Quantity + appointmentDetail.Quantity;
                    _selectedProduct.Total = (_selectedProduct.Price * _selectedProduct.Quantity) - (_selectedProduct.Quantity*_selectedProduct.Price * dtot/100);
                    _selectedProduct.Discount = newD;
                    
                    db.SaveChanges();
                }
                
                return RedirectToAction("Index", new { id = id });
            }
            
            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "ProductName", appointmentDetail.ProductID);
            return View(appointmentDetail);
        }

        // GET: AppointmentDetails/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AppointmentDetail appointmentDetail = db.AppointmentDetails.Find(id);
            if (appointmentDetail == null)
            {
                return HttpNotFound();
            }
            ViewBag.AppointmentId = new SelectList(db.Appointments, "AppointmentId", "Reason", appointmentDetail.AppointmentId);
            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "ProductName", appointmentDetail.ProductID);
            return View(appointmentDetail);
        }

        // POST: AppointmentDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,AppointmentId,ProductID,Quantity,Price,Discount,Total")] AppointmentDetail appointmentDetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(appointmentDetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AppointmentId = new SelectList(db.Appointments, "AppointmentId", "Reason", appointmentDetail.AppointmentId);
            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "ProductName", appointmentDetail.ProductID);
            return View(appointmentDetail);
        }

        // GET: AppointmentDetails/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AppointmentDetail appointmentDetail = db.AppointmentDetails.Find(id);
            if (appointmentDetail == null)
            {
                return HttpNotFound();
            }
            return View(appointmentDetail);
        }

        // POST: AppointmentDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AppointmentDetail appointmentDetail = db.AppointmentDetails.Find(id);
            db.AppointmentDetails.Remove(appointmentDetail);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Remove(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AppointmentDetail appointmentDetail = db.AppointmentDetails.Find(id);
            if (appointmentDetail == null)
            {
                return HttpNotFound();
            }
            else
            {
                db.AppointmentDetails.Remove(appointmentDetail);
                db.SaveChanges();
                return RedirectToAction("Index", new { id = Session["AppointmentId"] });
            }
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
