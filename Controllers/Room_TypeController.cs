using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IdentitySample.Models;
using SchoolManagement.Models;

namespace SchoolManagement.Controllers
{
    public class Room_TypeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Room_Type
        public ActionResult Index()
        {
            return View(db.Room_Type.ToList());
        }

        // GET: Room_Type/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room_Type room_Type = db.Room_Type.Find(id);
            if (room_Type == null)
            {
                return HttpNotFound();
            }
            return View(room_Type);
        }

        // GET: Room_Type/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Room_Type/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Room_TypeId,RtName,No_Occupants")] Room_Type room_Type)
        {
            if (ModelState.IsValid)
            {
                db.Room_Type.Add(room_Type);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(room_Type);
        }

        // GET: Room_Type/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room_Type room_Type = db.Room_Type.Find(id);
            if (room_Type == null)
            {
                return HttpNotFound();
            }
            return View(room_Type);
        }

        // POST: Room_Type/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Room_TypeId,RtName,No_Occupants")] Room_Type room_Type)
        {
            if (ModelState.IsValid)
            {
                db.Entry(room_Type).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(room_Type);
        }

        // GET: Room_Type/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room_Type room_Type = db.Room_Type.Find(id);
            if (room_Type == null)
            {
                return HttpNotFound();
            }
            return View(room_Type);
        }

        // POST: Room_Type/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Room_Type room_Type = db.Room_Type.Find(id);
            db.Room_Type.Remove(room_Type);
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
