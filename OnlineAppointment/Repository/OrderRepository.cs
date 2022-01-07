using OnlineAppointment.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace OnlineAppointment.Repository
{
    public class OrderRepository
    {

        private OnlineAppointmentContext objOnlineAppointmentContext;

        public OrderRepository()
        {
            objOnlineAppointmentContext = new OnlineAppointmentContext();
        }

        public bool AddOrder(Sale objOrder)
        {
            Sale order = new Sale();
            order.SaleID = order.SaleID;

            order.UserID = objOrder.UserID;
            order.FinalTotal = objOrder.FinalTotal;
            order.OrderDate = DateTime.Now;
            order.OrderNumber = string.Format("{0:ddmmmyyyyhhmmss}", DateTime.Now);
            order.PaymentTypeID = objOrder.PaymentTypeID;
            order.DiscountTypeID = objOrder.DiscountTypeID;
          
            order.DiscountedTotal = objOrder.DiscountedTotal;
            order.isPaid = true;

            objOnlineAppointmentContext.Sales.Add(order);
            objOnlineAppointmentContext.SaveChanges();
            int OrderID = order.SaleID;


            foreach (var item in objOrder.ListOfOrderDetail)
            {
                SaleDetail objOrderDetail = new SaleDetail();
                objOrderDetail.SaleID = OrderID;
                objOrderDetail.Discount = item.Discount;
                objOrderDetail.ProductID = item.ProductID;
                objOrderDetail.Total = item.Total;
                objOrderDetail.UnitPrice = item.UnitPrice;
                objOrderDetail.Quantity = item.Quantity;
                objOnlineAppointmentContext.SaleDetails.Add(objOrderDetail);
                objOnlineAppointmentContext.SaveChanges();


            }

            return true;
        }

        public bool AddInvoice(Order objOrder)
        {
            //OnlineAppointmentContext db = new OnlineAppointmentContext();
            Order order = new Order();
            order.UserID = objOrder.UserID;
            order.FinalTotal = objOrder.FinalTotal;
            order.OrderDate = DateTime.Now;
            order.OrderNumber = string.Format("{0:ddmmmyyyyhhmmss}", DateTime.Now);
            order.PaymentTypeID = objOrder.PaymentTypeID;
            order.DiscountTypeID = objOrder.DiscountTypeID;
            order.DiscountedTotal = objOrder.DiscountedTotal;
            order.isPaid = false;
            //db.Entry(order).State = EntityState.Modified;
            objOnlineAppointmentContext.Orders.Add(order);
            objOnlineAppointmentContext.SaveChanges();
            int OrderID = order.OrderID;


            foreach (var item in objOrder.ListOfOrderDetail)
            {
                OrderDetail objOrderDetail = new OrderDetail();
                objOrderDetail.OrderID = OrderID;
                objOrderDetail.Discount = item.Discount;
                objOrderDetail.ProductID = item.ProductID;
                objOrderDetail.Total = item.Total;
                objOrderDetail.UnitPrice = item.UnitPrice;
                objOrderDetail.Quantity = item.Quantity;
                objOnlineAppointmentContext.OrderDetails.Add(objOrderDetail);
                objOnlineAppointmentContext.SaveChanges();


            }

            return true;
        }


        public bool SettleOrder(Order objOrder)
        {
            Order order = new Order();

            order.OrderID = order.OrderID;

            order.UserID = objOrder.UserID;
            order.FinalTotal = objOrder.FinalTotal;
            order.OrderDate = DateTime.Now;
            order.OrderNumber = string.Format("{0:ddmmmyyyyhhmmss}", DateTime.Now);
            order.PaymentTypeID = objOrder.PaymentTypeID;
            order.DiscountTypeID = objOrder.DiscountTypeID;

            order.DiscountedTotal = objOrder.DiscountedTotal;
            order.isPaid = true;

            objOnlineAppointmentContext.Orders.Add(order);
            objOnlineAppointmentContext.SaveChanges();
            int OrderID = order.OrderID;


            foreach (var item in objOrder.ListOfOrderDetail)
            {
                OrderDetail objOrderDetail = new OrderDetail();
                objOrderDetail.OrderID = OrderID;
                objOrderDetail.Discount = item.Discount;
                objOrderDetail.ProductID = item.ProductID;
                objOrderDetail.Total = item.Total;
                objOrderDetail.UnitPrice = item.UnitPrice;
                objOrderDetail.Quantity = item.Quantity;
                objOnlineAppointmentContext.OrderDetails.Add(objOrderDetail);
                objOnlineAppointmentContext.SaveChanges();


            }

            return true;
        }
    }
}