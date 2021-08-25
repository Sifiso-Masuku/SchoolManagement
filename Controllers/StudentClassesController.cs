
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using IdentitySample.Models;
using SchoolManagement.Model.Entity;
using SchoolManagement.Model.ViewModels;

namespace SchoolManagement.Controllers
{
    public class StudentClassesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private StudentClassVM studentClassVM = new StudentClassVM();

        // GET: StudentClasses
        public ActionResult Index()
        {
            var studentClasses = db.StudentClasses.Include(s => s.ClassName).Include(s => s.Section);
            return View(studentClasses.ToList());
        }

        // GET: StudentClasses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentClass studentClass = db.StudentClasses.Find(id);
            if (studentClass == null)
            {
                return HttpNotFound();
            }
            return View(studentClass);
        }

        // GET: StudentClasses/Create
        public ActionResult Create()
        {
            ViewBag.ClassNameId = new SelectList(db.ClassNames, "Id", "Name");
            ViewBag.SectionId = new SelectList(db.Sections, "Id", "Name");
            return View();
        }

        // POST: StudentClasses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ClassNameId,SectionId,Name")] StudentClass studentClass)
        {
            if (ModelState.IsValid)
            {
                studentClass.Name = studentClassVM.GetName();
                var std = new StudentClass() { Id=studentClass.Id,ClassNameId=studentClass.ClassNameId,SectionId=studentClass.SectionId,Name=studentClass.Name}; 
                db.StudentClasses.Add(std);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ClassNameId = new SelectList(db.ClassNames, "Id", "Name", studentClass.ClassNameId);
            ViewBag.SectionId = new SelectList(db.Sections, "Id", "Name", studentClass.SectionId);
            return View(studentClass);
        }

        // GET: StudentClasses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentClass studentClass = db.StudentClasses.Find(id);
            if (studentClass == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClassNameId = new SelectList(db.ClassNames, "Id", "Name", studentClass.ClassNameId);
            ViewBag.SectionId = new SelectList(db.Sections, "Id", "Name", studentClass.SectionId);
            return View(studentClass);
        }

        // POST: StudentClasses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ClassNameId,SectionId")] StudentClass studentClass)
        {
            if (ModelState.IsValid)
            {
                db.Entry(studentClass).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ClassNameId = new SelectList(db.ClassNames, "Id", "Name", studentClass.ClassNameId);
            ViewBag.SectionId = new SelectList(db.Sections, "Id", "Name", studentClass.SectionId);
            return View(studentClass);
        }

        // GET: StudentClasses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentClass studentClass = db.StudentClasses.Find(id);
            if (studentClass == null)
            {
                return HttpNotFound();
            }
            return View(studentClass);
        }

        // POST: StudentClasses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            StudentClass studentClass = db.StudentClasses.Find(id);
            db.StudentClasses.Remove(studentClass);
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
