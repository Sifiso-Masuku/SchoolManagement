using IdentitySample.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using SchoolManagement.Model.Entity;

namespace SchoolManagement.Controllers
{
    public class ClassNamesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ClassNames
        public ActionResult Index()
        {
            return View(db.ClassNames.ToList());
        }

        // GET: ClassNames/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClassName className = db.ClassNames.Find(id);
            if (className == null)
            {
                return HttpNotFound();
            }
            return View(className);
        }

        // GET: ClassNames/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ClassNames/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] ClassName className)
        {
            if (ModelState.IsValid)
            {
                db.ClassNames.Add(className);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(className);
        }

        // GET: ClassNames/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClassName className = db.ClassNames.Find(id);
            if (className == null)
            {
                return HttpNotFound();
            }
            return View(className);
        }

        // POST: ClassNames/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] ClassName className)
        {
            if (ModelState.IsValid)
            {
                db.Entry(className).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(className);
        }

        // GET: ClassNames/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClassName className = db.ClassNames.Find(id);
            if (className == null)
            {
                return HttpNotFound();
            }
            return View(className);
        }

        // POST: ClassNames/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ClassName className = db.ClassNames.Find(id);
            db.ClassNames.Remove(className);
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
