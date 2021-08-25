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
    public class RoomsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Rooms
        public ActionResult Index()
        {
            var rooms = db.Rooms.Include(r => r.Residence).Include(r => r.Room_Type);
            return View(rooms.ToList());
        }

        // GET: Rooms/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room room = db.Rooms.Find(id);
            if (room == null)
            {
                return HttpNotFound();
            }
            return View(room);
        }

        // GET: Rooms/Create
        public ActionResult Create()
        {
            ViewBag.ResidenceId = new SelectList(db.Residences, "ID", "Name");
            ViewBag.Room_TypeId = new SelectList(db.Room_Type, "Room_TypeId", "RtName");
            return View();
        }

        // POST: Rooms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RoomId,Room_TypeId,ResidenceId,RoomNumber,Status")] Room room)
        {
            if (ModelState.IsValid)
            {
                if (BILogic.ChechRoomNumber(room.RoomNumber))
                {
                    room.Status = "Available";
                    db.Rooms.Add(room);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Duplicate Room Number");
                    ViewBag.ResidenceId = new SelectList(db.Residences, "ID", "Name", room.ResidenceId);
                    ViewBag.Room_TypeId = new SelectList(db.Room_Type, "Room_TypeId", "RtName", room.Room_TypeId);
                    return View(room);
                }
            }
            ViewBag.ResidenceId = new SelectList(db.Residences, "ID", "Name", room.ResidenceId);
            ViewBag.Room_TypeId = new SelectList(db.Room_Type, "Room_TypeId", "RtName", room.Room_TypeId);
            return View(room);
        }

        // GET: Rooms/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room room = db.Rooms.Find(id);
            if (room == null)
            {
                return HttpNotFound();
            }
            ViewBag.ResidenceId = new SelectList(db.Residences, "ID", "Name", room.ResidenceId);
            ViewBag.Room_TypeId = new SelectList(db.Room_Type, "Room_TypeId", "RtName", room.Room_TypeId);
            return View(room);
        }

        // POST: Rooms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RoomId,Room_TypeId,ResidenceId,RoomNumber,Status")] Room room)
        {
            if (ModelState.IsValid)
            {
                db.Entry(room).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ResidenceId = new SelectList(db.Residences, "ID", "Name", room.ResidenceId);
            ViewBag.Room_TypeId = new SelectList(db.Room_Type, "Room_TypeId", "RtName", room.Room_TypeId);
            return View(room);
        }

        // GET: Rooms/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room room = db.Rooms.Find(id);
            if (room == null)
            {
                return HttpNotFound();
            }
            return View(room);
        }

        // POST: Rooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Room room = db.Rooms.Find(id);
            db.Rooms.Remove(room);
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
