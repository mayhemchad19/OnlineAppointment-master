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
    public class EmployeeUsersController : Controller
    {
        private OnlineAppointmentContext db = new OnlineAppointmentContext();



        // GET: EmployeeUsers
        public ActionResult Index()
        {
            var users = db.Users.Include(u => u.Gender).Include(u => u.UserRole);
            return View(users.Where(u=>u.RoleID == 4).ToList());
        }

        // GET: EmployeeUsers/Details/5
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

        // GET: EmployeeUsers/Create
        public ActionResult Create()
        {
            ViewBag.GenderID = new SelectList(db.Gender, "GenderID", "GenderType");
            ViewBag.RoleID = new SelectList(db.UserRoles, "RoleID", "RoleName");
            return View();
        }

        // POST: EmployeeUsers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserID,Username,Password,FirstName,LastName,GenderID,BirthDate,Email,MobileNumber,RoleID,UserStatus,isVerified")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            //ViewBag.GenderID = new SelectList(db.Gender, "GenderID", "GenderType", user.GenderID);
            ViewBag.RoleID = new SelectList(db.UserRoles, "RoleID", "RoleName", user.RoleID);
            return View(user);
        }

        // GET: EmployeeUsers/Edit/5
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

        // POST: EmployeeUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserID,Username,Password,FirstName,LastName,GenderID,BirthDate,Email,MobileNumber,RoleID,UserStatus,isVerified")] User user)
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

        // GET: EmployeeUsers/Delete/5
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

        // POST: EmployeeUsers/Delete/5
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
