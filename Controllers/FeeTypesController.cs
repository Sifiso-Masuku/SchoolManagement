
using System.Data.Entity;
using System.Linq;
using System.Net;
using IdentitySample.Models;
using System.Web.Mvc;
using SchoolManagement.Model.Entity;

namespace SchoolManagement.Controllers
{
    public class FeeTypesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: FeeTypes
        public ActionResult Index()
        {
            return View(db.FeeTypes.ToList());
        }

        // GET: FeeTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FeeType feeType = db.FeeTypes.Find(id);
            if (feeType == null)
            {
                return HttpNotFound();
            }
            return View(feeType);
        }

        // GET: FeeTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FeeTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,FeeAmount")] FeeType feeType)
        {
            if (ModelState.IsValid)
            {
                db.FeeTypes.Add(feeType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(feeType);
        }

        // GET: FeeTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FeeType feeType = db.FeeTypes.Find(id);
            if (feeType == null)
            {
                return HttpNotFound();
            }
            return View(feeType);
        }

        // POST: FeeTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,FeeAmount")] FeeType feeType)
        {
           
            if (ModelState.IsValid)
            {
                db.Entry(feeType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(feeType);
        }

        // GET: FeeTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FeeType feeType = db.FeeTypes.Find(id);
            if (feeType == null)
            {
                return HttpNotFound();
            }
            return View(feeType);
        }

        // POST: FeeTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FeeType feeType = db.FeeTypes.Find(id);
            db.FeeTypes.Remove(feeType);
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
