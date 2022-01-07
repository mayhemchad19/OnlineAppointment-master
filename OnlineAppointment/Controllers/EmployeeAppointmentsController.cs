using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using OnlineAppointment.Models;
using System.Data.Entity.Validation;

namespace OnlineAppointment.Controllers
{
    public class EmployeeAppointmentsController : Controller
    {
        private OnlineAppointmentContext db = new OnlineAppointmentContext();
        public Boolean CheckSession()
        {
            if (Session["id"] == null)
            {
                Session["id"] = 0;
            }

            if (int.Parse(Session["id"].ToString()) != 2)
            {
                return false;
            }

            else
            {
                Session.Timeout = 60;
                return true;
            }
        }
        String userEmail;
        int appID;
        public void SendEmail(string emailBody)
        {
            MailMessage mailMessage = new MailMessage("vivianinon@gmail.com", this.userEmail);
            mailMessage.Subject = "Vivi Online Appointment";
            mailMessage.Body = emailBody;

            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.Credentials = new NetworkCredential()
            {
                UserName = "vivianinon@gmail.com",
                Password = "Vivianinon123"
            };
            smtpClient.EnableSsl = true;
            smtpClient.Send(mailMessage);
        }

        // GET: EmployeeAppointments
        public ActionResult Index()
        {
            //------------------------------------session-------------------
            //if (CheckSession())
            //{
            //    // FUNCTIONS
            //    var appointments = db.Appointments.Include(a => a.AppointmentState).Include(a => a.Slot).Include(a => a.User);
            //    return View(appointments.ToList());
            //}
            //else return RedirectToAction("Login", "Logs");
            var lessdate = DateTime.Today.AddDays(-1);
            var greatdate = DateTime.Today.AddDays(1);
            var appointments = db.Appointments.Include(a => a.AppointmentState).Include(a => a.Product).Include(a => a.Slot).Include(a => a.User);
            return View(appointments.Where(a => a.AppointmentDate < greatdate && a.AppointmentDate > lessdate && a.AppointmentStateID == 1 || a.AppointmentDate < greatdate && a.AppointmentDate > lessdate && a.AppointmentStateID == 5 || a.AppointmentDate < greatdate && a.AppointmentDate > lessdate && a.AppointmentStateID == 4).OrderBy(b => b.SlotID).ToList());
            //return View(appointments.Where(a => a.AppointmentStateID == 3).ToList());
            //return View(appointments.ToList());
        }

        public ActionResult PendingAppointments()
        {
            var appointments = db.Appointments.Include(a => a.AppointmentState).Include(a => a.Product).Include(a => a.Slot).Include(a => a.User);
            return View(appointments.Where(a => a.AppointmentStateID == 5 && a.AppointmentDate>= DateTime.Today).OrderBy(b => b.AppointmentDate).ThenBy(c => c.SlotID).ToList());
            //return View(appointments.Where(a => a.AppointmentStateID == 3).ToList());
        }

        // GET: EmployeeAppointments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appointment appointment = db.Appointments.Find(id);
            if (appointment == null)
            {
                return HttpNotFound();
            }

            var uID = (from x in db.Appointments where x.AppointmentId == id select x.UserID).FirstOrDefault();
            //var date = (from x in db.Appointments where x.AppointmentId == id select x.AppointmentDate).FirstOrDefault();
            var slotID = (from x in db.Appointments where x.AppointmentId == id select x.SlotID).FirstOrDefault();
            var time = (from x in db.Slots where x.SlotsID == slotID select x.Time).FirstOrDefault();
            var name = (from x in db.Users where x.UserID == uID select x.FirstName).FirstOrDefault();
            this.userEmail = (from x in db.Users where x.UserID == uID select x.Email).FirstOrDefault();

            string body = "Hello " + name.ToString() + " you have an appointment Today at " + time;
            SendEmail(body);


            return RedirectToAction("Index");
        }



