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
    public class OrdersController : Controller
    {
        private OnlineAppointmentContext db = new OnlineAppointmentContext();

        // GET: Orders
        public ActionResult Index(DateTime? start, DateTime? end)
        {
            ViewBag.Start = start.ToString();
            ViewBag.End = end.ToString();
            var orders = db.Orders.Include(o => o.DiscountType).Include(o => o.PaymentType).Include(o => o.User);
            var st = start;
            var e = end;
            if (start != null)
            {
                //var sales = db.Sales.Include(s => s.DiscountType).Include(s => s.PaymentType).Include(s => s.User);
                //return View(sales.OrderByDescending(s => s.OrderDate).ToList());

                return View(orders.Where(s => s.OrderDate <= e && s.OrderDate >= st).OrderBy(s => s.OrderID).ToList());
            }
            //.Where(i => i.isPaid == true)
            return View(orders.Where(o=> o.OrderStatus !=false).OrderByDescending(s => s.OrderDate).ToList()) ;
        }

        //----------------
        public ActionResult Today()
        {
            var orders = db.Orders.Include(o => o.DiscountType).Include(o => o.PaymentType).Include(o => o.User);
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
            var orders = db.Orders.Include(o => o.DiscountType).Include(o => o.PaymentType).Include(o => o.User);
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
            var orders = db.Orders.Include(o => o.DiscountType).Include(o => o.PaymentType).Include(o => o.User);
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
            var orders = db.Orders.Include(o => o.DiscountType).Include(o => o.PaymentType).Include(o => o.User);
            //return View(sales.Where(s => s.OrderDate <= DateTime.Now && s.OrderDate >= lastYear).OrderBy(s => s.SaleID).ToList());

            return RedirectToAction("Index", new
            {
                start,
                end
            });
        }
        //---------------








        public ActionResult unPaidBill()
        {
            var orders = db.Orders.Include(o => o.DiscountType).Include(o => o.PaymentType).Include(o => o.User);
            return View(orders.ToList().Where(b => b.isPaid == false && b.OrderStatus != false)) ;
         
        }

        public ActionResult unsettledInvoice()
        {
            var orders = db.Orders.Include(o => o.DiscountType).Include(o => o.PaymentType).Include(o => o.User);
            //return View(orders.ToList().Where(b => b.isPaid == false));
            return PartialView(orders.ToList().Where(b => b.isPaid == false));
            /*    return View(orders.ToList())*/
            ;
        }

        // GET: Orders/Details/5
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

        // GET: Orders/Create
        public ActionResult Create()
        {
            ViewBag.DiscountTypeID = new SelectList(db.DiscountTypes, "DiscountTypeID", "DiscountTypeName");
            ViewBag.PaymentTypeID = new SelectList(db.PaymentTypes, "PaymentTypeID", "PaymentTypeName");
            ViewBag.UserID = new SelectList(db.Users, "UserID", "Username");
            return View();
        }

        // POST: Orders/Create
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

        // GET: Orders/Edit/5
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

        // POST: Orders/Edit/5
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

        // GET: Orders/Delete/5
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

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Orders.Find(id);
            order.OrderStatus = false;
            //db.Orders.Remove(order);

            db.SaveChanges();
            return RedirectToAction("unPaidBill");
        }


        public ActionResult Void(int? id)
        {


            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                Order order = db.Orders.Find(id);
                
                if (id == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    order.OrderStatus = false;
                    db.SaveChanges();
                    return RedirectToAction("unPaidBill");
                }
                ViewBag.DiscountTypeID = new SelectList(db.DiscountTypes, "DiscountTypeID", "DiscountTypeName", order.DiscountTypeID);
                ViewBag.PaymentTypeID = new SelectList(db.PaymentTypes, "PaymentTypeID", "PaymentTypeName", order.PaymentTypeID);
                ViewBag.UserID = new SelectList(db.Users, "UserID", "Username", order.UserID);
                return View("unPaidBill");
            }

        }



        //[HttpPost]
        //public ActionResult Void(int? id)
        //{
        //    if(id != null)
        //    {
        //        Order vorder = db.Orders.Find(id);
        //        vorder.OrderStatus = false;
        //        db.SaveChanges();
        //        return RedirectToAction("unPaidBill");
        //    }
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Order order = db.Orders.Find(id);
        //    if (order == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(order);
        //}

        //[HttpPost, ActionName("Void")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Order order = db.Orders.Find(id);
        //    order.isPaid = true;
        //    //db.Orders.Remove(order);

        //    db.SaveChanges();
        //    return RedirectToAction("unPaidBill");
        //}


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
