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
    public class DiscountTypesController : Controller
    {
        private OnlineAppointmentContext db = new OnlineAppointmentContext();

        // GET: DiscountTypes
        public ActionResult Index()
        {
            return View(db.DiscountTypes.ToList());
        }

        // GET: DiscountTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DiscountType discountType = db.DiscountTypes.Find(id);
            if (discountType == null)
            {
                return HttpNotFound();
            }
            return View(discountType);
        }

        // GET: DiscountTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DiscountTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DiscountTypeID,DiscountTypeName,DiscountAmount,DiscountTypeStatus")] DiscountType discountType)
        {
            if (ModelState.IsValid)
            {
                db.DiscountTypes.Add(discountType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(discountType);
        }

        // GET: DiscountTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DiscountType discountType = db.DiscountTypes.Find(id);
            if (discountType == null)
            {
                return HttpNotFound();
            }
            return View(discountType);
        }

        // POST: DiscountTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DiscountTypeID,DiscountTypeName,DiscountAmount,DiscountTypeStatus")] DiscountType discountType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(discountType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(discountType);
        }

        // GET: DiscountTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DiscountType discountType = db.DiscountTypes.Find(id);
            if (discountType == null)
            {
                return HttpNotFound();
            }
            return View(discountType);
        }

        // POST: DiscountTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DiscountType discountType = db.DiscountTypes.Find(id);
            db.DiscountTypes.Remove(discountType);
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
