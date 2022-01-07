using OnlineAppointment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineAppointment.Repository
{
    public class DiscountTypeRepository
    {

        private OnlineAppointmentContext objOnlineAppointmentContext;
        public DiscountTypeRepository()
        {
            objOnlineAppointmentContext = new OnlineAppointmentContext();
        }

        public IEnumerable<SelectListItem> GetAllDiscountType()
        {
            var objSelectListItems = new List<SelectListItem>();
            objSelectListItems = (from obj in objOnlineAppointmentContext.DiscountTypes
                                  select new SelectListItem()
                                  {
                                      Text = obj.DiscountTypeName,
                                      Value = obj.DiscountTypeID.ToString(),
                                      Selected = true
                                  }).ToList();
            return objSelectListItems;
        }
    }
}