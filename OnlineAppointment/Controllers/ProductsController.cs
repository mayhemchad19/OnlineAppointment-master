using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using OnlineAppointment.Models;

namespace OnlineAppointment.Controllers
{
    public class ProductsController : Controller
    {
        private OnlineAppointmentContext db = new OnlineAppointmentContext();

        // GET: Products
        public ActionResult Index()
        {
            var products = db.Products.Include(p => p.ProductType);
            return View(db.Products.Where(p => p.ProductStatus == true && p.ProductTypeID != 4).OrderBy(p=>p.ProductTypeID + p.ProductPrice).ToList());
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


        // GET: Products/Details/5
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

        // GET: Products/Create
        public ActionResult Create()
        {
            ViewBag.ProductTypeID = new SelectList(db.ProductTypes , "ProductTypeID", "ProductTypeName");
            //ViewBag.ProductTypeID = new SelectList(db.ProductTypes.Where(p => p.ProductTypeID != 3), "ProductTypeID", "ProductTypeName");
            return View();
        }

        // POST: Products/Create
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
                //    ModelState.AddModelError("ProductName", "Product/Service has already been deleted. ");
                //    return View(product);

                //}

                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProductTypeID = new SelectList(db.ProductTypes, "ProductTypeID", "ProductTypeName", product.ProductTypeID);
            return View(product);
        }

        // GET: Products/Edit/5
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

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductID,ProductName,ProductPrice,ProductTypeID,ProductDescription,ProductStatus")] Product product)
        {
            if (ModelState.IsValid)
            {

                //var checkProduct = db.Products.Any(x => x.ProductName == product.ProductName && x.ProductStatus == true);
                //if (checkProduct)
                //{
                //    ViewBag.ProductTypeID = new SelectList(db.ProductTypes, "ProductTypeID", "ProductTypeName", product.ProductTypeID);
                //    ModelState.AddModelError("ProductName", "Product/Service already exists");
                //    return View(product);

                //}



                product.ProductStatus = true;
                db.Entry(product).State = EntityState.Modified;

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProductTypeID = new SelectList(db.ProductTypes, "ProductTypeID", "ProductTypeName", product.ProductTypeID);
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            Appointment appointment = new Appointment();
            Product product1 = new Product();
            var checkProduct = (from u in db.Appointments
                                 where u.AppointmentStateID == 1 && u.ProductID == id || u.AppointmentStateID == 3 && u.ProductID == id
                                select u).ToList();

            ViewBag.Message = "Are you sure you want to delete this ?";
            if (checkProduct.Count >=1)
            {
                ViewBag.Message = "This product is still being used. Do you still want to DELETE this?";
            }


         

      
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

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);

            //var checkProduct = db.Products.Any(x => x.ProductName == product.ProductName && x.ProductStatus == true);
            //if (checkProduct)
            //{
            //    ViewBag.ProductTypeID = new SelectList(db.ProductTypes, "ProductTypeID", "ProductTypeName", product.ProductTypeID);
            //    ModelState.AddModelError("ProductName", "Product/Service already exists");
            //    return View(product);

            //}




            product.ProductStatus = false;
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
