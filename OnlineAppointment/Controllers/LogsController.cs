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
    public class LogsController : Controller
    {
        private OnlineAppointmentContext db = new OnlineAppointmentContext();


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

        String userEmail;
        public void SendEmail(string emailBody)
        {
            MailMessage mailMessage = new MailMessage("vivianinon@gmail.com", this.userEmail);
            mailMessage.Subject = "Email Verification";
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

        // GET: Logs/Login
        public ActionResult Login()
        {
            Session["id"] = null;
            ViewBag.UserID = new SelectList(db.Users, "UserID", "Username");
            return View();
        }

        // POST: Logs/Login
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Log log, User user)
        {
            bool test = false;
            //var userID = user.UserID;
            var userID = (from x in db.Users where x.Username == user.Username select x.UserID).FirstOrDefault();
            var roleID = (from x in db.Users where x.Username == user.Username select x.RoleID).FirstOrDefault();
            var email = (from x in db.Users where x.UserID == userID select x.Email).FirstOrDefault();

            var pass = (from x in db.Users where x.UserID == userID select x.Password).FirstOrDefault();


            if (pass == user.Password)
            {
                test = true;
            }

            //x.UserName.Equals(UserModel.UserName, StringComparison.Ordinal)

            var validate_user = (from u in db.Users
                                 where u.Username == user.Username && test == true && u.isVerified == true
                                 select u).ToList();

            //var validate_user = (from u in db.Users
            //                         where u.Username == user.Username && u.Password == user.Password && u.UserStatus == true && u.isVerified == true
            //                         select u).ToList();
            var validate_email = (from u in db.Users
                                  where u.Username == user.Username && test == true && u.isVerified != true
                                  select u).ToList();




            //userID = (int?)userID;






            Session["id"] = roleID;
            Session["UserID"] = userID;
            if (validate_email.Count == 1)
            {

                //var link = "https://localhost:44366/CustomerRegistration/emailVerification/" + userID.ToString();

                var link = "http://viviessence.xyz/CustomerRegistration/emailVerification/" + userID.ToString();

                string body = "Hello please click this link: " + link + " to verify Account";
                this.userEmail = email;
                SendEmail(body);
                ModelState.AddModelError("Password", "We've sent you an Email. Please Verify your Account.");
                return View();
                //return RedirectToAction("Index", "Logs");
            }


            else if (validate_user.Count == 1)
            {
                log.LogDate = DateTime.Now;
                log.UserID = userID;
                db.Logs.Add(log);
                db.SaveChanges();



                if (CheckSession())
                {
                    // FUNCTIONS
                    if (roleID == 1) //admin
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else if (roleID == 2) //employee
                    {
                        return RedirectToAction("Index", "EmployeeHome");
                    }
                    else if (roleID == 4) //customer
                    {
                        return RedirectToAction("Index", "CustomerHome");
                    }
                    return RedirectToAction("Index");
                }
                else
                {

                    return RedirectToAction("Index", "Login");
                }



            }

            else
            {
                ModelState.AddModelError("Password", "Invalid Username or Password");
                return View();
            }
            //db.Logs.Add(log);
            //db.SaveChanges();
            //return RedirectToAction("Index");
            //}

            //ViewBag.UserID = new SelectList(db.Users, "UserID", "Username", log.UserID);
            //return View(log);
        }

        public ActionResult EmailVerify()
        {

            return View();
        }




        // GET: Logs
        public ActionResult Index()
        {
            var logs = db.Logs.Include(l => l.User);
            //return View(logs.Where(l=>l.UserID != 2 && l.UserID != 3).OrderByDescending(l=> l.LogDate ).ToList());
            return View((logs.Where(l => l.User.RoleID != 1 && l.User.RoleID != 2)).OrderByDescending(l => l.LogDate).ToList());
        }

        // GET: Logs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Log log = db.Logs.Find(id);
            if (log == null)
            {
                return HttpNotFound();
            }
            return View(log);
        }

        // GET: Logs/Create
        public ActionResult Create()
        {
            ViewBag.UserID = new SelectList(db.Users, "UserID", "Username");
            return View();
        }

        // POST: Logs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LogID,UserID,LogDate")] Log log, User user)
        {
            var userID = (from x in db.Users where x.Username == user.Username select x.UserID).FirstOrDefault();
            var roleID = (from x in db.Users where x.Username == user.Username select x.RoleID).FirstOrDefault();
            var email = (from x in db.Users where x.UserID == userID select x.Email).FirstOrDefault();
            if (ModelState.IsValid)
            {
                var link = "http://viviessence.xyz/CustomerRegistration/emailVerification/" + userID.ToString();

                string body = "Hello please click this link: " + link + " to verify Account";
                this.userEmail = email;
                SendEmail(body);
                db.Logs.Add(log);
                db.SaveChanges();
               
            return RedirectToAction("Index"); 
               
            }

            ViewBag.UserID = new SelectList(db.Users, "UserID", "Username", log.UserID);
            return View(log);
        }

        // GET: Logs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Log log = db.Logs.Find(id);
            if (log == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserID = new SelectList(db.Users, "UserID", "Username", log.UserID);
            return View(log);
        }

        // POST: Logs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "LogID,UserID,LogDate")] Log log)
        {
            if (ModelState.IsValid)
            {
                db.Entry(log).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserID = new SelectList(db.Users, "UserID", "Username", log.UserID);
            return View(log);
        }

        // GET: Logs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Log log = db.Logs.Find(id);
            if (log == null)
            {
                return HttpNotFound();
            }
            return View(log);
        }

        // POST: Logs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Log log = db.Logs.Find(id);
            db.Logs.Remove(log);
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







    }
}
