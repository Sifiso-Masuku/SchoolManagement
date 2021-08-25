using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SchoolManagement.Model;
using SchoolManagement.Model.Entity;
using IdentitySample.Models; 
namespace SchoolManagement.Controllers
{
    public class ClassFeesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ClassFees
        public ActionResult Index()
        {
            var classFees = db.ClassFees.Include(c => c.ClassName).Include(c => c.FeeType);
            return View(classFees.ToList());
        }

        // GET: ClassFees/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClassFee classFee = db.ClassFees.Find(id);
            if (classFee == null)
            {
                return HttpNotFound();
            }
            return View(classFee);
        }

        // GET: ClassFees/Create
        public ActionResult Create()
        {
            ViewBag.ClassNameId = new SelectList(db.ClassNames, "Id", "Name");
            ViewBag.FeeTypeId = new SelectList(db.FeeTypes, "Id", "Name");
            return View();
        }

        // POST: ClassFees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FeeTypeId,ClassNameId")] ClassFee classFee)
        {
            if (ModelState.IsValid)
            {
                db.ClassFees.Add(classFee);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ClassNameId = new SelectList(db.ClassNames, "Id", "Name", classFee.ClassNameId);
            ViewBag.FeeTypeId = new SelectList(db.FeeTypes, "Id", "Name", classFee.FeeTypeId);
            return View(classFee);
        }

        // GET: ClassFees/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClassFee classFee = db.ClassFees.Find(id);
            if (classFee == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClassNameId = new SelectList(db.ClassNames, "Id", "Name", classFee.ClassNameId);
            ViewBag.FeeTypeId = new SelectList(db.FeeTypes, "Id", "Name", classFee.FeeTypeId);
            return View(classFee);
        }

        // POST: ClassFees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FeeTypeId,ClassNameId")] ClassFee classFee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(classFee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ClassNameId = new SelectList(db.ClassNames, "Id", "Name", classFee.ClassNameId);
            ViewBag.FeeTypeId = new SelectList(db.FeeTypes, "Id", "Name", classFee.FeeTypeId);
            return View(classFee);
        }

        // GET: ClassFees/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClassFee classFee = db.ClassFees.Find(id);
            if (classFee == null)
            {
                return HttpNotFound();
            }
            return View(classFee);
        }

        // POST: ClassFees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ClassFee classFee = db.ClassFees.Find(id);
            db.ClassFees.Remove(classFee);
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
