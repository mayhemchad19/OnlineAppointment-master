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

    public class AppointmentsController : Controller
    {
        private OnlineAppointmentContext db = new OnlineAppointmentContext();
     
       
        public Boolean CheckSession()
        {
            if (Session["id"] == null)
            {
                Session["id"] = 0;
            }

            if (int.Parse(Session["id"].ToString()) != 1)
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
        public void SendEmail(string emailBody)
        {
            MailMessage mailMessage = new MailMessage("vivianinon@gmail.com", this.userEmail);
            mailMessage.Subject = "ViVi Online Appointment";
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
        // GET: Appointments
        public ActionResult Index()
        {
            var appointments = db.Appointments.Include(a => a.Slot).Include(a => a.User).Include(a=> a.AppointmentState);
            return View(appointments.Where(u=>u.User.UserStatus != false).ToList());            
        }

        public ActionResult Transaction()
        {
            List<Transactions> TransactionList = new List<Transactions>();

            var _transactions = from a in db.Appointments.Include("User").Include("Slot")
                                join ad in db.AppointmentDetails.Include("Product") on a.AppointmentId equals ad.AppointmentId
                                select new
                                {
                                    Id = a.AppointmentId,
                                    UName = a.User.LastName + ", " + a.User.FirstName,
                                    SlotTime = a.Slot.StartTime + " - " + a.Slot.EndTime,
                                    TotalDiscount = a.Discount,
                                    ScheduledDate = a.AppointmentDate,
                                    Total = db.AppointmentDetails.Where(x => x.AppointmentId == a.AppointmentId).Select(x => x.Total).Sum()
                                };

            foreach (var item in _transactions)
            {
                Transactions t = new Transactions();
                t.Id = item.Id;
                t.CustomerName = item.UName;
                t.SlotTime = item.SlotTime;
                t.ScheduledDate = item.ScheduledDate;
                t.Total = item.Total;
                t.Discount = item.TotalDiscount;
                TransactionList.Add(t);
            }

            return View(TransactionList);
        }

        public ActionResult AppointmentSummary(int? status)
        {
            ViewBag.State = "List of all Appointments";
            var appointments = db.Appointments.Include(a => a.AppointmentState).Include(a => a.Slot).Include(a => a.User);
            var s = status;
            if (status!=null){
                if(s == 1)
                {
                    ViewBag.State = "List of Arrived Appointments";
                }
                else if (s == 2)
                {
                    ViewBag.State = "List of Pending Appointments";
                }
                else if (s == 3)
                {
                    ViewBag.State = "List of Cancelled Appointments";
                }
                else if (s == 4)
                {
                    ViewBag.State = "List of No-Show Appointments";
                }
                return View(appointments.Where(a=> a.AppointmentStateID == s).ToList());
            }
          
                // FUNCTIONS
               
                return View(appointments.ToList());
         
        }

        public ActionResult Arrived()
        {
            var appointments = db.Appointments.Include(a => a.AppointmentState).Include(a => a.Slot).Include(a => a.User);
            //return View(sales.Where(s=> s.OrderDate == DateTime.Now).ToList());
            var status = 1;
            return RedirectToAction("AppointmentSummary", new
            {
                status
            });

        }
        public ActionResult Pending()
        {
            var appointments = db.Appointments.Include(a => a.AppointmentState).Include(a => a.Slot).Include(a => a.User);
            //return View(sales.Where(s=> s.OrderDate == DateTime.Now).ToList());
            var status = 3;
            return RedirectToAction("AppointmentSummary", new
            {
                status
            });

        }
        public ActionResult Cancelled()
        {
            var appointments = db.Appointments.Include(a => a.AppointmentState).Include(a => a.Slot).Include(a => a.User);
            //return View(sales.Where(s=> s.OrderDate == DateTime.Now).ToList());
            var status =2 ;
            return RedirectToAction("AppointmentSummary", new
            {
                status
            });

        }
        public ActionResult NoShow()
        {
            var appointments = db.Appointments.Include(a => a.AppointmentState).Include(a => a.Slot).Include(a => a.User);
            //return View(sales.Where(s=> s.OrderDate == DateTime.Now).ToList());
            var status =4;
            return RedirectToAction("AppointmentSummary", new
            {
                status
            });

        }

        // GET: Appointments/Details/5
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
            var date = appointment.AppointmentDate;
            var slotID = (from x in db.Appointments where x.AppointmentId == id select x.SlotID).FirstOrDefault();
            var time = (from x in db.Slots where x.SlotsID == slotID select x.Time).FirstOrDefault();
            var name = (from x in db.Users where x.UserID == uID select x.FirstName).FirstOrDefault();
            this.userEmail = (from x in db.Users where x.UserID == uID select x.Email).FirstOrDefault();

            string body = "Hello " + name.ToString() + " you have an appointment on " + date?.ToString("MMM / dd / yyyy") + " at "  + time;
            SendEmail(body);


            return RedirectToAction("TodaysAppointment");
        }

        // GET: Appointments/Create
        public ActionResult Create(int? id)
        {
            ViewBag.ProductID = new SelectList(db.Products.Where(p => p.ProductTypeID != 1 && p.ProductStatus != false).OrderBy(p => p.ProductTypeID), "ProductID", "ProductName");
            ViewBag.SlotID = new SelectList(db.Slots, "SlotsID", "Schedule");
            ViewBag.UserID = new SelectList(db.Users.Where(x => x.UserID == id), "UserID", "Name");
            return View();
        }

        // POST: Appointments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AppointmentId,UserID,AppointmentDate,SlotID,Reason,Status,ProductID")] Appointment appointment, int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (ModelState.IsValid)
            {

                if (appointment.AppointmentDate >= DateTime.Now.AddYears(1) || appointment.AppointmentDate <= DateTime.Now)
                {
                  
                    ViewBag.ProductID = new SelectList(db.Products.Where(p => p.ProductTypeID != 1 && p.ProductStatus == true).OrderBy(p => p.ProductName), "ProductID", "ProductName", appointment.ProductID);
                    ViewBag.SlotID = new SelectList(db.Slots, "SlotsID", "Schedule");
                    ViewBag.UserID = new SelectList(db.Users.Where(x => x.UserID == id), "UserID", "Name");


                    ModelState.AddModelError("AppointmentDate", "Please set an appointment between Tomorrow and " + DateTime.Now.AddYears(1).ToString("MMM/dd/yyyy"));


                    return View(appointment);

                }


                var status = (from a in db.Appointments
                              where id == appointment.UserID && appointment.AppointmentDate == a.AppointmentDate && appointment.SlotID == a.SlotID
                              select a.AppointmentStateID).FirstOrDefault();

                //pending pero dapat 5


                var checkSpecific = (from u in db.Appointments
                                     where u.AppointmentDate == appointment.AppointmentDate && u.SlotID == appointment.SlotID && u.UserID == appointment.UserID
                                     select u).ToList();
                if (checkSpecific.Count >= 1)
                {
                    ViewBag.ProductID = new SelectList(db.Products.Where(p => p.ProductTypeID != 1 && p.ProductStatus == true).OrderBy(p => p.ProductName), "ProductID", "ProductName", appointment.ProductID);
                    ViewBag.SlotID = new SelectList(db.Slots, "SlotsID", "Schedule");
                    ViewBag.UserID = new SelectList(db.Users.Where(x => x.UserID == id), "UserID", "Name");
                    if (status == 3)
                    {
                        ModelState.AddModelError("AppointmentDate",  "The customer already has a Pending Appointment with the same schedule. Please pick another Date.");
                    }
                    if (status == 5)
                    {
                        ModelState.AddModelError("AppointmentDate", "The customer already has an Approved Appointment with the same schedule. Please pick another Date.");
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
                    ViewBag.ProductID = new SelectList(db.Products.Where(p => p.ProductTypeID != 1 && p.ProductStatus == true).OrderBy(p => p.ProductName), "ProductID", "ProductName", appointment.ProductID);
                    ViewBag.SlotID = new SelectList(db.Slots, "SlotsID", "Schedule");
                    ViewBag.UserID = new SelectList(db.Users.Where(x => x.UserID == id), "UserID", "Name");


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
                    //var time = (from s in db.Slots where s.SlotsID == appointment.SlotID select s.Time).FirstOrDefault();
                    //var fullname = db.Slots.Where(u => u.RoleID == 4 || u.RoleID == 3).OrderBy(u => u.FirstName).ToList();
                    //IEnumerable<SelectListItem> selectList = from s in fullname
                    //                                         select new SelectListItem
                    //                                         {
                    //                                             Value = s.SlotID.ToString(),
                    //                                             Text = s.FirstName + " " + s.LastName
                    //                                         };
                    //ViewBag.SlotID = new SelectList(selectList, "Value", "Text");


                    ViewBag.ProductID = new SelectList(db.Products.Where(p => p.ProductTypeID != 1 && p.ProductStatus == true).OrderBy(p => p.ProductName), "ProductID", "ProductName", appointment.ProductID);
                    ViewBag.SlotID = new SelectList(db.Slots, "SlotsID", "Schedule");
                    ViewBag.UserID = new SelectList(db.Users.Where(x => x.UserID == id), "UserID", "Name");


                    ModelState.AddModelError("SlotID", "Appointment for SLOT is Already Full. Please pick Another Slot.");


                    return View(appointment);

                }

                appointment.Status = "Approved";
                appointment.AppointmentStateID = 5;
                db.Appointments.Add(appointment);
                db.SaveChanges();
                ModelState.Clear();
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }

            ViewBag.SlotID = new SelectList(db.Slots, "SlotsID", "Time", appointment.SlotID);
            ViewBag.UserID = new SelectList(db.Users, "UserID", "Name", appointment.UserID);
            
            return RedirectToAction("Index");           
        }

        //// POST: Appointments/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "AppointmentId,UserID,ProductID,AppointmentDate,SlotID,AppointmentStateID")] Appointment appointment)
        //{
        //    var fullname = db.Users.Where(u => u.RoleID == 4 && u.UserStatus != false || u.RoleID == 3 && u.UserStatus != false).OrderBy(u => u.FirstName).ToList();
        //    IEnumerable<SelectListItem> selectList = from s in fullname
        //                                             select new SelectListItem
        //                                             {
        //                                                 Value = s.UserID.ToString(),
        //                                                 Text = s.FirstName + " " + s.LastName
        //                                             };

        //    if (ModelState.IsValid)
        //    {

        //        appointment.AppointmentStateID = 3;



        //        var userID = appointment.UserID;
        //        var slotID = appointment.SlotID;
        //        var date = appointment.AppointmentDate;
        //        var productID = appointment.ProductID;
        //        var service = (from x in db.Products where x.ProductID == productID select x.ProductName).FirstOrDefault();
        //        //var date2 = date?.ToString("dd/M/yyyy");
        //        var time = (from x in db.Slots where x.SlotsID == slotID select x.Time).FirstOrDefault();
        //        var name = (from x in db.Users where x.UserID == userID select x.FirstName).FirstOrDefault();
        //        this.userEmail = (from x in db.Users where x.UserID == userID select x.Email).FirstOrDefault();

        //        string body = "Hello " + name.ToString() + " you have been scheduled an appointmnet for " + service + " on " + date?.ToString("MMM/dd/yyyy") + " at " + time + ". Please visit our website http://viviessence.xyz/ for more information.";

        //        SendEmail(body);
        //        db.Appointments.Add(appointment);







        //        //------------------------------------------------------------------------
        //        //var userID = (from x in db.Users where x.UserID == uID select x.UserID).FirstOrDefault();
        //        appointment.UserID = userID;

        //        var status = (from a in db.Appointments
        //                      where userID == appointment.UserID && appointment.AppointmentDate == a.AppointmentDate && appointment.SlotID == a.SlotID
        //                      select a.AppointmentStateID).FirstOrDefault();

        //        //pending pero dapat 5


        //        var checkSpecific = (from u in db.Appointments
        //                             where u.AppointmentDate == appointment.AppointmentDate && u.SlotID == appointment.SlotID && u.UserID == appointment.UserID
        //                             select u).ToList();
        //        if (checkSpecific.Count >= 1)
        //        {
        //            ViewBag.AppointmentStateID = new SelectList(db.AppointmentStates, "AppointmentStateID", "AppointmentStatus", appointment.AppointmentStateID);
        //            ViewBag.ProductID = new SelectList(db.Products.Where(p => p.ProductTypeID != 1 && p.ProductStatus == true).OrderBy(p => p.ProductName), "ProductID", "ProductName", appointment.ProductID);
        //            ViewBag.SlotID = new SelectList(db.Slots, "SlotsID", "Time", appointment.SlotID);
        //            ViewBag.UserID = new SelectList(selectList, "Value", "Text");

        //            if (status == 3)
        //            {
        //                ModelState.AddModelError("AppointmentDate", name + " already has a Pending Appointment with the same schedule. Please pick another Date.");
        //            }
        //            if (status == 5)
        //            {
        //                ModelState.AddModelError("AppointmentDate", name + " already has an Approved Appointment with the same schedule. Please pick another Date.");
        //            }

        //            return View(appointment);

        //        }
        //        //var checkDate = (from u in db.Appointments
        //        //                 where u.AppointmentDate == appointment.AppointmentDate && u.AppointmentStateID == 5 ||
        //        //                 u.AppointmentDate == appointment.AppointmentDate && u.AppointmentStateID == 3 ||
        //        //                 u.AppointmentDate == appointment.AppointmentDate && u.AppointmentStateID == 1
        //        //                 select u).ToList();

        //        var checkDate = (from u in db.Appointments
        //                         where u.AppointmentDate == appointment.AppointmentDate && u.AppointmentStateID == 5 && u.User.UserStatus == true ||
        //                         u.AppointmentDate == appointment.AppointmentDate && u.AppointmentStateID == 3 && u.User.UserStatus == true ||
        //                         u.AppointmentDate == appointment.AppointmentDate && u.AppointmentStateID == 1 && u.User.UserStatus == true
        //                         select u).ToList();
        //        if (checkDate.Count >= 88)
        //        {
        //            ViewBag.AppointmentStateID = new SelectList(db.AppointmentStates, "AppointmentStateID", "AppointmentStatus", appointment.AppointmentStateID);
        //            ViewBag.ProductID = new SelectList(db.Products.Where(p => p.ProductTypeID != 1 && p.ProductStatus == true).OrderBy(p => p.ProductName), "ProductID", "ProductName", appointment.ProductID);
        //            ViewBag.SlotID = new SelectList(db.Slots, "SlotsID", "Time", appointment.SlotID);
        //            ViewBag.UserID = new SelectList(selectList, "Value", "Text");


        //            ModelState.AddModelError("AppointmentDate", "Appointment for this Date is Already Full. Please try another Date");


        //            return View(appointment);

        //        }
        //        //var checkSlot = (from u in db.Appointments
        //        //                 where u.AppointmentDate == appointment.AppointmentDate && u.SlotID == appointment.SlotID && u.AppointmentStateID == 5 ||
        //        //                 u.AppointmentDate == appointment.AppointmentDate && u.SlotID == appointment.SlotID && u.AppointmentStateID == 3 ||
        //        //                 u.AppointmentDate == appointment.AppointmentDate && u.AppointmentStateID == 1
        //        //                 select u).ToList();

        //        var checkSlot = (from u in db.Appointments
        //                         where u.AppointmentDate == appointment.AppointmentDate && u.SlotID == appointment.SlotID && u.AppointmentStateID == 5 && u.User.UserStatus == true ||
        //                         u.AppointmentDate == appointment.AppointmentDate && u.SlotID == appointment.SlotID && u.AppointmentStateID == 3 && u.User.UserStatus == true ||
        //                         u.AppointmentDate == appointment.AppointmentDate && u.AppointmentStateID == 1 && u.User.UserStatus == true
        //                         select u).ToList();

        //        if (checkSlot.Count >= 8)
        //        {
        //            //var time = (from s in db.Slots where s.SlotsID == appointment.SlotID select s.Time).FirstOrDefault();
        //            //var fullname = db.Slots.Where(u => u.RoleID == 4 || u.RoleID == 3).OrderBy(u => u.FirstName).ToList();
        //            //IEnumerable<SelectListItem> selectList = from s in fullname
        //            //                                         select new SelectListItem
        //            //                                         {
        //            //                                             Value = s.SlotID.ToString(),
        //            //                                             Text = s.FirstName + " " + s.LastName
        //            //                                         };
        //            //ViewBag.SlotID = new SelectList(selectList, "Value", "Text");


        //            ViewBag.AppointmentStateID = new SelectList(db.AppointmentStates, "AppointmentStateID", "AppointmentStatus", appointment.AppointmentStateID);
        //            ViewBag.ProductID = new SelectList(db.Products.Where(p => p.ProductTypeID != 1 && p.ProductStatus == true).OrderBy(p => p.ProductName), "ProductID", "ProductName", appointment.ProductID);
        //            ViewBag.SlotID = new SelectList(db.Slots, "SlotsID", "Time", appointment.SlotID);
        //            ViewBag.UserID = new SelectList(selectList, "Value", "Text");


        //            ModelState.AddModelError("SlotID", "Appointment for " + time + " is Already Full. Please pick Another Slot.");


        //            return View(appointment);

        //        }
        //        //------------------------------------------------------------------------






        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }


        //    ViewBag.AppointmentStateID = new SelectList(db.AppointmentStates, "AppointmentStateID", "AppointmentStatus", appointment.AppointmentStateID);
        //    ViewBag.ProductID = new SelectList(db.Products.Where(p => p.ProductTypeID != 1 && p.ProductStatus == true).OrderBy(p => p.ProductName), "ProductID", "ProductName", appointment.ProductID);
        //    ViewBag.SlotID = new SelectList(db.Slots, "SlotsID", "Time", appointment.SlotID);
        //    ViewBag.UserID = new SelectList(db.Users, "UserID", "Username", appointment.User);
        //    return View(appointment);
        //}

        // GET: Appointments/Edit/5

        public ActionResult Edit(int? id)
        {
            Appointment appointment = db.Appointments.Find(id);

            ViewBag.SlotID = new SelectList(db.Slots.Where(x => x.SlotsID == appointment.SlotID), "SlotsID", "Schedule");
            ViewBag.UserID = new SelectList(db.Users.Where(x => x.UserID == appointment.UserID), "UserID", "Name");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            
            if (appointment == null)
            {
                return HttpNotFound();
            }

            return View(appointment);

        }

        // POST: Appointments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AppointmentId,AppointmentDate,UserID,Status")] Appointment appointment, int id)
        {
            if (ModelState.IsValid)
            {
                var _appointment = db.Appointments.Find(id);
                _appointment.Status = appointment.Status;

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.SlotID = new SelectList(db.Slots, "SlotsID", "Time", appointment.SlotID);
            ViewBag.UserID = new SelectList(db.Users, "UserID", "Name", appointment.UserID);

            return RedirectToAction("Index");
        }

        public ActionResult Reschedule(int? id)
        {
            Appointment appointment = db.Appointments.Find(id);

            ViewBag.SlotID = new SelectList(db.Slots.Where(x => x.SlotsID == appointment.SlotID), "SlotsID", "Schedule");
            ViewBag.UserID = new SelectList(db.Users.Where(x => x.UserID == appointment.UserID), "UserID", "Name");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            if (appointment == null)
            {
                return HttpNotFound();
            }

            return View(appointment);

        }

        // POST: Appointments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Reschedule([Bind(Include = "AppointmentId,AppointmentDate,UserID")] Appointment appointment, int id)
        {
            if (ModelState.IsValid)
            {
                var _appointment = db.Appointments.Find(id);
                _appointment.AppointmentDate = appointment.AppointmentDate;

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.SlotID = new SelectList(db.Slots, "SlotsID", "Time", appointment.SlotID);
            ViewBag.UserID = new SelectList(db.Users, "UserID", "Name", appointment.UserID);

            return RedirectToAction("Index");
        }

        // GET: Appointments/Delete/5
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

        // POST: Appointments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Appointment appointment = db.Appointments.Find(id);
            db.Appointments.Remove(appointment);
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



        public ActionResult Resched(int? id)
        {
            var fullname = db.Users.Where(u => u.RoleID == 4 && u.UserStatus != false || u.RoleID == 3 && u.UserStatus != false).OrderBy(u => u.FirstName).ToList();
            IEnumerable<SelectListItem> selectList = from s in fullname
                                                     select new SelectListItem
                                                     {
                                                         Value = s.UserID.ToString(),
                                                         Text = s.FirstName + " " + s.LastName
                                                     };

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
            ViewBag.ProductID = new SelectList(db.Products.Where(p => p.ProductTypeID != 1 && p.ProductStatus == true).OrderBy(p => p.ProductName), "ProductID", "ProductName", appointment.ProductID);
            ViewBag.SlotID = new SelectList(db.Slots, "SlotsID", "Time", appointment.SlotID);
            ViewBag.UserID = new SelectList(selectList, "Value", "Text");
            return View(appointment);
        }

        // POST: Appointments1/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Resched([Bind(Include = "AppointmentId,UserID,ProductID,AppointmentDate,SlotID,Reason,AppointmentStateID")] Appointment appointment)
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


             


                var userID = appointment.UserID;
                var slotID = appointment.SlotID;
                var date = appointment.AppointmentDate;
                var productID = appointment.ProductID;
                var service = (from x in db.Products where x.ProductID == productID select x.ProductName).FirstOrDefault();
                //var date2 = date?.ToString("dd/M/yyyy");
                var time = (from x in db.Slots where x.SlotsID == slotID select x.Time).FirstOrDefault();
                var name = (from x in db.Users where x.UserID == userID select x.FirstName).FirstOrDefault();
        
                this.userEmail = (from x in db.Users where x.UserID == userID select x.Email).FirstOrDefault();

                string body = "Hello " + name.ToString() + " your appointmnet for " + service + " has been rescheduled on " + date?.ToString("MMM/dd/yyyy") + " at " + time + ". Please visit our website http://viviessence.xyz/ for more information.";

                SendEmail(body);
                db.Appointments.Add(appointment);







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
                        ModelState.AddModelError("AppointmentDate", name + " already has a Pending Appointment with the same schedule. Please pick another Date.");
                    }
                    if (status == 5)
                    {
                        ModelState.AddModelError("AppointmentDate", name + " already has an Approved Appointment with the same schedule. Please pick another Date.");
                    }

                    return View(appointment);

                }


                //var checkDate = (from u in db.Appointments
                //                 where u.AppointmentDate == appointment.AppointmentDate && u.AppointmentStateID == 5 || u.AppointmentDate == appointment.AppointmentDate && u.AppointmentStateID == 3
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
                //                 where u.AppointmentDate == appointment.AppointmentDate && u.SlotID == appointment.SlotID && u.AppointmentStateID == 5 || u.AppointmentDate == appointment.AppointmentDate && u.SlotID == appointment.SlotID && u.AppointmentStateID == 3
                //                 select u).ToList();

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


                appointment.AppointmentStateID = status;


                
                db.Entry(appointment).State = EntityState.Unchanged;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AppointmentStateID = new SelectList(db.AppointmentStates, "AppointmentStateID", "AppointmentStatus", appointment.AppointmentStateID);
            ViewBag.ProductID = new SelectList(db.Products.Where(p => p.ProductTypeID != 1 && p.ProductStatus == true).OrderBy(p => p.ProductName), "ProductID", "ProductName", appointment.ProductID);
            ViewBag.SlotID = new SelectList(db.Slots, "SlotsID", "Time", appointment.SlotID);
            ViewBag.UserID = new SelectList(db.Users, "UserID", "Username", appointment.UserID);
            return View(appointment);
        }


        public ActionResult TodaysAppointment()
        {
            var lessdate = DateTime.Today.AddDays(-1);
            var greatdate = DateTime.Today.AddDays(1);
            var appointments = db.Appointments.Include(a => a.AppointmentState).Include(a => a.Product).Include(a => a.Slot).Include(a => a.User);
            return View(appointments.Where(a => a.AppointmentDate < greatdate && a.AppointmentDate > lessdate && a.AppointmentStateID == 1 || a.AppointmentDate < greatdate && a.AppointmentDate > lessdate && a.AppointmentStateID == 5 || a.AppointmentDate < greatdate && a.AppointmentDate > lessdate && a.AppointmentStateID == 4).OrderBy(b => b.SlotID).ToList());

        }



        public ActionResult PendingAppointment()
        {

                // FUNCTIONS
                var appointments = db.Appointments.Include(a => a.AppointmentState).Include(a => a.Product).Include(a => a.Slot).Include(a => a.User);
                return View(appointments.Where(a=> a.AppointmentStateID ==3 && a.AppointmentDate >= DateTime.Today).ToList());      

        }


        public ActionResult UpcomingAppointments()
        {
            var appointments = db.Appointments.Include(a => a.AppointmentState).Include(a => a.Product).Include(a => a.Slot).Include(a => a.User);
            return View(appointments.Where(a => a.AppointmentStateID == 5 && a.AppointmentDate >= DateTime.Today).OrderBy(b => b.AppointmentDate).ThenBy(c => c.SlotID).ToList());
            //return View(appointments.Where(a => a.AppointmentStateID == 3).ToList());
        }



        // GET: CustomerAppointmentTest/Edit/5
        public ActionResult ApproveDeny(int? id)
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
            ViewBag.AppointmentStateID = new SelectList(db.AppointmentStates.Where(a=>a.AppointmentStateID == 5 || a.AppointmentStateID == 6), "AppointmentStateID", "AppointmentStatus", appointment.AppointmentStateID);
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
        public ActionResult ApproveDeny([Bind(Include = "AppointmentId,UserID,ProductID,AppointmentDate,SlotID,Reason,AppointmentStateID")] Appointment appointment)
        {
          
            var appID = appointment.AppointmentId;
            var uid = (from x in db.Appointments where x.AppointmentId == appID select x.UserID).FirstOrDefault();
            //appointment.UserID = (from x in db.Appointments where x.AppointmentId == appID select x.UserID).FirstOrDefault();
            var date = (from x in db.Appointments where x.AppointmentId == appID select x.AppointmentDate).FirstOrDefault();
            var time = (from x in db.Appointments where x.AppointmentId == appID select x.SlotID).FirstOrDefault();
            var service = (from x in db.Appointments where x.AppointmentId == appID select x.ProductID).FirstOrDefault();
            var name = (from x in db.Users where x.UserID == uid select x.FirstName).FirstOrDefault();
            var reason = appointment.Reason;
            this.userEmail = (from x in db.Users where x.UserID == uid select x.Email).FirstOrDefault();
            //if (ModelState.IsValid)
            //{

            //var name = (from x in db.Users where x.UserID == userID select x.FirstName).FirstOrDefault();
            if (appointment.AppointmentStateID == 6)
            {
                if (reason == null)
                {
                    ModelState.AddModelError("Reason", "Please provide a reason for denying the Appointment");

                    ViewBag.AppointmentStateID = new SelectList(db.AppointmentStates.Where(a => a.AppointmentStateID == 5 || a.AppointmentStateID == 6), "AppointmentStateID", "AppointmentStatus", appointment.AppointmentStateID);
                    ViewBag.ProductID = new SelectList(db.Products, "ProductID", "ProductName", appointment.ProductID);
                    ViewBag.SlotID = new SelectList(db.Slots, "SlotsID", "Time", appointment.SlotID);
                    ViewBag.UserID = new SelectList(db.Users, "UserID", "Username", appointment.UserID);
                    return View(appointment);
                }
            }



            appointment.UserID = uid;
                appointment.AppointmentDate = date;
                appointment.SlotID = time;
                appointment.ProductID = service;

            if (appointment.AppointmentStateID == 5)
            {
                string body = "Hello " + name.ToString() + " your appointment for " + date?.ToString("MMM / dd / yyyy") + " at " + time + " has been Approved." +
                                "Plese visit our site http://www.viviessence.xyz for more information.";
                SendEmail(body);
            }
            if (appointment.AppointmentStateID == 6)
            {
                string body = "Hello " + name.ToString() + " your appointment for " + date?.ToString("MMM / dd / yyyy") + " at " + time + " has been Denied due to " +reason +
                                ". Plese visit our site http://www.viviessence.xyz for more information.";
                SendEmail(body);
            }

           

            db.Entry(appointment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("PendingAppointment");
            //}
            ViewBag.AppointmentStateID = new SelectList(db.AppointmentStates.Where(a => a.AppointmentStateID == 5 || a.AppointmentStateID == 6), "AppointmentStateID", "AppointmentStatus", appointment.AppointmentStateID);
            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "ProductName", appointment.ProductID);
            ViewBag.SlotID = new SelectList(db.Slots, "SlotsID", "Time", appointment.SlotID);
            ViewBag.UserID = new SelectList(db.Users, "UserID", "Username", appointment.UserID);
            //return View(appointment);
        }


        public ActionResult ArrivedStatus(int? id)
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

            appointment.AppointmentStateID = 1;
            db.SaveChanges();
            return RedirectToAction("TodaysAppointment");
        }
        public ActionResult NoShowStatus(int? id)
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

            appointment.AppointmentStateID = 4;
            db.SaveChanges();
            return RedirectToAction("TodaysAppointment");
        }

        //public ActionResult SendReminder(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Appointment appointment = db.Appointments.Find(id);
        //    if (appointment == null)
        //    {
        //        return HttpNotFound();
        //    }

        //    appointment.AppointmentStateID = 4;
        //    db.SaveChanges();
        //    return RedirectToAction("TodaysAppointment");
        //}
    }
}
