using OnlineAppointment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineAppointment.Repository
{
    public class ProductRepository
    {


        private OnlineAppointmentContext objOnlineAppointmentContext;
        public ProductRepository()
        {
            objOnlineAppointmentContext = new OnlineAppointmentContext();
        }

        public IEnumerable<SelectListItem> GetAllProducts()
        {
            var objSelectListItems = new List<SelectListItem>();
            objSelectListItems = (from obj in objOnlineAppointmentContext.Products.Where(p=> p.ProductTypeID !=3 && p.ProductTypeID !=4 && p.ProductStatus != false)
                                  select new SelectListItem()
                                  {
                                      Text = obj.ProductName,
                                      Value = obj.ProductID.ToString(),
                                      Selected = true
                                  }).ToList();
            return objSelectListItems;
        }
    }
}