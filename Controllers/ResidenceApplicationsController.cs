using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IdentitySample.Models;
using Microsoft.AspNet.Identity;
using SchoolManagement.Models;
using SchoolManagement;
using SchoolManagement.Services;

namespace SchoolManagement.Controllers
{
    public class ResidenceApplicationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ResidenceApplications
        public ActionResult Index()
        {
            var residenceApplications = db.ResidenceApplications.Include(r => r.Residence);
            var userName = User.Identity.GetUserName();
            if (User.IsInRole("Admin"))
            {
                return View(residenceApplications.ToList());
            }
            else
            {
                return View(residenceApplications.ToList().Where(x=>x.UeserEmail==userName));
            }
            
           
        }

        // GET: ResidenceApplications/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ResidenceApplication residenceApplication = db.ResidenceApplications.Find(id);
            if (residenceApplication == null)
            {
                return HttpNotFound();
            }
            return View(residenceApplication);
        }

        // GET: ResidenceApplications/Create
        public ActionResult OnceOff(int id)
        {

            return RedirectToAction("OnceOff", "PaymentR", new { id = id });
        }
           public ActionResult Create(int? id)
        {
          var res=  db.Residences.Find(id);

            ViewBag.ResidenceName = res.Name;
            ViewBag.ResidenceType = res.ResidenceType.ResidenceTypeName;
            Session["ResidenceId"] = id;
            ViewBag.ResidenceId = new SelectList(db.Residences, "ID", "Name");
            return View();
        }

        // POST: ResidenceApplications/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ResidenceApplicationId,ResidenceId,UeserEmail,DateApplied,RegistrationFee,Status")] ResidenceApplication residenceApplication)
        {
            if (ModelState.IsValid)
            {
                residenceApplication.ResidenceId = int.Parse(Session["ResidenceId"].ToString());
                residenceApplication.DateApplied = DateTime.Now.Date;
                residenceApplication.Status = "Applied";
                residenceApplication.RegistrationFee = BILogic.GetRegistrationFee(residenceApplication.ResidenceId);
                residenceApplication.UeserEmail = User.Identity.GetUserName();
                db.ResidenceApplications.Add(residenceApplication);
                db.SaveChanges();
                EmailSender.ResidenceApplication(residenceApplication);
                return RedirectToAction("Index");
            }

            ViewBag.ResidenceId = new SelectList(db.Residences, "ID", "Name", residenceApplication.ResidenceId);
            return View(residenceApplication);
        }


        public ActionResult Approve(int? id)
        {
            BILogic.ApplicationStatus(id, "Approved");
            return RedirectToAction("Index");
        }  
        public ActionResult Decline(int? id)
        {
            BILogic.ApplicationStatus(id, "Approved");
            return RedirectToAction("Index");
        }
        // GET: ResidenceApplications/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ResidenceApplication residenceApplication = db.ResidenceApplications.Find(id);
            if (residenceApplication == null)
            {
                return HttpNotFound();
            }
            ViewBag.ResidenceId = new SelectList(db.Residences, "ID", "Name", residenceApplication.ResidenceId);
            return View(residenceApplication);
        }

        // POST: ResidenceApplications/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ResidenceApplicationId,ResidenceId,UeserEmail,DateApplied,RegistrationFee,Status")] ResidenceApplication residenceApplication)
        {
            if (ModelState.IsValid)
            {
                residenceApplication.RegistrationFee = BILogic.GetRegistrationFee(residenceApplication.ResidenceId);
                db.Entry(residenceApplication).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ResidenceId = new SelectList(db.Residences, "ID", "Name", residenceApplication.ResidenceId);
            return View(residenceApplication);
        }

        // GET: ResidenceApplications/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ResidenceApplication residenceApplication = db.ResidenceApplications.Find(id);
            if (residenceApplication == null)
            {
                return HttpNotFound();
            }
            return View(residenceApplication);
        }

        // POST: ResidenceApplications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ResidenceApplication residenceApplication = db.ResidenceApplications.Find(id);
            db.ResidenceApplications.Remove(residenceApplication);
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
