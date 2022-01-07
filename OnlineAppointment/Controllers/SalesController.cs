using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OnlineAppointment.Models;
using OnlineAppointment.Report;

namespace OnlineAppointment.Controllers
{
    public class SalesController : Controller
    {
        private OnlineAppointmentContext db = new OnlineAppointmentContext();

        // GET: Sales
        public ActionResult Index(DateTime? start, DateTime? end)
        {
  


            ViewBag.Start = start.ToString();
            ViewBag.End = end.ToString();
            var sales = db.Sales.Include(s => s.DiscountType).Include(s => s.PaymentType).Include(s => s.User);
            //return View(sales.OrderByDescending(s => s.OrderDate).ToList());
            var st = start;
            var e = end;

            if (start != null)
            {
                //var sales = db.Sales.Include(s => s.DiscountType).Include(s => s.PaymentType).Include(s => s.User);
                //return View(sales.OrderByDescending(s => s.OrderDate).ToList());

                return View(sales.Where(s => s.OrderDate <= e && s.OrderDate >= st).OrderBy(s => s.SaleID).ToList());
            }

            //return View(sales.Where(s => s.OrderDate <= st && s.OrderDate >= e).OrderBy(s => s.SaleID).ToList());

            return View(sales.Where(s=>s.SaleStatus != false).OrderByDescending(s => s.OrderDate).ToList());

        }
        public ActionResult Today()
        {
            var sales = db.Sales.Include(s => s.DiscountType).Include(s => s.PaymentType).Include(s => s.User);
            //return View(sales.Where(s=> s.OrderDate == DateTime.Now).ToList());
            var start = DateTime.Today;
            var end = DateTime.Now;
            return RedirectToAction("Index", new
            {
                start,
                end
            });

        }
        public ActionResult recentWeek()
        {
            var start = DateTime.Now.AddDays(-7);
            var sales = db.Sales.Include(s => s.DiscountType).Include(s => s.PaymentType).Include(s => s.User);
            //return View(sales.Where(s=> s.OrderDate <= DateTime.Now && s.OrderDate >= lastWeek).OrderBy(s => s.SaleID).ToList());
            var end = DateTime.Now;
            return RedirectToAction("Index", new
            {
                start,
                end
            });

        }


        public ActionResult recentMonth()
        {
            var start = DateTime.Now.AddMonths(-1);
            var end = DateTime.Now;
            var sales = db.Sales.Include(s => s.DiscountType).Include(s => s.PaymentType).Include(s => s.User);
            //return View(sales.Where(s => s.OrderDate <= DateTime.Now && s.OrderDate >= lastMonth).OrderBy(s=>s.SaleID).ToList());
            return RedirectToAction("Index", new
            {
                start,
                end
            });
        }

        public ActionResult recentYear()
        {
            var start = DateTime.Now.AddYears(-1);
            var end = DateTime.Now;
            var sales = db.Sales.Include(s => s.DiscountType).Include(s => s.PaymentType).Include(s => s.User);
            //return View(sales.Where(s => s.OrderDate <= DateTime.Now && s.OrderDate >= lastYear).OrderBy(s => s.SaleID).ToList());

            return RedirectToAction("Index", new
            {
                start,
                end
            });
        }
        public ActionResult SalesList()
        {
            var sales = db.Sales.Include(s => s.DiscountType).Include(s => s.PaymentType).Include(s => s.User);
            return View(sales.Where(s=> s.SaleStatus != false).OrderByDescending(s => s.OrderDate).ToList());
        }
        //public ActionResult Search()
        //{
        //    var sales = db.Sales.Include(s => s.DiscountType).Include(s => s.PaymentType).Include(s => s.User);
        //    return View(sales.OrderByDescending(s => s.OrderDate).ToList());
        //}
        //[HttpPost]
        public ActionResult Search(DateTime? start, DateTime? end)
        {
            ViewBag.Start = start.ToString();
            ViewBag.End = end.ToString();
            var sDate = start;
            var eDate = end;
          //if(end!= null)
          //  {
          //      end = end.AddDays(1);
          //  }
            var sales = db.Sales.Include(s => s.DiscountType).Include(s => s.PaymentType).Include(s => s.User);
            //return View(sales.Where(s => s.OrderDate >= start && s.OrderDate <= end).OrderBy(s => s.SaleID).ToList());
            if (start != null||end != null)
            {
                return RedirectToAction("Index", new
                {
                    start,
                    end
                });
          
            }

            return RedirectToAction("Index");

        }

