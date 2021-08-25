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
    public class ResidenceTypesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ResidenceTypes
        public ActionResult Index()
        {
            return View(db.ResidenceTypes.ToList());
        }
        public ActionResult ResidenceTyoeView()
        {
            return View(db.ResidenceTypes.ToList());
        }

        // GET: ResidenceTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ResidenceType residenceType = db.ResidenceTypes.Find(id);
            if (residenceType == null)
            {
                return HttpNotFound();
            }
            return View(residenceType);
        }

        // GET: ResidenceTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ResidenceTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ResidenceTypeId,ResidenceTypeName")] ResidenceType residenceType)
        {
            if (ModelState.IsValid)
            {
                db.ResidenceTypes.Add(residenceType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(residenceType);
        }

        // GET: ResidenceTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ResidenceType residenceType = db.ResidenceTypes.Find(id);
            if (residenceType == null)
            {
                return HttpNotFound();
            }
            return View(residenceType);
        }

        // POST: ResidenceTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ResidenceTypeId,ResidenceTypeName")] ResidenceType residenceType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(residenceType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(residenceType);
        }

        // GET: ResidenceTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ResidenceType residenceType = db.ResidenceTypes.Find(id);
            if (residenceType == null)
            {
                return HttpNotFound();
            }
            return View(residenceType);
        }

        // POST: ResidenceTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ResidenceType residenceType = db.ResidenceTypes.Find(id);
            db.ResidenceTypes.Remove(residenceType);
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