        // GET: EmployeeAppointments/Create
        public ActionResult Create()
        {

            var fullname = db.Users.Where(u => u.RoleID == 4 && u.UserStatus != false || u.RoleID == 3 && u.UserStatus != false).OrderBy(u => u.FirstName).ToList();
            IEnumerable<SelectListItem> selectList = from s in fullname
                                                     select new SelectListItem
                                                     {
                                                         Value = s.UserID.ToString(),
                                                         Text = s.FirstName + " " + s.LastName
                                                     };



            ViewBag.AppointmentStateID = new SelectList(db.AppointmentStates, "AppointmentStateID", "AppointmentStatus");
            ViewBag.ProductID = new SelectList(db.Products.Where(p => p.ProductTypeID != 1 && p.ProductStatus == true).OrderBy(p=>p.ProductTypeID), "ProductID", "ProductName");
            ViewBag.SlotID = new SelectList(db.Slots, "SlotsID", "Time");
            //ViewBag.UserID = new SelectList(db.Users.Where(u => u.RoleID !=1 && u.RoleID !=2 ).OrderBy(u=>u.FirstName), "UserID", "Username");
            ViewBag.UserID = new SelectList(selectList, "Value", "Text");
            return View();
        }

        // POST: EmployeeAppointments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AppointmentId,UserID,ProductID,AppointmentDate,SlotID,AppointmentStateID")] Appointment appointment)
        {
            var fullname = db.Users.Where(u => u.RoleID == 4 && u.UserStatus != false || u.RoleID == 3 && u.UserStatus != false).OrderBy(u => u.FirstName).ToList();
            IEnumerable<SelectListItem> selectList = from s in fullname
                                                     select new SelectListItem
                                                     {
                                                         Value = s.UserID.ToString(),
                                                         Text = s.FirstName + " " + s.LastName
                                                     };


            if (ModelState.IsValid)
            {
                db.Appointments.Add(appointment);
                appointment.AppointmentStateID = 3;


                var userID = appointment.UserID;
                var slotID = appointment.SlotID;
                var date = appointment.AppointmentDate;
                var productID = appointment.ProductID;
                var service = (from x in db.Products where x.ProductID == productID select x.ProductName).FirstOrDefault();
                //var date2 = date?.ToString("dd/M/yyyy");
                var time = (from x in db.Slots where x.SlotsID == slotID select x.Time).FirstOrDefault();
                var name = (from x in db.Users where x.UserID == userID select x.FirstName).FirstOrDefault();
                this.userEmail = (from x in db.Users where x.UserID == userID select x.Email).FirstOrDefault();

                string body = "Hello " + name.ToString() + " you have been scheduled an appointmnet for "+service+" on "+date?.ToString("MMM/dd/yyyy")+" at " + time + ". Please visit our website http://viviessence.xyz/ for more information.";
                SendEmail(body);



                //------------------------------------------------------------------------
                //var userID = (from x in db.Users where x.UserID == uID select x.UserID).FirstOrDefault();
                appointment.UserID = userID;

                var status = (from a in db.Appointments
                              where userID == appointment.UserID && appointment.AppointmentDate == a.AppointmentDate && appointment.SlotID == a.SlotID
                              select a.AppointmentStateID).FirstOrDefault();

                //pending pero dapat 5


                var checkSpecific = (from u in db.Appointments
                                     where u.AppointmentDate == appointment.AppointmentDate && u.SlotID == appointment.SlotID && u.UserID == appointment.UserID
                                     select u).ToList();
                if (checkSpecific.Count >= 1)
                {
                    ViewBag.AppointmentStateID = new SelectList(db.AppointmentStates, "AppointmentStateID", "AppointmentStatus", appointment.AppointmentStateID);
                    ViewBag.ProductID = new SelectList(db.Products.Where(p => p.ProductTypeID != 1 && p.ProductStatus == true).OrderBy(p => p.ProductName), "ProductID", "ProductName", appointment.ProductID);
                    ViewBag.SlotID = new SelectList(db.Slots, "SlotsID", "Time", appointment.SlotID);
                    ViewBag.UserID = new SelectList(selectList, "Value", "Text");

                    if (status == 3)
                    {
                        ModelState.AddModelError("AppointmentDate",name +" already has a Pending Appointment with the same schedule. Please pick another Date.");
                    }
                    if (status == 5)
                    {
                        ModelState.AddModelError("AppointmentDate",name +" already has an Approved Appointment with the same schedule. Please pick another Date.");
                    }

                    return View(appointment);

                }


                //var checkDate = (from u in db.Appointments
                //                 where u.AppointmentDate == appointment.AppointmentDate && u.AppointmentStateID == 5 ||
                //                 u.AppointmentDate == appointment.AppointmentDate && u.AppointmentStateID == 3 ||
                //                 u.AppointmentDate == appointment.AppointmentDate && u.AppointmentStateID == 1
                //                 select u).ToList();

                var checkDate = (from u in db.Appointments
                                 where u.AppointmentDate == appointment.AppointmentDate && u.AppointmentStateID == 5 && u.User.UserStatus == true ||
                                 u.AppointmentDate == appointment.AppointmentDate && u.AppointmentStateID == 3 && u.User.UserStatus == true ||
                                 u.AppointmentDate == appointment.AppointmentDate && u.AppointmentStateID == 1 && u.User.UserStatus == true
                                 select u).ToList();

                if (checkDate.Count >= 88)
                {
                    ViewBag.AppointmentStateID = new SelectList(db.AppointmentStates, "AppointmentStateID", "AppointmentStatus", appointment.AppointmentStateID);
                    ViewBag.ProductID = new SelectList(db.Products.Where(p => p.ProductTypeID != 1 && p.ProductStatus == true).OrderBy(p => p.ProductName), "ProductID", "ProductName", appointment.ProductID);
                    ViewBag.SlotID = new SelectList(db.Slots, "SlotsID", "Time", appointment.SlotID);
                    ViewBag.UserID = new SelectList(selectList, "Value", "Text");


                    ModelState.AddModelError("AppointmentDate", "Appointment for this Date is Already Full. Please try another Date");


                    return View(appointment);

                }


                //var checkSlot = (from u in db.Appointments
                //                 where u.AppointmentDate == appointment.AppointmentDate && u.SlotID == appointment.SlotID && u.AppointmentStateID == 5 ||
                //                 u.AppointmentDate == appointment.AppointmentDate && u.SlotID == appointment.SlotID && u.AppointmentStateID == 3 ||
                //                 u.AppointmentDate == appointment.AppointmentDate && u.AppointmentStateID == 1
                                 //select u).ToList();


                var checkSlot = (from u in db.Appointments
                                 where u.AppointmentDate == appointment.AppointmentDate && u.SlotID == appointment.SlotID && u.AppointmentStateID == 5 && u.User.UserStatus == true ||
                                 u.AppointmentDate == appointment.AppointmentDate && u.SlotID == appointment.SlotID && u.AppointmentStateID == 3 && u.User.UserStatus == true ||
                                 u.AppointmentDate == appointment.AppointmentDate && u.AppointmentStateID == 1 && u.User.UserStatus == true
                                 select u).ToList();

                if (checkSlot.Count >= 8)
                {
                    //var time = (from s in db.Slots where s.SlotsID == appointment.SlotID select s.Time).FirstOrDefault();
                    //var fullname = db.Slots.Where(u => u.RoleID == 4 || u.RoleID == 3).OrderBy(u => u.FirstName).ToList();
                    //IEnumerable<SelectListItem> selectList = from s in fullname
                    //                                         select new SelectListItem
                    //                                         {
                    //                                             Value = s.SlotID.ToString(),
                    //                                             Text = s.FirstName + " " + s.LastName
                    //                                         };
                    //ViewBag.SlotID = new SelectList(selectList, "Value", "Text");


                    ViewBag.AppointmentStateID = new SelectList(db.AppointmentStates, "AppointmentStateID", "AppointmentStatus", appointment.AppointmentStateID);
                    ViewBag.ProductID = new SelectList(db.Products.Where(p => p.ProductTypeID != 1 && p.ProductStatus == true).OrderBy(p => p.ProductName), "ProductID", "ProductName", appointment.ProductID);
                    ViewBag.SlotID = new SelectList(db.Slots, "SlotsID", "Time", appointment.SlotID);
                    ViewBag.UserID = new SelectList(selectList, "Value", "Text");


                    ModelState.AddModelError("SlotID", "Appointment for " + time + " is Already Full. Please pick Another Slot.");


                    return View(appointment);

                }
                //------------------------------------------------------------------------











                db.SaveChanges();
                return RedirectToAction("PendingAppointments");
            }

            ViewBag.AppointmentStateID = new SelectList(db.AppointmentStates, "AppointmentStateID", "AppointmentStatus", appointment.AppointmentStateID);
            ViewBag.ProductID = new SelectList(db.Products.Where(p => p.ProductTypeID != 1 && p.ProductStatus == true).OrderBy(p => p.ProductName), "ProductID", "ProductName", appointment.ProductID);
            ViewBag.SlotID = new SelectList(db.Slots, "SlotsID", "Time", appointment.SlotID);
            ViewBag.UserID = new SelectList(selectList, "Value", "Text");
            return View(appointment);
        }

