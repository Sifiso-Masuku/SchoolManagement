using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using IdentitySample.Models;
using SchoolManagement.Model.Entity;
using SchoolManagement.Model.ViewModels;

namespace SchoolManagement.Controllers
{
    public class AssignTeacheToClassesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private AssignTeacheToClassVM attC = new AssignTeacheToClassVM();

        // GET: AssignTeacheToClasses
        public ActionResult Index()
        {
            var assignTeacheToClasses = db.AssignTeacheToClasses.Include(a => a.ClassName).Include(a => a.Teacher);
            return View(assignTeacheToClasses.ToList());
        }

        // GET: AssignTeacheToClasses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AssignTeacheToClass assignTeacheToClass = db.AssignTeacheToClasses.Find(id);
            if (assignTeacheToClass == null)
            {
                return HttpNotFound();
            }
            return View(assignTeacheToClass);
        }

        // GET: AssignTeacheToClasses/Create
        public ActionResult Create()
        {
            ViewBag.ClassNameId = new SelectList(db.ClassNames, "Id", "Name");
            ViewBag.TeacherId = new SelectList(db.Teachers, "Id", "Name");
            return View();
        }

        // POST: AssignTeacheToClasses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TeacherClassId,TeacherId,ClassNameId")] AssignTeacheToClass assignTeacheToClass)
        {
            if (ModelState.IsValid)
            {
                if (attC.CheckExists())
                {
                    ModelState.AddModelError("", "You can not assing a teacher twice to the same class");
                }
				else
				{
                    db.AssignTeacheToClasses.Add(assignTeacheToClass);
                    db.SaveChanges();
                }
   
                return RedirectToAction("Index");
            }

            ViewBag.ClassNameId = new SelectList(db.ClassNames, "Id", "Name", assignTeacheToClass.ClassNameId);
            ViewBag.TeacherId = new SelectList(db.Teachers, "Id", "Name", assignTeacheToClass.TeacherId);
            return View(assignTeacheToClass);
        }

        // GET: AssignTeacheToClasses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AssignTeacheToClass assignTeacheToClass = db.AssignTeacheToClasses.Find(id);
            if (assignTeacheToClass == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClassNameId = new SelectList(db.ClassNames, "Id", "Name", assignTeacheToClass.ClassNameId);
            ViewBag.TeacherId = new SelectList(db.Teachers, "Id", "Name", assignTeacheToClass.TeacherId);
            return View(assignTeacheToClass);
        }

        // POST: AssignTeacheToClasses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TeacherClassId,TeacherId,ClassNameId")] AssignTeacheToClass assignTeacheToClass)
        {
            if (ModelState.IsValid)
            {
                db.Entry(assignTeacheToClass).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ClassNameId = new SelectList(db.ClassNames, "Id", "Name", assignTeacheToClass.ClassNameId);
            ViewBag.TeacherId = new SelectList(db.Teachers, "Id", "Name", assignTeacheToClass.TeacherId);
            return View(assignTeacheToClass);
        }

        // GET: AssignTeacheToClasses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AssignTeacheToClass assignTeacheToClass = db.AssignTeacheToClasses.Find(id);
            if (assignTeacheToClass == null)
            {
                return HttpNotFound();
            }
            return View(assignTeacheToClass);
        }

        // POST: AssignTeacheToClasses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AssignTeacheToClass assignTeacheToClass = db.AssignTeacheToClasses.Find(id);
            db.AssignTeacheToClasses.Remove(assignTeacheToClass);
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
