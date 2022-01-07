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
    public class SaleDetailsController : Controller
    {
        private OnlineAppointmentContext db = new OnlineAppointmentContext();

        //public DateTime? pStart;
        //public DateTime? pEnd;
        // GET: SaleDetails
        public ActionResult Index()
        {
            var sID = int.Parse(Session["SalesID"].ToString());

            var saleDetails = db.SaleDetails.Include(s => s.Product).Include(s => s.Sale);
            return View(saleDetails.Where(s=>s.SaleID==sID).ToList());
        }

        //--------------------------------------------------------------------


        public ActionResult SalesSummary(DateTime? start, DateTime? end)
        {
            Session["Duration"] = " ";

            ViewBag.Start = start.ToString();
            ViewBag.End = end.ToString();
            var saleDetails = db.SaleDetails.Include(s => s.Product).Include(s => s.Sale).Include(s=>s.Sale.User);
            //return View(sales.OrderByDescending(s => s.OrderDate).ToList());
            var st = start;
            var e = end;



            if (start != null)
            {


                return View(saleDetails.Where(s => s.Sale.OrderDate <= e && s.Sale.OrderDate >= st && s.Quantity!=0).OrderBy(s => s.SaleID).ToList());
            }


           
            return View(saleDetails.Where(s=>s.Quantity != 0).ToList());
        }



        public ActionResult Today()
        {
            var saleDetails = db.SaleDetails.Include(s => s.Product).Include(s => s.Sale).Include(s => s.Sale.User);
            //return View(sales.Where(s=> s.OrderDate == DateTime.Now).ToList());
            var start = DateTime.Today;
            var end = DateTime.Now;


            //this.pStart = start;
            //this.pEnd = end;

            Session["SStart"] = start;
            Session["SEnd"] = end;
            Session["Duration"] = "Today";
            ;
            return RedirectToAction("SalesSummary", new
            {
                start,
                end
            });

        }
        public ActionResult recentWeek()
        {
            var start = DateTime.Now.AddDays(-7);
            var saleDetails = db.SaleDetails.Include(s => s.Product).Include(s => s.Sale).Include(s => s.Sale.User);
            //return View(sales.Where(s=> s.OrderDate <= DateTime.Now && s.OrderDate >= lastWeek).OrderBy(s => s.SaleID).ToList());
            var end = DateTime.Now;
            //this.pStart = start;
            //this.pEnd = end;
            Session["SStart"] = start;
            Session["SEnd"] = end;
            Session["Duration"] = "Last 7 days";
            return RedirectToAction("SalesSummary", new
            {
                start,
                end
            });

        }


        public ActionResult recentMonth()
        {
            var start = DateTime.Now.AddMonths(-1);
            var end = DateTime.Now;
            var saleDetails = db.SaleDetails.Include(s => s.Product).Include(s => s.Sale).Include(s => s.Sale.User);
            //this.pStart = start;
            //this.pEnd = end;
            Session["SStart"] = start;
            Session["SEnd"] = end;
            Session["Duration"] = "Last 30 days";
            //return View(sales.Where(s => s.OrderDate <= DateTime.Now && s.OrderDate >= lastMonth).OrderBy(s=>s.SaleID).ToList());
            return RedirectToAction("SalesSummary", new
            {
                start,
                end
            });
        }

        public ActionResult recentYear()
        {
            var start = DateTime.Now.AddYears(-1);
            var end = DateTime.Now;
            var saleDetails = db.SaleDetails.Include(s => s.Product).Include(s => s.Sale).Include(s => s.Sale.User);
            //this.pStart = start;
            //this.pEnd = end;
            Session["SStart"] = start;
            Session["SEnd"] = start;
            Session["Duration"] = "Last 365 days";
            //return View(sales.Where(s => s.OrderDate <= DateTime.Now && s.OrderDate >= lastYear).OrderBy(s => s.SaleID).ToList());

            return RedirectToAction("SalesSummary", new
            {
                start,
                end
            });
        }


        public ActionResult Search(DateTime? start, DateTime? end)
        {
            ViewBag.Start = start.ToString();
            ViewBag.End = end.ToString();
            var sDate = start;
            var eDate = end;
            //this.pStart = sDate;
            //this.pEnd = eDate;

            Session["SStart"] = sDate;
            Session["SEnd"] = eDate;
            //if(end!= null)
            //  {
            //      end = end.AddDays(1);
            //  }
            var saleDetails = db.SaleDetails.Include(s => s.Product).Include(s => s.Sale).Include(s => s.Sale.User);
            //return View(sales.Where(s => s.OrderDate >= start && s.OrderDate <= end).OrderBy(s => s.SaleID).ToList());
            if (start != null || end != null)
            {
                return RedirectToAction("SalesSummary", new
                {
                    start,
                    end
                });

            }

            return RedirectToAction("SalesSummary");

        }



        //--------------------------------------------------------------------



        // GET: SaleDetails/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SaleDetail saleDetail = db.SaleDetails.Find(id);
            if (saleDetail == null)
            {
                return HttpNotFound();
            }
            return View(saleDetail);
        }

        // GET: SaleDetails/Create
        public ActionResult Create()
        {
            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "ProductName");
            ViewBag.SaleID = new SelectList(db.Sales, "SaleID", "OrderNumber");
            return View();
        }

        // POST: SaleDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OrderDetailID,SaleID,ProductID,UnitPrice,Quantity,Discount,Total")] SaleDetail saleDetail)
        {
            if (ModelState.IsValid)
            {
                db.SaleDetails.Add(saleDetail);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "ProductName", saleDetail.ProductID);
            ViewBag.SaleID = new SelectList(db.Sales, "SaleID", "OrderNumber", saleDetail.SaleID);
            return View(saleDetail);
        }

        // GET: SaleDetails/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SaleDetail saleDetail = db.SaleDetails.Find(id);
            if (saleDetail == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "ProductName", saleDetail.ProductID);
            ViewBag.SaleID = new SelectList(db.Sales, "SaleID", "OrderNumber", saleDetail.SaleID);
            return View(saleDetail);
        }

        // POST: SaleDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrderDetailID,SaleID,ProductID,UnitPrice,Quantity,Discount,Total")] SaleDetail saleDetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(saleDetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "ProductName", saleDetail.ProductID);
            ViewBag.SaleID = new SelectList(db.Sales, "SaleID", "OrderNumber", saleDetail.SaleID);
            return View(saleDetail);
        }

        // GET: SaleDetails/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SaleDetail saleDetail = db.SaleDetails.Find(id);
            if (saleDetail == null)
            {
                return HttpNotFound();
            }
            return View(saleDetail);
        }

        // POST: SaleDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SaleDetail saleDetail = db.SaleDetails.Find(id);
            db.SaleDetails.Remove(saleDetail);
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



        public ActionResult Report(SaleSummaryViewMOdel sale)
        {
            SaleSummaryReport salesReport = new SaleSummaryReport();
            byte[] abytes = salesReport.PrepareSalesReport(GetSaleDetails());
            return File(abytes, "application/pdf");
        }
        //sa kabila yan sir
        //public decimal totaltotal = 0;
        public List<SaleSummaryViewMOdel> GetSaleDetails()
        {
           
            //var checkSpecific = (from u in db.Appointments
            //                     where u.AppointmentDate == appointment.AppointmentDate && u.SlotID == appointment.SlotID && u.UserID == appointment.UserID
            //                     select u).ToList();

           var countItem = (from x in db.SaleDetails select x).ToList();
        
            List<SaleSummaryViewMOdel> sales = new List<SaleSummaryViewMOdel>();
            SaleSummaryViewMOdel sale = new SaleSummaryViewMOdel();
            decimal? totalcount = 0;
            for (int i = 1; i <= countItem.Count; i++)
            {
                var sID = (from x in db.SaleDetails where x.OrderDetailID == i select x.SaleID).FirstOrDefault();
                //var status = (from x in db.Sales where x.SaleID == i select x.SaleStatus).FirstOrDefault();
             
                //var discount = (from x in db.Sales where x.SaleID == i select x.DiscountTypeID).FirstOrDefault();
                //var subtotal = (from x in db.Sales where x.SaleID == i select x.FinalTotal).FirstOrDefault();
                //var disctotal = (from x in db.Sales where x.SaleID == i select x.DiscountedTotal).FirstOrDefault();

                var fname = (from x in db.Sales.Include("User") where x.SaleID == sID select x.User.FirstName).FirstOrDefault();
                var lname = (from x in db.Sales.Include("User") where x.SaleID == sID select x.User.LastName).FirstOrDefault();
                var customer = (from x in db.SaleDetails.Include("Sale") where x.SaleID == sID select x).FirstOrDefault();
                var fullname = fname + " " + lname;
                var ordernumber = (from x in db.SaleDetails.Include("Sale") where x.SaleID == sID select x).FirstOrDefault();
                var product = (from x in db.SaleDetails where x.OrderDetailID == i select x.Product.ProductName).FirstOrDefault();
                var UnitPrice = (from x in db.SaleDetails where x.OrderDetailID == i select x.Product.ProductPrice).FirstOrDefault();
                var quantity = (from x in db.SaleDetails where x.OrderDetailID == i select x.Quantity).FirstOrDefault();
                var idiscount = (from x in db.SaleDetails where x.OrderDetailID == i select x.Discount).FirstOrDefault();
                var date = (from x in db.SaleDetails.Include("Sale") where x.SaleID == sID select x).FirstOrDefault();
                var s_ale= (from x in db.SaleDetails.Include("Sale") where x.SaleID == sID select x).FirstOrDefault();
                var total = (from x in db.SaleDetails where x.OrderDetailID == i select x.Total).FirstOrDefault();
                //}
                //if (ordernumber == null)
                //{
                //    continue;
                //}
                sale.totaltotal = 1;
                if (quantity == 0)
                {
                    continue;
                }
                //sale.totaltotal = 1;
                sale = new SaleSummaryViewMOdel();
                sale.SaleNumber = ordernumber.Sale.OrderNumber;
                sale.Customer = fullname;
                sale.Product = product;
                sale.UnitPrice = UnitPrice;
                sale.Quantity = quantity;
                sale.Discount = idiscount;
                sale.Total =  total;
                var ate = date.Sale.OrderDate;
           
                sale.Date = ate;
                totalcount = sale.totaltotal + sale.Total;
                sale.totaltotal += totalcount;
                sales.Add(sale);
      

            }
          
            if (Session["SStart"] != null)
            {
               
                var st = (DateTime)Session["SStart"];
                var e = (DateTime)Session["SEnd"];

                return sales.Where(s => s.Date <= e && s.Date>= st && s.Quantity != 0).ToList();
            }
            Session["SStart"] = null;
            Session["SEnd"] = null;
            Session["Duration"] = " ";
            return sales.Where(p=>p.Quantity != 0).ToList();

         
        }
    }
}
