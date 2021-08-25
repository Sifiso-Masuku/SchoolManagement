
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using IdentitySample.Models;

using SchoolManagement.Model.Entity;

namespace SchoolManagement.Controllers
{
    public class WorkSchedulesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: WorkSchedules
        public ActionResult Index()
        {
            var WorkSchedule = db.WorkSchedule.Include(a => a.ClassName).Include(a => a.teachers);
            return View(WorkSchedule.ToList());
        }

        // GET: WorkSchedules/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WorkSchedule workSchedule = db.WorkSchedule.Find(id);
            if (workSchedule == null)
            {
                return HttpNotFound();
            }
            return View(workSchedule);
        }

        // GET: WorkSchedules/Create
        public ActionResult Create()
        {
            ViewBag.classRoomId = new SelectList(db.ClassNames, "Id", "Name");
            ViewBag.staffMemberId = new SelectList(db.Teachers, "Id", "Name");
            return View();
        }

        // POST: WorkSchedules/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "scheduleId,scheduleStartDate,scheduleEndDate,ThemeColor,archived,classRoomId,staffMemberId")] WorkSchedule workSchedule)
        {
            if (ModelState.IsValid)
            {
                db.WorkSchedule.Add(workSchedule);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.classRoomId = new SelectList(db.ClassNames, "Id", "Name", workSchedule.classRoomId);
            ViewBag.staffMemberId = new SelectList(db.Teachers, "Id", "Name", workSchedule.staffMemberId);
            return View(workSchedule);
        }

        // GET: WorkSchedules/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WorkSchedule workSchedule = db.WorkSchedule.Find(id);
            if (workSchedule == null)
            {
                return HttpNotFound();
            }
            return View(workSchedule);
        }

        // POST: WorkSchedules/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "scheduleId,scheduleStartDate,scheduleEndDate,ThemeColor,archived,classRoomId,staffMemberId")] WorkSchedule workSchedule)
        {
            if (ModelState.IsValid)
            {
                db.Entry(workSchedule).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(workSchedule);
        }

        // GET: WorkSchedules/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WorkSchedule workSchedule = db.WorkSchedule.Find(id);
            if (workSchedule == null)
            {
                return HttpNotFound();
            }
            return View(workSchedule);
        }

        // POST: WorkSchedules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            WorkSchedule workSchedule = db.WorkSchedule.Find(id);
            db.WorkSchedule.Remove(workSchedule);
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
