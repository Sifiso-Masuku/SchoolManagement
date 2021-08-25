
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using PagedList;
using IdentitySample.Models;
using SchoolManagement.Model.Entity;

namespace SchoolManagement.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class SubjectController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        public ActionResult SubjectView(int? id)
        {
            if (id == null)
            {
                var results = db.Subjects.ToList();
                return View(results);

            }
            else
            {
                var results = db.Subjects.Where(x => x.ClassNameId == id).ToList();
                return View(results);

            }
        }
        public ActionResult SubjectView2(int? id)
        {
            if (id == null)
            {
                var results = db.Subjects.ToList();
                return View(results);

            }
            else
            {
                var results = db.Subjects.Where(x => x.ClassNameId == id).ToList();
                return View(results);

            }
        }
        public ActionResult Index(string option, string search, int? pageNumber, string sort)
        {
            var Subject = db.Subjects.Include(c => c.ClassName);
           
               
            if (option == "ClassName")
            {
                //Index action method will return a view with a student records based on what a user specify the value in textbox  
                return View(Subject.Where(x => x.ClassName.Name.StartsWith(search) || search == null).ToList().ToPagedList(pageNumber ?? 1, 8));
            }
            else if (option == "Code")
            {
                return View(Subject.Where(x => x.Code.StartsWith(search) || search == null).ToList().ToPagedList(pageNumber ?? 1, 5));
            }
            else
            {
                    return View(Subject.Where(x => x.Name.StartsWith(search) || search == null).ToList().ToPagedList(pageNumber ?? 1, 3));
               
                }
            }
        public ActionResult ExportToExcel()
        {
            var gv = new GridView();
            gv.DataSource = db.Studentapplications.ToList();
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=DemoExcel.xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter objStringWriter = new StringWriter();
            HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);
            gv.RenderControl(objHtmlTextWriter);
            Response.Output.Write(objStringWriter.ToString());
            Response.Flush();
            Response.End();
            return View("Index");
        }


        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subject Subject = db.Subjects.Find(id);
            if (Subject == null)
            {
                return HttpNotFound();
            }
            return View(Subject);
        }

        public ActionResult Create()
        {
            ViewBag.ClassNameId = new SelectList(db.ClassNames, "Id","Name");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Code,Theory,Mcq,Practical,Subject_Credit,ClassNameId")] Subject Subject)
        {

           
            if (ModelState.IsValid)
            {
                
                db.Subjects.Add(Subject);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ClassNameId = new SelectList(db.ClassNames, "Id", "Name", Subject.ClassNameId); ;
            return View(Subject);
        }

        // GET: Courses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subject Subject = db.Subjects.Find(id);
            if (Subject == null)
            {
                return HttpNotFound();
            }
            return View(Subject);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( Subject Subject)
        {
            if (ModelState.IsValid)
            {
                db.Entry(Subject).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(Subject);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subject Subject = db.Subjects.Find(id);
            if (Subject == null)
            {
                return HttpNotFound();
            }
            return View(Subject);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Subject Subject = db.Subjects.Find(id);
            db.Subjects.Remove(Subject);
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
