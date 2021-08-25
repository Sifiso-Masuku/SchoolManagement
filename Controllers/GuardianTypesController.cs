using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SchoolManagement.Model.Entity;
using IdentitySample.Models;
namespace SchoolManagement.Controllers
{
    public class GuardianTypesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: GuardianTypes
        public ActionResult Index()
        {
            return View(db.GuardianTypes.ToList());
        }

        // GET: GuardianTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GuardianType guardianType = db.GuardianTypes.Find(id);
            if (guardianType == null)
            {
                return HttpNotFound();
            }
            return View(guardianType);
        }

        // GET: GuardianTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: GuardianTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] GuardianType guardianType)
        {
            if (ModelState.IsValid)
            {
                db.GuardianTypes.Add(guardianType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(guardianType);
        }

        // GET: GuardianTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GuardianType guardianType = db.GuardianTypes.Find(id);
            if (guardianType == null)
            {
                return HttpNotFound();
            }
            return View(guardianType);
        }

        // POST: GuardianTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] GuardianType guardianType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(guardianType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(guardianType);
        }

        // GET: GuardianTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GuardianType guardianType = db.GuardianTypes.Find(id);
            if (guardianType == null)
            {
                return HttpNotFound();
            }
            return View(guardianType);
        }

        // POST: GuardianTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            GuardianType guardianType = db.GuardianTypes.Find(id);
            db.GuardianTypes.Remove(guardianType);
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
