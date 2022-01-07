using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OnlineAppointment.Models;

namespace OnlineAppointment.Controllers
{
    public class CustomerUsersController : Controller
    {
        private OnlineAppointmentContext db = new OnlineAppointmentContext();
       

        // GET: CustomerUsers
        public ActionResult Index()
        {
            var users = db.Users.Include(u => u.UserRole);

            var uID = int.Parse(Session["UserID"].ToString());


            return View(users.Where(u => u.UserID == uID).ToList());
        }

        // GET: CustomerUsers/Details/5
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

        // GET: CustomerUsers/Create
        public ActionResult Create()
        {
            ViewBag.GenderID = new SelectList(db.Gender, "GenderID", "GenderType");
            ViewBag.RoleID = new SelectList(db.UserRoles, "RoleID", "RoleName");
            return View();
        }

        // POST: CustomerUsers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserID,Username,Password,FirstName,LastName,GenderID,BirthDate,Email,MobileNumber,RoleID,UserStatus,isVerified")] User user)
        {
            if (ModelState.IsValid)
            {
                //user.RoleID = 4;//customer
                //user.isVerified = false;
                //user.UserStatus = true;
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            //ViewBag.GenderID = new SelectList(db.Gender, "GenderID", "GenderType", user.GenderID);
            ViewBag.RoleID = new SelectList(db.UserRoles, "RoleID", "RoleName", user.RoleID);
            return View(user);
        }

        // GET: CustomerUsers/Edit/5
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

        // POST: CustomerUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( User user)
        {
            //if (ModelState.IsValid)
            //{
                var uID = int.Parse(Session["UserID"].ToString());
                var userID = (from x in db.Users where x.UserID == uID select x.UserID).FirstOrDefault();

                var pass = (from x in db.Users where x.UserID == uID  select x.Password).FirstOrDefault();
                var bday = (from x in db.Users where x.UserID == uID select x.BirthDate).FirstOrDefault();
            var gender = (from x in db.Users where x.UserID == uID select x.Gender).FirstOrDefault();
            var email = (from x in db.Users where x.UserID == uID select x.Email).FirstOrDefault();
            var reg = (from x in db.Users where x.UserID == uID select x.RegDate).FirstOrDefault();

            user.RegDate = reg;
            user.Gender = gender;
            user.isVerified = true;
                user.Email = email;
                user.RoleID = 4;
                user.Password = pass;
                user.BirthDate = bday;
                user.UserStatus = true;
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            //}
            //ViewBag.GenderID = new SelectList(db.Gender, "GenderID", "GenderType", user.GenderID);
            ViewBag.RoleID = new SelectList(db.UserRoles, "RoleID", "RoleName", user.RoleID);
            return View(user);
        }

        // GET: CustomerUsers/Delete/5
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

        // POST: CustomerUsers/Delete/5
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
