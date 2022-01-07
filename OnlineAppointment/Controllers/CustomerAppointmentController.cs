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

namespace OnlineAppointment.Controllers
{
    public class CustomerAppointmentController : Controller
    {
        private OnlineAppointmentContext db = new OnlineAppointmentContext();
        

        public Boolean CheckSession()
        {
            if (Session["id"] == null)
            {
                Session["id"] = 0;
            }

            if (int.Parse(Session["id"].ToString()) != 4)
            {
                return false;
            }

            else
            {
                Session.Timeout = 60;
                return true;
            }
        }
        //String userEmail;
        public void SendEmailelow(string emailBody)
        {
            MailMessage mailMessage = new MailMessage("mayhem.chad19@gmail.com", "vivianinon@gmail.com");
            mailMessage.Subject = "Appointment waiting for response";
            mailMessage.Body = emailBody;

            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.Credentials = new NetworkCredential()
            {
                UserName = "mayhem.chad19@gmail.com",
                Password = "honeykuh"
            };
            smtpClient.EnableSsl = true;
            smtpClient.Send(mailMessage);
        }



        public ActionResult HasAppointment()
        {


         
       
                return View();
       

        }

        // GET: CustomerAppointment
        public ActionResult Index() //pending Appointments
        {


            if (CheckSession())
            {
                var uID = int.Parse(Session["UserID"].ToString());


                //return View(users.Where(u => u.UserID == uID).ToList());
                // FUNCTIONS
                var appointments = db.Appointments.Include(a => a.AppointmentState).Include(a => a.Product).Include(a => a.Slot).Include(a => a.User);
                return View(appointments.Where(u => u.UserID == uID && u.AppointmentStateID == 3).ToList());
            }
            else return RedirectToAction("Login", "Logs");
            
        }

        public ActionResult CustomerAll() //All Appointments
        {


                var uID = int.Parse(Session["UserID"].ToString());


                //return View(users.Where(u => u.UserID == uID).ToList());
                // FUNCTIONS
                var appointments = db.Appointments.Include(a => a.AppointmentState).Include(a => a.Product).Include(a => a.Slot).Include(a => a.User);
                return View(appointments.Where(u => u.UserID == uID).OrderByDescending(a=> a.AppointmentDate).ToList());
            
   
        }

        public ActionResult Approved() //All Appointments
        {


            var uID = int.Parse(Session["UserID"].ToString());


            //return View(users.Where(u => u.UserID == uID).ToList());
            // FUNCTIONS
            var appointments = db.Appointments.Include(a => a.AppointmentState).Include(a => a.Product).Include(a => a.Slot).Include(a => a.User);
            return View(appointments.Where(u => u.UserID == uID && u.AppointmentStateID == 5).OrderBy(a => a.AppointmentDate).ToList());


        }

        public ActionResult Pending() //All Appointments
        {


            var uID = int.Parse(Session["UserID"].ToString());


            //return View(users.Where(u => u.UserID == uID).ToList());
            // FUNCTIONS
            var appointments = db.Appointments.Include(a => a.AppointmentState).Include(a => a.Product).Include(a => a.Slot).Include(a => a.User);
            return View(appointments.Where(u => u.UserID == uID && u.AppointmentStateID == 3).OrderBy(a => a.AppointmentDate).ToList());


        }

        public ActionResult Denied() //All Appointments
        {


            var uID = int.Parse(Session["UserID"].ToString());


            //return View(users.Where(u => u.UserID == uID).ToList());
            // FUNCTIONS
            var appointments = db.Appointments.Include(a => a.AppointmentState).Include(a => a.Product).Include(a => a.Slot).Include(a => a.User);
            return View(appointments.Where(u => u.UserID == uID && u.AppointmentStateID == 6).OrderBy(a => a.AppointmentDate).ToList());


        }


