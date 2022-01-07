using OnlineAppointment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace OnlineAppointment.Repository
{
    public class UserRepository
    {
        private OnlineAppointmentContext objOnlineAppointmentContext;
        public UserRepository()
        {
            objOnlineAppointmentContext = new OnlineAppointmentContext();
        }

        public IEnumerable<SelectListItem> GetAllUser()
        {
            var objSelectListItems = new List<SelectListItem>();
            objSelectListItems = (from obj in objOnlineAppointmentContext.Users.Where(u => u.RoleID == 4 && u.UserStatus != false || u.RoleID == 3 && u.UserStatus !=false)
                                  select new SelectListItem()
                                  {
                                      Text = obj.FirstName +" " +obj.LastName,
                                      Value = obj.UserID.ToString(),
                                      Selected = true
                                  }).ToList();
            return objSelectListItems;
        }
    }
}