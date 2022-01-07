using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OnlineAppointment.Models;

namespace OnlineAppointment.Controllers
{
    public class UsersController : Controller
    {
        private OnlineAppointmentContext db = new OnlineAppointmentContext();


        // GET: Users
        public ActionResult Index()
        {
            var users = db.Users.Include(u => u.UserRole);
            return View(users.ToList().Where(u=>u.UserStatus == true));

        }

        public ActionResult CustomerIndex()
        {
            var users = db.Users.Include(u => u.UserRole);
            return View(users.ToList().Where(u => u.UserStatus == true && u.UserRole.RoleName == "Customer" && u.isVerified == true));

        }
        public ActionResult VIP()
        {
            var users = db.Users.Include(u => u.UserRole);
            return View(users.ToList().Where(u=>u.RoleID ==3 && u.UserStatus ==true));

        }

        // GET: Users/Details/5
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

        // GET: Users/Create
        public ActionResult Create()
        {
            //ViewBag.GenderID = new SelectList(db.Gender, "GenderID", "GenderType");
            ViewBag.RoleID = new SelectList(db.UserRoles, "RoleID", "RoleName");
            return View();
        }



        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserID,Username,Password,FirstName,LastName,GenderID,BirthDate,Email,MobileNumber,RoleID,UserStatus")] User user)
        {
            if (ModelState.IsValid)
            {
                user.UserStatus = true;
                user.isVerified = false;
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            //ViewBag.GenderID = new SelectList(db.Gender, "GenderID", "GenderType", user.GenderID);
            ViewBag.RoleID = new SelectList(db.UserRoles, "RoleID", "RoleName", user.RoleID);
            return View(user);
        }
      
        // GET: Users/Edit/5
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

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserID,Username,Password,FirstName,LastName,GenderID,BirthDate,Email,MobileNumber,RoleID,UserStatus")] User user)
        {
            if (ModelState.IsValid)
            {
                user.isVerified = (from x in db.Users where x.UserID == user.UserID select x.isVerified).SingleOrDefault();
                user.UserStatus = (from x in db.Users where x.UserID == user.UserID select x.UserStatus).SingleOrDefault();
              user.RegDate = (from x in db.Users where x.UserID == user.UserID select x.RegDate).SingleOrDefault();
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //ViewBag.GenderID = new SelectList(db.Gender, "GenderID", "GenderType", user.GenderID);
            ViewBag.BirthDate = new SelectList(db.Users, "BirthDate", "BirthDate", user.BirthDate);
            ViewBag.RoleID = new SelectList(db.UserRoles, "RoleID", "RoleName", user.RoleID);
            return View(user);
        }

        // GET: Users/Delete/5
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

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            user.UserStatus = false;
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



        public ActionResult TryDataTable()
        {
            //var users = db.Users.Include(u => u.Gender).Include(u => u.UserRole);
            return View(/*users.ToList()*/);

        }
    }
}
