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
    public class CustomerRegistrationController : Controller
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
        public ActionResult emailVerification(int? id)
        {
            

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                User user = db.Users.Find(id);
                if (user == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    user.isVerified = true;
                    db.SaveChanges();
                    return RedirectToAction("EmailVerified");
                }
                //ViewBag.GenderID = new SelectList(db.Gender, "GenderID", "GenderType", user.GenderID);
                ViewBag.RoleID = new SelectList(db.UserRoles, "RoleID", "RoleName", user.RoleID);
                return View(user);
            }
           
        }

        // POST: CustomerRegistration/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult emailVerification(User user)
        {
            //TempData["SuccessMessage"] = "Your Success Message";

            var userID = (from x in db.Users where x.Username == user.Username select x.UserID).FirstOrDefault();
            var username = (from x in db.Users where x.UserID == userID select x.Username).FirstOrDefault();
            var password = (from x in db.Users where x.UserID == userID select x.Password).FirstOrDefault();
            var fname = (from x in db.Users where x.UserID == userID select x.FirstName).FirstOrDefault();
            var lname = (from x in db.Users where x.UserID == userID select x.LastName).FirstOrDefault();
            var gender = (from x in db.Users where x.UserID == userID select x.Gender).FirstOrDefault();
            var bdate = (from x in db.Users where x.UserID == userID select x.BirthDate).FirstOrDefault();
            var roleID = (from x in db.Users where x.UserID == userID select x.RoleID).FirstOrDefault();
            var email = (from x in db.Users where x.UserID == userID select x.Email).FirstOrDefault();
            var mobile = (from x in db.Users where x.UserID == userID select x.MobileNumber).FirstOrDefault();

            user.Username = username;
            user.FirstName = fname;
            user.LastName = lname;
            user.MobileNumber = mobile;
            user.UserStatus = true;
            user.Email = email;
            user.RoleID = roleID;
            user.Gender = gender;
            user.BirthDate = bdate;
            user.Password = password;
            user.isVerified = true;


            db.Entry(user).State = EntityState.Modified;

            db.SaveChanges();
            return RedirectToAction("Login", "Logs");

            //ViewBag.GenderID = new SelectList(db.Gender, "GenderID", "GenderType", user.GenderID);
            ViewBag.RoleID = new SelectList(db.UserRoles, "RoleID", "RoleName", user.RoleID);
            return RedirectToAction("Login", "Logs");
        }


        public ActionResult EmailVerified()
        {
          
            return View();
        }
        // GET: CustomerRegistration
        public ActionResult Index()
        {
            var users = db.Users.Include(u => u.UserRole);
            return View(users.ToList());
        }

        // GET: CustomerRegistration/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: CustomerRegistration/Create
        public ActionResult Create()
        {
            ViewBag.GenderID = new SelectList(db.Gender, "GenderID", "GenderType");
           
            ViewBag.RoleID = new SelectList(db.UserRoles, "RoleID", "RoleName");
            return View();
        }

        // POST: CustomerRegistration/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(User user)
        {
            //if (ModelState.IsValid)
            //{

            //var checkUsername = (from u in db.Users
            //                     where u.Username == user.Username select u).ToList();

            //if (checkUsername.Count == 1)
            //{

            //        ModelState.AddModelError("Username", "Username is already taken");
            //        return View();

            //}


            //     var checkUsername = db.Users.Any(x => x.Username == user.Username);
            //     if (checkUsername)
            //     {

            //         ModelState.AddModelError("Username", "Username is already taken");
            //         return View(user);

            //     }
            //else
            var roleID = (int)Session["id"];
            user.UserStatus = true;
                user.isVerified = false;
                user.RoleID = 4;
            //ViewBag.GenderID = new SelectList(db.Gender, "GenderID", "GenderType", user.GenderID);
            ViewBag.RoleID = new SelectList(db.UserRoles, "RoleID", "RoleName", user.RoleID);

            if (user.LastName == null)
            {

                ModelState.AddModelError("LastName", "This field is required.");
                return View(user);

            }

            var checkUsername = db.Users.Any(x => x.Username == user.Username);
            if (checkUsername)
            {

                ModelState.AddModelError("Username", "Username is already taken");
                return View(user);

            }
            var checkEmail = db.Users.Any(x => x.Email == user.Email);
            if (checkEmail)
            {

                ModelState.AddModelError("Email", "Email is already exist");
                return View(user);

            }
            var date = DateTime.Today;
            if( user.BirthDate > date ){
                ModelState.AddModelError("BirthDate", "BirthDate is invalid");
                return View(user);
            }

            user.RegDate = DateTime.Now;
            db.Users.Add(user);
            db.SaveChanges();

            //------------------------------------------send email

            var userID = (from x in db.Users where x.Username == user.Username select x.UserID).FirstOrDefault();
            var email = (from x in db.Users where x.UserID == userID select x.Email).FirstOrDefault();
            this.userEmail = email;

            //local
            //var link = "https://localhost:44366/CustomerRegistration/emailVerification/" +userID.ToString();
            //web
            var link = "http://viviessence.xyz/CustomerRegistration/emailVerification/" + userID.ToString();

            string body = "Hello please click this link: " + link + " to verify email";
            SendEmail(body);

            if (roleID == 2)
            {


                    return RedirectToAction("Index", "EmployeeHome");
                
            }
            else
            {


                return RedirectToAction("EmailVerify","Logs");
            }




        //    }

        ////var roleID = Session["RoleID"];


       
            return RedirectToAction("Login", "Logs");
    }

    // GET: CustomerRegistration/Edit/5
    public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            //ViewBag.GenderID = new SelectList(db.Gender, "GenderID", "GenderType", user.GenderID);
            ViewBag.RoleID = new SelectList(db.UserRoles, "RoleID", "RoleName", user.RoleID);
            return View(user);
        }

        // POST: CustomerRegistration/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserID,Username,Password,FirstName,LastName,GenderID,BirthDate,Email,MobileNumber,RegDate,RoleID,UserStatus,isVerified")] User user)
        {
            if (ModelState.IsValid)
            {
              
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //ViewBag.GenderID = new SelectList(db.Gender, "GenderID", "GenderType", user.GenderID);
            ViewBag.RoleID = new SelectList(db.UserRoles, "RoleID", "RoleName", user.RoleID);
            return View(user);
        }

        // GET: CustomerRegistration/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: CustomerRegistration/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
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