        // GET: CustomerAppointment/Details/5
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
            return View(appointment);
        }
        string error = "";
        // GET: CustomerAppointment/Create
        public ActionResult Create()
        {
           
            ViewBag.Error = error;
            ViewBag.AppointmentStateID = new SelectList(db.AppointmentStates, "AppointmentStateID", "AppointmentStatus");
            ViewBag.ProductID = new SelectList(db.Products.Where(p=> p.ProductTypeID == 2 && p.ProductStatus ==true).OrderBy(p=>p.ProductName), "ProductID", "ProductName");
            ViewBag.SlotID = new SelectList(db.Slots, "SlotsID", "Time");
            ViewBag.UserID = new SelectList(db.Users, "UserID", "Username");
            return View();
        }

        // POST: CustomerAppointment/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AppointmentId,UserID,ProductID,AppointmentDate,SlotID,AppointmentStateID")] Appointment appointment)
        {
            //ViewBag.Error = error;
            if (ModelState.IsValid)
            {

                if (appointment.AppointmentDate >= DateTime.Now.AddYears(1) || appointment.AppointmentDate <= DateTime.Now)
                {
                    ViewBag.AppointmentStateID = new SelectList(db.AppointmentStates, "AppointmentStateID", "AppointmentStatus", appointment.AppointmentStateID);
                    ViewBag.ProductID = new SelectList(db.Products.Where(p => p.ProductTypeID == 2 && p.ProductStatus == true).OrderBy(p => p.ProductName), "ProductID", "ProductName", appointment.ProductID);
                    ViewBag.SlotID = new SelectList(db.Slots, "SlotsID", "Time", appointment.SlotID);
                    ViewBag.UserID = new SelectList(db.Users, "UserID", "Username", appointment.UserID);


                    ModelState.AddModelError("AppointmentDate", "Please set an appointment between Tomorrow and " + DateTime.Now.AddYears(1).ToString("MMM/dd/yyyy"));


                    return View(appointment);

                }



                var uID = int.Parse(Session["UserID"].ToString());

                var userID = (from x in db.Users where x.UserID == uID select x.UserID).FirstOrDefault();
                appointment.UserID = userID;

                var status = (from a in db.Appointments
                              where uID == appointment.UserID && appointment.AppointmentDate == a.AppointmentDate && appointment.SlotID == a.SlotID
                              select a.AppointmentStateID).FirstOrDefault();

                //pending pero dapat 5


                var checkSpecific = (from u in db.Appointments
                                     where u.AppointmentDate == appointment.AppointmentDate && u.SlotID == appointment.SlotID && u.UserID == appointment.UserID && u.AppointmentStateID == 3
                                     || u.AppointmentDate == appointment.AppointmentDate && u.SlotID == appointment.SlotID && u.UserID == appointment.UserID && u.AppointmentStateID == 5
                                     select u).ToList();
                if (checkSpecific.Count >= 1)
                {
                    ViewBag.AppointmentStateID = new SelectList(db.AppointmentStates, "AppointmentStateID", "AppointmentStatus", appointment.AppointmentStateID);
                    ViewBag.ProductID = new SelectList(db.Products.Where(p => p.ProductTypeID == 2 && p.ProductStatus == true).OrderBy(p => p.ProductName), "ProductID", "ProductName", appointment.ProductID);
                    ViewBag.SlotID = new SelectList(db.Slots, "SlotsID", "Time", appointment.SlotID);
                    ViewBag.UserID = new SelectList(db.Users, "UserID", "Username", appointment.UserID);

                    if(status == 3)
                    {
                        ModelState.AddModelError("AppointmentDate", "You already have a Pending Appointment with the same schedule. Please pick another Date.");
                    }
                    if (status == 5)
                    {
                        ModelState.AddModelError("AppointmentDate", "You already have an Approved Appointment with the same schedule. Please pick another Date.");
                    }

                    return View(appointment);

                }


                //var checkDate = (from u in db.Appointments
                //                 where u.AppointmentDate == appointment.AppointmentDate && u.AppointmentStateID == 5 ||
                //                 u.AppointmentDate == appointment.AppointmentDate && u.AppointmentStateID ==3 ||
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
                    ViewBag.ProductID = new SelectList(db.Products.Where(p => p.ProductTypeID == 2 && p.ProductStatus == true).OrderBy(p => p.ProductName), "ProductID", "ProductName", appointment.ProductID);
                    ViewBag.SlotID = new SelectList(db.Slots, "SlotsID", "Time", appointment.SlotID);
                    ViewBag.UserID = new SelectList(db.Users, "UserID", "Username", appointment.UserID);


                    ModelState.AddModelError("AppointmentDate", "Appointment for this Date is Already Full. Please try another Date");


                    return View(appointment);

                }


                //var checkSlot = (from u in db.Appointments
                //                 where u.AppointmentDate == appointment.AppointmentDate &&  u.SlotID == appointment.SlotID && u.AppointmentStateID == 5 ||
                //                 u.AppointmentDate == appointment.AppointmentDate && u.SlotID == appointment.SlotID && u.AppointmentStateID == 3 ||
                //                 u.AppointmentDate == appointment.AppointmentDate && u.AppointmentStateID == 1
                //                 select u).ToList();

                var checkSlot = (from u in db.Appointments
                                 where u.AppointmentDate == appointment.AppointmentDate && u.SlotID == appointment.SlotID && u.AppointmentStateID == 5 && u.User.UserStatus == true ||
                                 u.AppointmentDate == appointment.AppointmentDate && u.SlotID == appointment.SlotID && u.AppointmentStateID == 3 && u.User.UserStatus == true ||
                                 u.AppointmentDate == appointment.AppointmentDate && u.AppointmentStateID == 1 && u.User.UserStatus == true
                                 select u).ToList();

                if (checkSlot.Count >= 8)
                {
                    var time = (from s in db.Slots where s.SlotsID == appointment.SlotID select s.Time).FirstOrDefault();
                    //var fullname = db.Slots.Where(u => u.RoleID == 4 || u.RoleID == 3).OrderBy(u => u.FirstName).ToList();
                    //IEnumerable<SelectListItem> selectList = from s in fullname
                    //                                         select new SelectListItem
                    //                                         {
                    //                                             Value = s.SlotID.ToString(),
                    //                                             Text = s.FirstName + " " + s.LastName
                    //                                         };
                    //ViewBag.SlotID = new SelectList(selectList, "Value", "Text");


                    ViewBag.AppointmentStateID = new SelectList(db.AppointmentStates, "AppointmentStateID", "AppointmentStatus", appointment.AppointmentStateID);
                    ViewBag.ProductID = new SelectList(db.Products.Where(p => p.ProductTypeID == 2 && p.ProductStatus == true).OrderBy(p => p.ProductName), "ProductID", "ProductName", appointment.ProductID);
                    ViewBag.SlotID = new SelectList(db.Slots, "SlotsID", "Time", appointment.SlotID);
                    ViewBag.UserID = new SelectList(db.Users, "UserID", "Username", appointment.UserID);


                    ModelState.AddModelError("SlotID", "Appointment for "+time+" is Already Full. Please pick Another Slot.");


                    return View(appointment);

                }



                //var checkPending = db.Appointments.Any(x => x.UserID == userID && x.AppointmentStateID ==3);
                //if (checkPending)
                //{
                //    ViewBag.AppointmentStateID = new SelectList(db.AppointmentStates, "AppointmentStateID", "AppointmentStatus", appointment.AppointmentStateID);
                //    ViewBag.ProductID = new SelectList(db.Products, "ProductID", "ProductName", appointment.ProductID);
                //    ViewBag.SlotID = new SelectList(db.Slots, "SlotsID", "Time", appointment.SlotID);
                //    ViewBag.UserID = new SelectList(db.Users, "UserID", "Username", appointment.UserID);


                //    ModelState.AddModelError("ProductID", "You still have ongoing appointment schedule. Please reschedule or cancel your appointment first before proceeding.");


                //    return View(appointment);

                //}
                var name = (from x in db.Users where x.UserID == appointment.UserID select x.FirstName).FirstOrDefault();
            
             
           
                appointment.AppointmentStateID = 3;
                db.Appointments.Add(appointment);
                var appID = appointment.AppointmentId;
                var date = appointment.AppointmentDate;
                string body = name + " has set an appointment on "+ date?.ToString("MMM / dd / yyyy")+".Please response to the request in http://www.viviessence.xyz/Appointments/PendingAppointment/";
                //SendEmail(body);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AppointmentStateID = new SelectList(db.AppointmentStates, "AppointmentStateID", "AppointmentStatus", appointment.AppointmentStateID);
            ViewBag.ProductID = new SelectList(db.Products.Where(p => p.ProductTypeID == 2 && p.ProductStatus == true).OrderBy(p => p.ProductName), "ProductID", "ProductName", appointment.ProductID);
            ViewBag.SlotID = new SelectList(db.Slots, "SlotsID", "Time", appointment.SlotID);
            ViewBag.UserID = new SelectList(db.Users, "UserID", "Username", appointment.UserID);
            return View(appointment);
        }