        // GET: EmployeeAppointments/Edit/5
        public ActionResult Edit(int? id)
        {
           if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                Appointment appointment = db.Appointments.Find(id);
                if (appointment == null)
                {
                    return HttpNotFound();
                }
                else
                {

              

                    var uID = (from x in db.Appointments where x.AppointmentId == id select x.UserID).FirstOrDefault();
                    var date = (from x in db.Appointments where x.AppointmentId == id select x.AppointmentDate).FirstOrDefault();
                    var slotID = (from x in db.Appointments where x.AppointmentId == id select x.SlotID).FirstOrDefault();
                    var time = (from x in db.Slots where x.SlotsID == slotID select x.Time).FirstOrDefault();
                    var serv = (from x in db.Appointments where x.AppointmentId == id select x.ProductID).FirstOrDefault();
                    //var name = (from x in db.Users where x.UserID == uID select x.FirstName).FirstOrDefault();

                    appointment.ProductID = serv;
                    appointment.SlotID = slotID;
                    appointment.UserID = uID;
                    appointment.AppointmentDate = DateTime.Today;

                    appointment.AppointmentStateID = 4;
                    //appointment.AppointmentDate = date;
                    ViewBag.AppointmentStateID = new SelectList(db.AppointmentStates, "AppointmentStateID", "AppointmentStatus", appointment.AppointmentStateID);
                    ViewBag.SlotID = new SelectList(db.Slots, "SlotsID", "Time", appointment.SlotID);
                    ViewBag.UserID = new SelectList(db.Users, "UserID", "Username", appointment.UserID);
                    db.SaveChanges();
                    return RedirectToAction("index");
                }
                ViewBag.AppointmentStateID = new SelectList(db.AppointmentStates, "AppointmentStateID", "AppointmentStatus", appointment.AppointmentStateID);
                ViewBag.SlotID = new SelectList(db.Slots, "SlotsID", "Time", appointment.SlotID);
                ViewBag.UserID = new SelectList(db.Users, "UserID", "Username", appointment.UserID);
                return View(appointment);
            }
        }

        // POST: EmployeeAppointments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Bind(Include = "AppointmentId,UserID,ProductID,AppointmentDate,SlotID,AppointmentStateID")]
        public ActionResult Edit( Appointment appointment)
        {
            this.appID = appointment.AppointmentId;
            //if (ModelState.IsValid)
            //{

                var uID = (from x in db.Appointments where x.AppointmentId == this.appID select x.UserID).FirstOrDefault();
                //var date = (from x in db.Appointments where x.AppointmentId == appID select x.AppointmentDate).FirstOrDefault();
                var slotID = (from x in db.Appointments where x.AppointmentId == this.appID select x.SlotID).FirstOrDefault();
                var time = (from x in db.Slots where x.SlotsID == slotID select x.Time).FirstOrDefault();
                var serv = (from x in db.Appointments where x.AppointmentId == this.appID select x.ProductID).FirstOrDefault();
                //var name = (from x in db.Users where x.UserID == uID select x.FirstName).FirstOrDefault();

                appointment.ProductID = serv;
                appointment.SlotID = slotID;
                appointment.UserID = uID;
                appointment.AppointmentDate = DateTime.Today;
                appointment.AppointmentStateID = 1;
                db.Entry(appointment).State = EntityState.Modified;

               
                db.SaveChanges();
            return RedirectToAction("Index");
            //}
            //ViewBag.AppointmentStateID = new SelectList(db.AppointmentStates, "AppointmentStateID", "AppointmentStatus", appointment.AppointmentStateID);
                ViewBag.ProductID = new SelectList(db.Products, "ProductID", "ProductName", appointment.ProductID);
                ViewBag.SlotID = new SelectList(db.Slots, "SlotsID", "Time", appointment.SlotID);
                ViewBag.UserID = new SelectList(db.Users, "UserID", "Username", appointment.UserID);
                return RedirectToAction("Index");



           
        }

        // GET: EmployeeAppointments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                Appointment appointment = db.Appointments.Find(id);
                if (appointment == null)
                {
                    return HttpNotFound();
                }
                else
                {



                    var uID = (from x in db.Appointments where x.AppointmentId == id select x.UserID).FirstOrDefault();
                    var date = (from x in db.Appointments where x.AppointmentId == id select x.AppointmentDate).FirstOrDefault();
                    var slotID = (from x in db.Appointments where x.AppointmentId == id select x.SlotID).FirstOrDefault();
                    var time = (from x in db.Slots where x.SlotsID == slotID select x.Time).FirstOrDefault();
                    var serv = (from x in db.Appointments where x.AppointmentId == id select x.ProductID).FirstOrDefault();
                    //var name = (from x in db.Users where x.UserID == uID select x.FirstName).FirstOrDefault();

                    appointment.ProductID = serv;
                    appointment.SlotID = slotID;
                    appointment.UserID = uID;
                    appointment.AppointmentDate = DateTime.Today;

                    appointment.AppointmentStateID = 1;
                    //appointment.AppointmentDate = date;
                    ViewBag.AppointmentStateID = new SelectList(db.AppointmentStates, "AppointmentStateID", "AppointmentStatus", appointment.AppointmentStateID);
                    ViewBag.SlotID = new SelectList(db.Slots, "SlotsID", "Time", appointment.SlotID);
                    ViewBag.UserID = new SelectList(db.Users, "UserID", "Username", appointment.UserID);
                    db.SaveChanges();
                    return RedirectToAction("index");
                }
                ViewBag.AppointmentStateID = new SelectList(db.AppointmentStates, "AppointmentStateID", "AppointmentStatus", appointment.AppointmentStateID);
                ViewBag.SlotID = new SelectList(db.Slots, "SlotsID", "Time", appointment.SlotID);
                ViewBag.UserID = new SelectList(db.Users, "UserID", "Username", appointment.UserID);
                return View(appointment);
            }


        }

        // POST: EmployeeAppointments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Appointment appointment = db.Appointments.Find(id);
            appointment.AppointmentStateID = 1;
            //db.SaveChanges();
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
    }
}