        // GET: Sales/Details/5
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
            Session["SalesID"] = id;

            return RedirectToAction("Index", "SaleDetails");
        }

        // GET: Sales/Create
        public ActionResult Create()
        {
            ViewBag.DiscountTypeID = new SelectList(db.DiscountTypes, "DiscountTypeID", "DiscountTypeName");
            ViewBag.PaymentTypeID = new SelectList(db.PaymentTypes, "PaymentTypeID", "PaymentTypeName");
            ViewBag.UserID = new SelectList(db.Users, "UserID", "Username");
            return View();
        }

        // POST: Sales/Create
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

        // GET: Sales/Edit/5
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

        // POST: Sales/Edit/5
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

        // GET: Sales/Delete/5
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

        // POST: Sales/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Sale sale = db.Sales.Find(id);
            sale.SaleStatus = false;
            //db.Sales.Remove(sale);
            db.SaveChanges();
            return RedirectToAction("SalesList");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }



        //public ActionResult Report(SaleSummaryViewMOdel sale)
        //{
        //    SalesReport salesReport = new SalesReport();
        //    byte[] abytes = salesReport.PrepareSalesReport(GetSaleDetails());
        //    return File(abytes, "application/pdf");
        //}
        ////sa kabila yan sir
        //public List<SaleSummaryViewMOdel> GetSaleDetails()
        //{
        //    List<SaleSummaryViewMOdel> sales = new List<SaleSummaryViewMOdel>();
        //    SaleSummaryViewMOdel sale = new SaleSummaryViewMOdel();
        //    for (int i = 1; i <= 10; i++)
        //    {
        //        var sID = (from x in db.Sales where x.SaleID == i select x.SaleID).FirstOrDefault();
        //        var status = (from x in db.Sales where x.SaleID == i select x.SaleStatus).FirstOrDefault();
        //        var ordernumber = (from x in db.Sales where x.SaleID == i select x.OrderNumber).FirstOrDefault();
        //        var date = (from x in db.Sales where x.SaleID == i select x.OrderDate).FirstOrDefault();
        //        var discount = (from x in db.Sales where x.SaleID == i select x.DiscountTypeID).FirstOrDefault();
        //        var subtotal = (from x in db.Sales where x.SaleID == i select x.FinalTotal).FirstOrDefault();
        //        var disctotal = (from x in db.Sales where x.SaleID == i select x.DiscountedTotal).FirstOrDefault();
             
        //        var name = (from x in db.Sales where x.SaleID == i select x.User.FirstName).FirstOrDefault();
        //        var customer = (from x in db.Sales.Include("User") where x.SaleID == i select x).FirstOrDefault();
        //        var discountType = (from x in db.Sales.Include("DiscountType") where x.SaleID == i select x).FirstOrDefault();
        //        var product = (from x in db.Sales.Include("Product") where x.SaleID == i select x).FirstOrDefault();
        //        if (status == false)
        //        {
        //            continue;
        //        }
        //        if (ordernumber == null)
        //        {
        //            continue;
        //        }
          
        //        sale = new SaleSummaryViewMOdel();
        //        sale.SaleNumber = ordernumber;
        //        sale.Customer = customer.User.FirstName + " " + customer.User.LastName; //problema to kasi ang variable na userid is integer the ang value na nilalagay mo is string incompatible types kailang  mo talaga ng view model sa reports mo
        //        sale.Date = date.ToString();
        //        sale.Product =
        //        //yung sinundan ko po kasing tuturial wala pong view model... may massuggest po ba kayong link? 
        //        sale.OrderNumber = ordernumber;
        //        sale.FinalTotal = subtotal;
        //        sale.Discount = discount;
        //        sale.DiscountedPrice = disctotal;
        //        sale.DiscountType = discountType.DiscountType.DiscountTypeName;
        //        //sale.User.FirstName = name;

        //        //saleDetail.Product.ProductName = itemname;
        //        sales.Add(sale);
        //        //eto sir kaso yung puro foreign key lang yung values

        //    }
        //    return sales;
        //}

    }
}
