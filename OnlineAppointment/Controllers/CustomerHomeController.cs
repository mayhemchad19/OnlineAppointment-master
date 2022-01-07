using OnlineAppointment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineAppointment.Controllers
{
    public class CustomerHomeController : Controller
    {
        OnlineAppointmentContext db = new OnlineAppointmentContext();
        // GET: CustomerHome
        public Boolean CheckSession()
        {
            if (Session["id"] == null)
            {
                Session["id"] = 0;
            }

            if (int.Parse(Session["id"].ToString()) == 0)
            {
                return false;
            }

            else
            {
                Session.Timeout = 60;
                return true;
            }
        }

        public ActionResult Index(User user)
        {
          
            var uid = int.Parse(Session["userID"].ToString());



            var username = (from x in db.Users where x.UserID == uid select x.FirstName).FirstOrDefault();
            ViewBag.Name = username;
            Session["username"] = username;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}