        // GET: CustomerAppointment/Edit/5
        public ActionResult Edit(int? id)
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
            ViewBag.AppointmentStateID = new SelectList(db.AppointmentStates, "AppointmentStateID", "AppointmentStatus", appointment.AppointmentStateID);
            ViewBag.ProductID = new SelectList(db.Products.Where(p => p.ProductTypeID == 2 && p.ProductStatus == true).OrderBy(p => p.ProductName), "ProductID", "ProductName", appointment.ProductID);
            ViewBag.SlotID = new SelectList(db.Slots, "SlotsID", "Time", appointment.SlotID);
            ViewBag.UserID = new SelectList(db.Users, "UserID", "Username", appointment.UserID);
            return View(appointment);
        }

        // POST: CustomerAppointment/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AppointmentId,UserID,ProductID,AppointmentDate,SlotID,Slots,Reason,AppointmentStateID")] Appointment appointment)/* [Bind(Include = "AppointmentId,UserID,ProductID,AppointmentDate,SlotID,Reason,AppointmentStateID")]*/
        {
            var uID = int.Parse(Session["UserID"].ToString());

            var userID = (from x in db.Users where x.UserID == uID select x.UserID).FirstOrDefault();


            appointment.UserID = userID;


            if (appointment.AppointmentDate >= DateTime.Now.AddYears(1) || appointment.AppointmentDate <= DateTime.Now)
            {
                ViewBag.AppointmentStateID = new SelectList(db.AppointmentStates, "AppointmentStateID", "AppointmentStatus", appointment.AppointmentStateID);
                ViewBag.ProductID = new SelectList(db.Products.Where(p => p.ProductTypeID == 2 && p.ProductStatus == true).OrderBy(p => p.ProductName), "ProductID", "ProductName", appointment.ProductID);
                ViewBag.SlotID = new SelectList(db.Slots, "SlotsID", "Time", appointment.SlotID);
                ViewBag.UserID = new SelectList(db.Users, "UserID", "Username", appointment.UserID);


                ModelState.AddModelError("AppointmentDate", "Please set an appointment between Tomorrow and " + DateTime.Now.AddYears(1).ToString("MMM/dd/yyyy"));


                return View(appointment);

            }



            var status = (from a in db.Appointments
                          where uID == appointment.UserID && appointment.AppointmentDate == a.AppointmentDate && appointment.SlotID == a.SlotID
                          select a.AppointmentStateID).FirstOrDefault();

            //pending pero dapat 5
            var tryss = (from u in db.Appointments
                         where u.AppointmentDate == appointment.AppointmentDate && u.SlotID == appointment.SlotID && u.UserID == appointment.UserID && u.AppointmentStateID == 3
                         select u).ToList();

            var checkSpecific = (from u in db.Appointments
                                 where u.AppointmentDate == appointment.AppointmentDate && u.SlotID == appointment.SlotID && u.UserID == appointment.UserID && u.AppointmentStateID == 3
                                 || u.AppointmentDate == appointment.AppointmentDate && u.SlotID == appointment.SlotID && u.UserID == appointment.UserID && u.AppointmentStateID == 5
                                 select u).ToList();
            if (checkSpecific.Count >= 1)
            {
                ViewBag.AppointmentStateID = new SelectList(db.AppointmentStates, "AppointmentStateID", "AppointmentStatus", appointment.AppointmentStateID);
                ViewBag.ProductID = new SelectList(db.Products.Where(p => p.ProductTypeID == 2 && p.ProductStatus == true).OrderBy(p => p.ProductName), "ProductID", "ProductName", appointment.ProductID);
                ViewBag.SlotID = new SelectList(db.Slots, "SlotsID", "Time", appointment.SlotID);
                ViewBag.UserID = new SelectList(db.Users, "UserID", "Username", appointment.UserID);

                if (status == 3)
                {
                    ModelState.AddModelError("AppointmentDate", "You already have a Pending Appointment with the same schedule. Please pick another Date.");
                }
                if (status == 5)
                {
                    ModelState.AddModelError("AppointmentDate", "You already have an Approved Appointment with the same schedule. Please pick another Date.");
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
                ViewBag.ProductID = new SelectList(db.Products.Where(p => p.ProductTypeID == 2 && p.ProductStatus == true).OrderBy(p => p.ProductName), "ProductID", "ProductName", appointment.ProductID);
                ViewBag.SlotID = new SelectList(db.Slots, "SlotsID", "Time", appointment.SlotID);
                ViewBag.UserID = new SelectList(db.Users, "UserID", "Username", appointment.UserID);


                ModelState.AddModelError("AppointmentDate", "Appointment for this Date is Already Full. Please try another Date");


                return View(appointment);

            }


            //var checkSlot = (from u in db.Appointments
            //                 where u.AppointmentDate == appointment.AppointmentDate && u.SlotID == appointment.SlotID && u.AppointmentStateID == 5 ||
            //                 u.AppointmentDate == appointment.AppointmentDate && u.SlotID == appointment.SlotID && u.AppointmentStateID == 3 ||
            //                 u.AppointmentDate == appointment.AppointmentDate && u.AppointmentStateID == 1
            //                 select u).ToList();


            var checkSlot = (from u in db.Appointments
                             where u.AppointmentDate == appointment.AppointmentDate && u.SlotID == appointment.SlotID && u.AppointmentStateID == 5 && u.User.UserStatus == true ||
                             u.AppointmentDate == appointment.AppointmentDate && u.SlotID == appointment.SlotID && u.AppointmentStateID == 3 && u.User.UserStatus == true ||
                             u.AppointmentDate == appointment.AppointmentDate && u.AppointmentStateID == 1 && u.User.UserStatus == true
                             select u).ToList();

            if (checkSlot.Count >= 8)
            {
                var time = (from s in db.Slots where s.SlotsID == appointment.SlotID select s.Time).FirstOrDefault();
                //var fullname = db.Slots.Where(u => u.RoleID == 4 || u.RoleID == 3).OrderBy(u => u.FirstName).ToList();
                //IEnumerable<SelectListItem> selectList = from s in fullname
                //                                         select new SelectListItem
                //                                         {
                //                                             Value = s.SlotID.ToString(),
                //                                             Text = s.FirstName + " " + s.LastName
                //                                         };
                //ViewBag.SlotID = new SelectList(selectList, "Value", "Text");


                ViewBag.AppointmentStateID = new SelectList(db.AppointmentStates, "AppointmentStateID", "AppointmentStatus", appointment.AppointmentStateID);
                ViewBag.ProductID = new SelectList(db.Products.Where(p => p.ProductTypeID == 2 && p.ProductStatus == true).OrderBy(p => p.ProductName), "ProductID", "ProductName", appointment.ProductID);
                ViewBag.SlotID = new SelectList(db.Slots, "SlotsID", "Time", appointment.SlotID);
                ViewBag.UserID = new SelectList(db.Users, "UserID", "Username", appointment.UserID);


                ModelState.AddModelError("SlotID", "Appointment for " + time + " is Already Full. Please pick Another Slot.");


                return View(appointment);

            }


            if (ModelState.IsValid)
            {
                var name = (from x in db.Users where x.UserID == appointment.UserID select x.FirstName).FirstOrDefault();
                var appID = appointment.AppointmentId;
                string body = name + " has rescheduled an appointment. Please response to the request in http://www.viviessence.xyz/Appointments/ApproveDeny/" + appID.ToString();

                appointment.AppointmentStateID = 3;
                //var reason = appointment.Reason;
                //appointment.Reason = appointment.Reason;
                appointment.Reason = "";
                //db.Appointments.Attach(appointment);

                db.Entry(appointment).State = EntityState.Modified;
                db.SaveChanges();


               

                //SendEmail(body);
                return RedirectToAction("Index");
            }
            ViewBag.AppointmentStateID = new SelectList(db.AppointmentStates, "AppointmentStateID", "AppointmentStatus", appointment.AppointmentStateID);
            ViewBag.ProductID = new SelectList(db.Products.Where(p => p.ProductTypeID == 2 && p.ProductStatus == true).OrderBy(p => p.ProductName), "ProductID", "ProductName", appointment.ProductID);
            ViewBag.SlotID = new SelectList(db.Slots, "SlotsID", "Time", appointment.SlotID);
            ViewBag.UserID = new SelectList(db.Users, "UserID", "Username", appointment.UserID);



            return View(appointment);
        }

        // GET: CustomerAppointment/Delete/5
        public ActionResult Delete(int? id)
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

            appointment.AppointmentStateID = 2;
            db.SaveChanges();
            return RedirectToAction("Pending");
        }

        // POST: CustomerAppointment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Appointment appointment = db.Appointments.Find(id);
            //db.Appointments.Remove(appointment);
            appointment.AppointmentStateID = 2;
            var reason = appointment.Reason;
            //appointment.Reason = appointment.Reason;
            db.SaveChanges();
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







        // GET: CustomerAppointmentTest/Edit/5
        public ActionResult Cancel(int? id)
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
            appointment.AppointmentStateID = 2;
            db.SaveChanges();
            return RedirectToAction("Index");
            ViewBag.AppointmentStateID = new SelectList(db.AppointmentStates, "AppointmentStateID", "AppointmentStatus", appointment.AppointmentStateID);
            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "ProductName", appointment.ProductID);
            ViewBag.SlotID = new SelectList(db.Slots, "SlotsID", "Time", appointment.SlotID);
            ViewBag.UserID = new SelectList(db.Users, "UserID", "Username", appointment.UserID);
            return View(appointment);
        }

        // POST: CustomerAppointmentTest/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cancel(Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                var uID = int.Parse(Session["UserID"].ToString());

                var userID = (from x in db.Users where x.UserID == uID select x.UserID).FirstOrDefault();
                var date = (from x in db.Appointments where x.AppointmentId == appointment.AppointmentId select x.AppointmentDate).FirstOrDefault();
                var slot = (from x in db.Appointments where x.AppointmentId == appointment.AppointmentId select x.SlotID).FirstOrDefault();
                var service = (from x in db.Appointments where x.AppointmentId == appointment.AppointmentId select x.ProductID).FirstOrDefault();
                appointment.UserID = userID;
                appointment.AppointmentDate = date;
                appointment.SlotID = slot;
                appointment.ProductID = service;
                
                appointment.AppointmentStateID = 2;
                db.Entry(appointment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AppointmentStateID = new SelectList(db.AppointmentStates, "AppointmentStateID", "AppointmentStatus", appointment.AppointmentStateID);
            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "ProductName", appointment.ProductID);
            ViewBag.SlotID = new SelectList(db.Slots, "SlotsID", "Time", appointment.SlotID);
            ViewBag.UserID = new SelectList(db.Users, "UserID", "Username", appointment.UserID);
            return View(appointment);
        }

        public ActionResult CancelApproved(int? id)
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

            appointment.AppointmentStateID = 2;
            db.SaveChanges();
            return RedirectToAction("Approved");
        }
        public ActionResult CancelPending(int? id)
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

            appointment.AppointmentStateID = 2;
            db.SaveChanges();
            return RedirectToAction("Pending");
        }


    }
}
