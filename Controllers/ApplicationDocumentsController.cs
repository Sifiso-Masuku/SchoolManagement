using System;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SchoolManagement.Model.Entity;
using IdentitySample.Models;

namespace SchoolManagement.Controllers
{
    public class ApplicationDocumentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ApplicationDocuments
        public ActionResult Index()
        {
            var applicationDocuments = db.ApplicationDocuments.Include(a => a.StudentApplication);
            return View(applicationDocuments.ToList());
        }

        // GET: ApplicationDocuments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationDocuments applicationDocuments = db.ApplicationDocuments.Find(id);
            if (applicationDocuments == null)
            {
                return HttpNotFound();
            }
            return View(applicationDocuments);
        }

        // GET: ApplicationDocuments/Create
        public ActionResult Create(int id)
        {
            //ViewBag.StudentApplicationId = new SelectList(db.Studentapplications, "Id", "Name");
           var Appdocs = new ApplicationDocuments();
            Appdocs.ApplicationDate = DateTime.Now;
            Appdocs.StudentApplicationId = id;
            return View(Appdocs);
        }

        // POST: ApplicationDocuments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Exclude = "PreviousSchoolDocument1,Certificate1,CertifiedID1,HomeAddress1")]ApplicationDocuments applicationDocuments)
        {
            if (ModelState.IsValid)
            {
                byte[] PreviousSchoolDocument = null;
                byte[] Certificate = null;
                byte[] CertifiedID = null;
                byte[] HomeAddress = null;
                if (Request.Files.Count > 0)
                {
                    HttpPostedFileBase poImgFile = Request.Files["PreviousSchoolDocument1"];
                    HttpPostedFileBase poImgFile1 = Request.Files["Certificate1"];
                    HttpPostedFileBase poImgFile2 = Request.Files["CertifiedID1"];
                    HttpPostedFileBase poImgFile3 = Request.Files["HomeAddress1"];

                    using (var binary = new BinaryReader(poImgFile.InputStream))
                    {
                        PreviousSchoolDocument = binary.ReadBytes(poImgFile.ContentLength);
                    }

                    using (var binary = new BinaryReader(poImgFile1.InputStream))
                    {
                        Certificate = binary.ReadBytes(poImgFile1.ContentLength);
                    }

                    using (var binary = new BinaryReader(poImgFile2.InputStream))
                    {
                        CertifiedID = binary.ReadBytes(poImgFile2.ContentLength);
                    }

                    using (var binary = new BinaryReader(poImgFile3.InputStream))
                    {
                        HomeAddress = binary.ReadBytes(poImgFile3.ContentLength);
                    }
                }
                applicationDocuments.PreviousSchoolDocument = PreviousSchoolDocument;
                applicationDocuments.Certificate = Certificate;
                applicationDocuments.CertifiedID = CertifiedID;
                applicationDocuments.HomeAddress = HomeAddress;

                db.ApplicationDocuments.Add(applicationDocuments);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(applicationDocuments);
        }

        public FileResult OpenPDFIdDoc(int id)
        {
            var fl = db.ApplicationDocuments.Where(l => l.Id == id).Select(o => o.PreviousSchoolDocument).FirstOrDefault();
            byte[] pdfByte = fl;
            return File(pdfByte, "application/pdf");
        }
        public FileResult OpenPDFIdDoc1(int id)
        {
            var fl = db.ApplicationDocuments.Where(l => l.Id == id).Select(o => o.Certificate).FirstOrDefault();
            byte[] pdfByte = fl;
            return File(pdfByte, "application/pdf");
        }
        public FileResult OpenPDFIdDoc2(int id)
        {
            var fl = db.ApplicationDocuments.Where(l => l.Id == id).Select(o => o.CertifiedID).FirstOrDefault();
            byte[] pdfByte = fl;
            return File(pdfByte, "application/pdf");
        }
        public FileResult OpenPDFIdDoc3(int id)
        {
            var fl = db.ApplicationDocuments.Where(l => l.Id == id).Select(o => o.HomeAddress).FirstOrDefault();
            byte[] pdfByte = fl;
            return File(pdfByte, "application/pdf");
        }
        // GET: ApplicationDocuments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationDocuments applicationDocuments = db.ApplicationDocuments.Find(id);
            if (applicationDocuments == null)
            {
                return HttpNotFound();
            }
            ViewBag.StudentApplicationId = new SelectList(db.Studentapplications, "Id", "Name", applicationDocuments.StudentApplicationId);
            return View(applicationDocuments);
        }

        // POST: ApplicationDocuments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ApplicationDate,StudentApplicationId,PreviousSchool,PreviousSchoolAddrs,PreviousSchoolDocument,Certificate,CertifiedID,HomeAddress")] ApplicationDocuments applicationDocuments)
        {
            if (ModelState.IsValid)
            {
                db.Entry(applicationDocuments).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.StudentApplicationId = new SelectList(db.Studentapplications, "Id", "Name", applicationDocuments.StudentApplicationId);
            return View(applicationDocuments);
        }

        // GET: ApplicationDocuments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationDocuments applicationDocuments = db.ApplicationDocuments.Find(id);
            if (applicationDocuments == null)
            {
                return HttpNotFound();
            }
            return View(applicationDocuments);
        }

        // POST: ApplicationDocuments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ApplicationDocuments applicationDocuments = db.ApplicationDocuments.Find(id);
            db.ApplicationDocuments.Remove(applicationDocuments);
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
