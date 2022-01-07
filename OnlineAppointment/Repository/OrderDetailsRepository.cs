using OnlineAppointment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineAppointment.Repository
{
    public class OrderDetailsRepository
    {
        private OnlineAppointmentContext objOnlineAppointmentContext;
        public OrderDetailsRepository()
        {
            objOnlineAppointmentContext = new OnlineAppointmentContext();
        }
        public IEnumerable<SelectListItem> GetAllOrderDetails()
        {
            var objSelectListItems = new List<SelectListItem>();
            objSelectListItems = (from obj in objOnlineAppointmentContext.OrderDetails
                                  select new SelectListItem()
                                  {
                                      Text = obj.OrderID.ToString(),
                                      Value = obj.OrderDetailID.ToString(),
                                      Selected = true
                                  }).ToList();
            return objSelectListItems;
        }
    }
}