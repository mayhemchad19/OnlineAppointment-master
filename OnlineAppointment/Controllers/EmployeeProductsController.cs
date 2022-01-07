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
    public class EmployeeProductsController : Controller
    {
        private OnlineAppointmentContext db = new OnlineAppointmentContext();

        // GET: EmployeeProducts
        public ActionResult Index()
        {
            var products = db.Products.Include(p => p.ProductType);
            return View(products.Where(a=> a.ProductStatus ==true && a.ProductTypeID !=4).OrderBy(p=>p.ProductTypeID).ThenBy(c=>c.ProductPrice).ToList());
        }

        // GET: EmployeeProducts/Details/5
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

        // GET: EmployeeProducts/Create
        public ActionResult Create()
        {
            ViewBag.ProductTypeID = new SelectList(db.ProductTypes, "ProductTypeID", "ProductTypeName");
            return View();
        }

        // POST: EmployeeProducts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductID,ProductName,ProductPrice,ProductTypeID,ProductDescription,ProductStatus")] Product product)
        {
            if (ModelState.IsValid)
            {
                product.ProductStatus = true;

                var checkProduct = db.Products.Any(x => x.ProductName == product.ProductName && x.ProductStatus == true);
                if (checkProduct)
                {
                    ViewBag.ProductTypeID = new SelectList(db.ProductTypes, "ProductTypeID", "ProductTypeName", product.ProductTypeID);
                    ModelState.AddModelError("ProductName", "Product/Service already exists");
                    return View(product);

                }
                //var checkdeletedProduct = db.Products.Any(x => x.ProductName == product.ProductName && x.ProductStatus == false);
                //if (checkdeletedProduct)
                //{
                //    ViewBag.ProductTypeID = new SelectList(db.ProductTypes, "ProductTypeID", "ProductTypeName", product.ProductTypeID);
                //    ModelState.AddModelError("ProductName", "Product/Service has already been deleted. Please contact the Admin to Restore.");
                //    return View(product);

                //}

                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProductTypeID = new SelectList(db.ProductTypes, "ProductTypeID", "ProductTypeName", product.ProductTypeID);
            return View(product);
        }

        // GET: EmployeeProducts/Edit/5
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

        // POST: EmployeeProducts/Edit/5
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

        // GET: EmployeeProducts/Delete/5
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

        // POST: EmployeeProducts/Delete/5
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
