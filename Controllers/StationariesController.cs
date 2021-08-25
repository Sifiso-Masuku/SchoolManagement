//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.Entity;
//using System.Linq;
//using System.Net;
//using System.Web;
//using System.Web.Mvc;
//using IdentitySample.Models;
//using SchoolManagement.Models.Entity;
//using Microsoft.AspNet.Identity;

//namespace SchoolManagement.Controllers
//{
//    public class StationariesController : Controller
//    {
//        private ApplicationDbContext db = new ApplicationDbContext();

//        // GET: Stationaries
//        public ActionResult Index()
//        {
//            return View(db.stationaries.ToList());
//        }

//        public ActionResult Order(int id)
//        {
//            Stationary stationary = db.stationaries.Where(x => x.ID == id).FirstOrDefault();
//            string userid = User.Identity.GetUserId();
//            ApplicationUser user = db.Users.Where(x => x.Id == userid).FirstOrDefault();

//            ViewBag.Item = stationary.Name;
//            ViewBag.Price = stationary.Price;
//            ViewBag.Email = user.Email;
//            ViewBag.ID = stationary.ID;

//            return View();
            
//        }

//        public ActionResult myOrders()
//        {
//            string userid = User.Identity.GetUserId();
//            ApplicationUser user = db.Users.Where(x => x.Id == userid).FirstOrDefault();
//            List<Order> orders = db.orders.ToList().Where(x => x.Email == user.Email).ToList();

//            return View(orders);
//        }
//        public ActionResult myOrders2()
//        {
//            string userid = User.Identity.GetUserId();
//            ApplicationUser user = db.Users.Where(x => x.Id == userid).FirstOrDefault();
//            List<Order> orders = db.orders.ToList().Where(x => x.Email == user.Email).ToList();

//            return View(orders);
//        }


//        public JsonResult addOrder(string email, int stationaryid, string collectororder)
//        {
//            Order order = new Order();
//            order.CollectOrDeliver = collectororder;
//            order.Email = email;
//            order.StationaryID = stationaryid;
//            order.Status = "Ordered";

//            db.orders.Add(order);
//            db.SaveChanges();
//            return Json(null,JsonRequestBehavior.AllowGet);
//        }

//        // GET: Stationaries/Details/5
//        public ActionResult Details(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            Stationary stationary = db.stationaries.Find(id);
//            if (stationary == null)
//            {
//                return HttpNotFound();
//            }
//            return View(stationary);
//        }

//        // GET: Stationaries/Create
//        public ActionResult Create()
//        {
//            return View();
//        }

//        // POST: Stationaries/Create
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Create([Bind(Include = "ID,Name,Quantity,Price")] Stationary stationary)
//        {
//            if (ModelState.IsValid)
//            {
//                db.stationaries.Add(stationary);
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }

//            return View(stationary);
//        }

//        // GET: Stationaries/Edit/5
//        public ActionResult Edit(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            Stationary stationary = db.stationaries.Find(id);
//            if (stationary == null)
//            {
//                return HttpNotFound();
//            }
//            return View(stationary);
//        }

//        // POST: Stationaries/Edit/5
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Edit([Bind(Include = "ID,Name,Quantity,Price")] Stationary stationary)
//        {
//            if (ModelState.IsValid)
//            {
//                db.Entry(stationary).State = EntityState.Modified;
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }
//            return View(stationary);
//        }

//        // GET: Stationaries/Delete/5
//        public ActionResult Delete(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            Stationary stationary = db.stationaries.Find(id);
//            if (stationary == null)
//            {
//                return HttpNotFound();
//            }
//            return View(stationary);
//        }

//        // POST: Stationaries/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public ActionResult DeleteConfirmed(int id)
//        {
//            Stationary stationary = db.stationaries.Find(id);
//            db.stationaries.Remove(stationary);
//            db.SaveChanges();
//            return RedirectToAction("Index");
//        }

//        protected override void Dispose(bool disposing)
//        {
//            if (disposing)
//            {
//                db.Dispose();
//            }
//            base.Dispose(disposing);
//        }
//    }
//}
