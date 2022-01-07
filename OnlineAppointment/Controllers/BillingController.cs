using OnlineAppointment.Models;
using OnlineAppointment.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace OnlineAppointment.Controllers
{
    public class BillingController : Controller
    {
        // GET: Billing
        OnlineAppointmentContext db = new OnlineAppointmentContext();
        private OnlineAppointmentContext objOnlineAppointmentContext;

        
        public BillingController()
        {
            objOnlineAppointmentContext = new OnlineAppointmentContext();
        }
        public ActionResult Index()
        {
            UserRepository objUserRepository = new UserRepository();
            ProductRepository objProductRepository = new ProductRepository();
            PaymentTypeRepository objPaymentTypeRepository = new PaymentTypeRepository();


            var objMultipleModels = new Tuple<IEnumerable<SelectListItem>, IEnumerable<SelectListItem>, IEnumerable<SelectListItem>>
                (objUserRepository.GetAllUser(), objProductRepository.GetAllProducts(), objPaymentTypeRepository.GetAllPaymentType());

            return View(objMultipleModels);
        }


        //-----------------------------------------------
        [HttpGet]
        public JsonResult GetAllSales()
        {
            //OnlineAppointmentContext db = new OnlineAppointmentContext();
            //POS_TutorialEntities db = new POS_TutorialEntities();
            var dataList = objOnlineAppointmentContext.Orders.Include("User").ToList();
            var modefiedData = dataList.Select(x => new
            {
                SalesId = x.OrderID,
                PaymentID = x.PaymentTypeID,
                UserID = x.UserID,
                OrderNo = x.OrderNumber,
                CustomerName = x.User.FirstName,
                //CustomerPhone = x.CustomerPhone,
                //CustomerAddress = x.CustomerAddress,
                OrderDate = x.OrderDate.ToString(),
                TotalAmount = x.FinalTotal,
                discountType = x.DiscountTypeID,
                dTotal = x.DiscountedTotal,
                isPaid = x.isPaid,
                orderstatus = x.OrderStatus
            }).Where(x => x.isPaid == false && x.orderstatus != false).ToList();
            return Json(modefiedData, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult GetInvoiceBySalesId(int salesId)
        {
            OnlineAppointmentContext db = new OnlineAppointmentContext();
            var dataList = objOnlineAppointmentContext.OrderDetails.Include("Order").Include("Product").ToList();
            var modefiedData = dataList.Select(x => new
            {
                OrderDetailID = x.OrderDetailID,
                OrderID = x.Order.OrderID,
                OrderNumber = x.Order.OrderNumber,
                ProductID = x.ProductID,
                productname = x.Product.ProductName,
                unitPrice = x.UnitPrice,
                quantity = x.Quantity,
                Discount = x.Discount,
                subTotal = x.Total,
                Total = x.Order.FinalTotal,
                UserID = x.Order.UserID
                //FirstName = x.Order.User.FirstName
            }).Where(x => x.OrderID == salesId).ToList();


            return Json(modefiedData, JsonRequestBehavior.AllowGet);
        }
        //-----------------------------------
        [HttpGet]
        public JsonResult getItemUnitPrice(int itemId)
        {
            decimal UnitPrice = objOnlineAppointmentContext.Products.Single(model => model.ProductID == itemId).ProductPrice;
            return Json(UnitPrice, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Index(Sale objOrder)
        {
            OrderRepository objOrderRepository = new OrderRepository();
            objOrderRepository.AddOrder(objOrder);
            return Json("Your Sale has been Successfully Placed.", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CreateInvoice(Order objOrder)
        {
            OrderRepository objOrderRepository = new OrderRepository();
            objOrderRepository.AddInvoice(objOrder);
            return Json("Your Order has been Successfully Placed.", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SetOrder(Order objOrder)
        {
            OrderRepository objOrderRepository = new OrderRepository();
            objOrderRepository.SettleOrder(objOrder);
            return Json("Your Order has been Successfully Placed.", JsonRequestBehavior.AllowGet);
        }
        //try section
        public ActionResult Try()
        {
            UserRepository objUserRepositroy = new UserRepository();
            ProductRepository objProductRepostiory = new ProductRepository();
            PaymentTypeRepository objPaymentTypeRepository = new PaymentTypeRepository();
            DiscountTypeRepository objDiscountTypeRepository = new DiscountTypeRepository();

            var objMultipleModels = new Tuple<IEnumerable<SelectListItem>, IEnumerable<SelectListItem>, IEnumerable<SelectListItem>, IEnumerable<SelectListItem>>
                (objUserRepositroy.GetAllUser(), objProductRepostiory.GetAllProducts(), objPaymentTypeRepository.GetAllPaymentType(), objDiscountTypeRepository.GetAllDiscountType());

            return View(objMultipleModels);
        }

        [HttpPost]
        public JsonResult Try(Order objOrder)
        {
            OrderRepository objOrderRepository = new OrderRepository();
            objOrderRepository.AddInvoice(objOrder);
            return Json("Your Order has been Successfully Placed.", JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult getDiscountRate(int DiscountTypeID)
        {
            var DiscountRate = objOnlineAppointmentContext.DiscountTypes.Single(model => model.DiscountTypeID == DiscountTypeID).DiscountAmount;
            return Json(DiscountRate, JsonRequestBehavior.AllowGet);
        }



        public ActionResult Invoice()
        {
            UserRepository objUserRepositroy = new UserRepository();
            ProductRepository objProductRepostiory = new ProductRepository();
            PaymentTypeRepository objPaymentTypeRepository = new PaymentTypeRepository();
            DiscountTypeRepository objDiscountTypeRepository = new DiscountTypeRepository();
            InvoiceRepository invoiceRepository = new InvoiceRepository();
            OrderDetailsRepository orderDetailsRepository = new OrderDetailsRepository();


            var objMultipleModels = new Tuple<IEnumerable<SelectListItem>, IEnumerable<SelectListItem>, IEnumerable<SelectListItem>, IEnumerable<SelectListItem>, IEnumerable<SelectListItem>, IEnumerable<SelectListItem>>
                (objUserRepositroy.GetAllUser(), objProductRepostiory.GetAllProducts(), objPaymentTypeRepository.GetAllPaymentType(), objDiscountTypeRepository.GetAllDiscountType(), invoiceRepository.GetAllInvoices(), orderDetailsRepository.GetAllOrderDetails());

            return View(objMultipleModels);
        }



        public ActionResult InvoiceList()
        {

            UserRepository objUserRepositroy = new UserRepository();
            ProductRepository objProductRepostiory = new ProductRepository();
            PaymentTypeRepository objPaymentTypeRepository = new PaymentTypeRepository();
            DiscountTypeRepository objDiscountTypeRepository = new DiscountTypeRepository();
            InvoiceRepository invoiceRepository = new InvoiceRepository();
            OrderDetailsRepository orderDetailsRepository = new OrderDetailsRepository();


            var objMultipleModels = new Tuple<IEnumerable<SelectListItem>, IEnumerable<SelectListItem>, IEnumerable<SelectListItem>, IEnumerable<SelectListItem>, IEnumerable<SelectListItem>, IEnumerable<SelectListItem>>
                (objUserRepositroy.GetAllUser(), objProductRepostiory.GetAllProducts(), objPaymentTypeRepository.GetAllPaymentType(), objDiscountTypeRepository.GetAllDiscountType(), invoiceRepository.GetAllInvoices(), orderDetailsRepository.GetAllOrderDetails());

            return View(objMultipleModels);
        }

        public ActionResult PartialInvoiceList()
        {

            //var users = db.Users.Include(u => u.Gender).Include(u => u.UserRole);

            var orders = objOnlineAppointmentContext.Orders;
            //var orders = objOnlineAppointmentContext.Orders.Include(u => user.Username).Include(o => PaymentType);
            return View(orders.Where(o => o.isPaid == false).ToList());
        }

        //// POST: Orders/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]

        //public ActionResult DeleteConfirmed(int id)
        //{
        //    OnlineAppointmentContext db = new OnlineAppointmentContext();
        //    Order order = db.Orders.Find(id);
        //    db.Orders.Remove(order);
        //    db.SaveChanges();
        //    return RedirectToAction("InvoiceList");
        //}





        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult OrderStatus([Bind(Include = "OrderID,PaymentTypeID,UserID,OrderNumber,OrderDate,DiscountTypeID,FinalTotal,DiscountedTotal")] Order order)
        //{
        //    OnlineAppointmentContext db = new OnlineAppointmentContext();
        //    if (ModelState.IsValid)
        //    {

        //        var userID = (from x in db.Users where x.Username == user.Username select x.UserID).FirstOrDefault();
        //        var username = (from x in db.Users where x.UserID == userID select x.Username).FirstOrDefault();

        //        var orderID = (from o in db.Orders where o.OrderID == order.OrderID select o.OrderID).FirstOrDefault();
        //        var paymentTypeID = (from o in db.Orders where o.OrderID == orderID select o.PaymentTypeID).FirstOrDefault();

        //        db.Entry(order).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.DiscountTypeID = new SelectList(db.DiscountTypes, "DiscountTypeID", "DiscountTypeName", order.DiscountTypeID);
        //    ViewBag.PaymentTypeID = new SelectList(db.PaymentTypes, "PaymentTypeID", "PaymentTypeName", order.PaymentTypeID);
        //    ViewBag.UserID = new SelectList(db.Users, "UserID", "Username", order.UserID);
        //    return true;
        //}

        //[HttpPost]
        //public JsonResult EditInvoiceSale([Bind(Include = "OrderID,PaymentTypeID,UserID,OrderNumber,OrderDate,DiscountTypeID,FinalTotal,DiscountedTotal")] Order order)
        //{
        //    {

        //    OnlineAppointmentContext db = new OnlineAppointmentContext();


        //    //if (deleted != null)
        //    //{
        //    //    foreach (var item in deleted)
        //    //    {
        //    //        var data = db.SalesDetails.Where(x => x.SalesDetailId == item).SingleOrDefault(); ;
        //    //        db.SalesDetails.Remove(data);
        //    //    }
        //    //}

        //    foreach (var item in order)
        //    {
        //        if (item.SalesDetailId > 0)
        //        {
        //            db.Entry(item).State = EntityState.Modified;
        //            retMessage.Messagae = "Update Success!";
        //        }
        //        else
        //        {
        //            sale.SalesDetails.Add(new SalesDetail { ProductId = item.ProductId, UnitPrice = item.UnitPrice, Quantity = item.Quantity, LineTotal = item.LineTotal });
        //            var prd = db.ProductStocks.Where(x => x.ProductId == item.ProductId && x.Quantity > 0).FirstOrDefault();
        //            prd.Quantity = prd.Quantity - item.Quantity;
        //            db.Entry(prd).State = EntityState.Modified;
        //            db.Sales.Add(sale);
        //            retMessage.Messagae = "Save Success!";
        //        }
        //    }


        //    try
        //    {
        //        db.SaveChanges();
        //    }
        //    catch (Exception)
        //    {
        //        retMessage.IsSuccess = false;
        //    }

        //    return Json(retMessage, JsonRequestBehavior.AllowGet);
        //}


        public ActionResult BillingIndex()
        {
            OnlineAppointmentContext db = new OnlineAppointmentContext();

            var orders = db.Orders.Include(o => o.User);
            //return View(users.ToList());

            return View(orders.Where(o => o.isPaid == false && o.OrderStatus != false).ToList());
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
            db.SaveChanges();
            return RedirectToAction("BillingIndex");
        }
    }
}