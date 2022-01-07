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
    public class CustomerProductsController : Controller
    {
        private OnlineAppointmentContext db = new OnlineAppointmentContext();

        // GET: CustomerEmployeeProducts
        public ActionResult Index()
        {
            var products = db.Products.Include(p => p.ProductType);
            return View(db.Products.Where(p => p.ProductStatus == true && p.ProductTypeID != 4).OrderBy(p => p.ProductTypeID + p.ProductPrice).ToList());
        }


        public ActionResult ProductList()
        {
            var products = db.Products.Include(p => p.ProductType);
            return View(db.Products.Where(p => p.ProductStatus == true && p.ProductTypeID == 1).OrderBy(p => p.ProductTypeID + p.ProductPrice).ToList());
        }

        public ActionResult ServiceList()
        {
            var products = db.Products.Include(p => p.ProductType);
            return View(db.Products.Where(p => p.ProductStatus == true && p.ProductTypeID == 2).OrderBy(p => p.ProductTypeID + p.ProductPrice).ToList());
        }
        // GET: CustomerEmployeeProducts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: CustomerEmployeeProducts/Create
        public ActionResult Create()
        {
            ViewBag.ProductTypeID = new SelectList(db.ProductTypes, "ProductTypeID", "ProductTypeName");
            return View();
        }

        // POST: CustomerEmployeeProducts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductID,ProductName,ProductPrice,ProductTypeID,ProductDescription,ProductStatus")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProductTypeID = new SelectList(db.ProductTypes, "ProductTypeID", "ProductTypeName", product.ProductTypeID);
            return View(product);
        }

        // GET: CustomerEmployeeProducts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductTypeID = new SelectList(db.ProductTypes, "ProductTypeID", "ProductTypeName", product.ProductTypeID);
            return View(product);
        }

        // POST: CustomerEmployeeProducts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductID,ProductName,ProductPrice,ProductTypeID,ProductDescription,ProductStatus")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProductTypeID = new SelectList(db.ProductTypes, "ProductTypeID", "ProductTypeName", product.ProductTypeID);
            return View(product);
        }

        // GET: CustomerEmployeeProducts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: CustomerEmployeeProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
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
