using OnlineAppointment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineAppointment.Repository
{
    public class InvoiceRepository
    {
        private OnlineAppointmentContext objOnlineAppointmentContext;
        public InvoiceRepository()
        {
            objOnlineAppointmentContext = new OnlineAppointmentContext();
        }
        public IEnumerable<SelectListItem> GetAllInvoices()
        {
            var objSelectListItems = new List<SelectListItem>();
            objSelectListItems = (from obj in objOnlineAppointmentContext.Orders.Where(o=>o.OrderStatus!=false)
                                  select new SelectListItem()
                                  {
                                      Text = obj.OrderNumber,
                                      Value = obj.OrderID.ToString(),
                                      Selected = true
                                  }).ToList();
            return objSelectListItems;
        }
    }
}