using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using PayFast;
using PayFast.AspNet;
using SchoolManagement.Model.Entity;
using SchoolManagement.Models;
using IdentitySample.Models;
using SchoolManagement.Models.Entity;

namespace SchoolManagement.Controllers
{
    public class StudentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Students
        public ActionResult Index()
        {
            var username = User.Identity.GetUserName();

            List<Student> students = db.Students.ToList();
            //return View(db.Students.ToList().Where(x => x.UserName == username));
            return View(db.Students.ToList().Where(x => x.Status == "Registered" || x.Status.ToLower() == "paid full amount"));
        }

        public ActionResult ViewReports(int id)
        {
            Student student = db.Students.Where(x => x.Id == id).FirstOrDefault();
            List<Term> terms = db.terms.ToList();
            ViewBag.StudentID = student.Id;
            ViewBag.StudentName = student.Name;

            return View(terms);

        }

        public ActionResult NoReport()
        {
            return View();
        }

        public ActionResult OpenReport(int studentid,string term)
        {
            Student student = db.Students.Where(x => x.Id == studentid).FirstOrDefault();
            Term trm = db.terms.Where(x => x.Name == term).FirstOrDefault();
            List<Report> reports = db.reports.Where(x => x.StudentID == studentid && x.Term == term).ToList();

            decimal avg = 0;

            reports.ForEach(rep => {
                avg += rep.Mark;
            });

            if(reports.Count == 0)
            {
                return RedirectToAction("NoReport");
            }

            avg = avg / reports.Count;

            if(avg > 49)
            {
                ViewBag.Passed = true;
            }
            else
            {
                ViewBag.Passed = false;
            }

            ViewBag.Term = term;
            ViewBag.Average = avg.ToString().Substring(0,5);
            ViewBag.Student = student.Name;
            ViewBag.Grade = student.Grade;
            return View(reports);
        }

        public ActionResult CreateReport(int studentid)
        {
            Student student = db.Students.Where(x => x.Id == studentid).FirstOrDefault();
            ViewBag.StudentID = student.Id;
            ViewBag.StudentName = student.Name;

            ClassName grade = db.ClassNames.Where(x => x.Name == student.Grade).FirstOrDefault();
            List<Subject> subjects = db.Subjects.Where(x=>x.ClassNameId == grade.Id).ToList();
            return View(subjects);
        }

        public ActionResult CreateReportPage(string subject, int studentid)
        {
            Student student = db.Students.Where(x => x.Id == studentid).FirstOrDefault();
            ViewBag.StudentName = student.Name;
            ViewBag.StudentID = student.Id;
            ViewBag.Subject = subject;
            ViewBag.Grade = student.Grade;
            return View();
        }

        [HttpGet]
        public JsonResult getTerms()
        {
            return Json(db.terms.ToList(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult getSubjects(int id)
        {
            
            Student student = db.Students.Where(x => x.Id == id).FirstOrDefault();
            ClassName grade = db.ClassNames.Where(x => x.Name == student.Grade).FirstOrDefault();
            db.Configuration.ProxyCreationEnabled = false;
            List<Subject> subjects = db.Subjects.ToList();

            return Json(subjects, JsonRequestBehavior.AllowGet);
        }

        public int buildReport(int studentid, string term, decimal mark, string subject)
        {
            Report report = new Report();

            report.StudentID = studentid;
            report.Term = term;
            report.Mark = mark;
            report.Subject = subject;

            try
            {
                db.reports.Add(report);
                db.SaveChanges();

                return 1;

            }catch(Exception e)
            {
                return -1;
            }

            
        }
        


        // GET: Students/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // GET: Students/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Surname,DateOfBirth,Gender,Email,PresentAddress,deposit,Tuition,BalanceDue,UserName,NID,subject,StudentNumber,GuardianName,Status")] Student student)
        {
            if (ModelState.IsValid)
            {
                db.Students.Add(student);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(student);
        }

        // GET: Students/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Surname,DateOfBirth,Gender,Email,PresentAddress,deposit,Tuition,BalanceDue,UserName,NID,subject,StudentNumber,GuardianName,Status")] Student student)
        {
            if (ModelState.IsValid)
            {
                db.Entry(student).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(student);
        }

        // GET: Students/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Student student = db.Students.Find(id);
            db.Students.Remove(student);
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

        public StudentsController()
        {
            this.payFastSettings = new PayFastSettings();
            this.payFastSettings.MerchantId = ConfigurationManager.AppSettings["MerchantId"];
            this.payFastSettings.MerchantKey = ConfigurationManager.AppSettings["MerchantKey"];
            this.payFastSettings.PassPhrase = ConfigurationManager.AppSettings["PassPhrase"];
            this.payFastSettings.ProcessUrl = ConfigurationManager.AppSettings["ProcessUrl"];
            this.payFastSettings.ValidateUrl = ConfigurationManager.AppSettings["ValidateUrl"];
            this.payFastSettings.ReturnUrl = ConfigurationManager.AppSettings["ReturnUrl"];
            this.payFastSettings.CancelUrl = ConfigurationManager.AppSettings["CancelUrl"];
            this.payFastSettings.NotifyUrl = ConfigurationManager.AppSettings["NotifyUrl"];
        }
        //Payment
        #region Fields

        private readonly PayFastSettings payFastSettings;

        #endregion Fields

        #region Constructor

        //public ApprovedOwnersController()
        //{

        //}

        #endregion Constructor

        #region Methods



        public ActionResult Recurring()
        {
            var recurringRequest = new PayFastRequest(this.payFastSettings.PassPhrase);

            // Merchant Details
            recurringRequest.merchant_id = this.payFastSettings.MerchantId;
            recurringRequest.merchant_key = this.payFastSettings.MerchantKey;
            recurringRequest.return_url = this.payFastSettings.ReturnUrl;
            recurringRequest.cancel_url = this.payFastSettings.CancelUrl;
            recurringRequest.notify_url = this.payFastSettings.NotifyUrl;

            // Buyer Details
            recurringRequest.email_address = "sbtu01@payfast.co.za";

            // Transaction Details
            recurringRequest.m_payment_id = "8d00bf49-e979-4004-228c-08d452b86380";
            recurringRequest.amount = 20;
            recurringRequest.item_name = "Recurring Option";
            recurringRequest.item_description = "Some details about the recurring option";

            // Transaction Options
            recurringRequest.email_confirmation = true;
            recurringRequest.confirmation_address = "drnendwandwe@gmail.com";

            // Recurring Billing Details
            recurringRequest.subscription_type = SubscriptionType.Subscription;
            recurringRequest.billing_date = DateTime.Now;
            recurringRequest.recurring_amount = 20;
            recurringRequest.frequency = BillingFrequency.Monthly;
            recurringRequest.cycles = 0;

            var redirectUrl = $"{this.payFastSettings.ProcessUrl}{recurringRequest.ToString()}";

            return Redirect(redirectUrl);
        }
        //public ActionResult pay(int? id)
        //{
        //    StudentApplication studentApplication = db.Studentapplications.Find(id);
        //    var priceId = db.ClassFees.Where(p => p.ClassNameId == studentApplication.ClassNameId).Select(p => p.FeeTypeId).FirstOrDefault();
        //    var price = db.FeeTypes.Where(p => p.Id == priceId).Select(p => p.FeeAmount).FirstOrDefault();
        //    studentApplication.Status = "Paid";
        //    db.Entry(studentApplication).State = EntityState.Modified;

        //    // db.Studentapplications.Add(studentApplication);
        //    db.SaveChanges();
        //    return RedirectToAction("Index2");

        //}
        public ActionResult OnceOff(int? id)
        {
            var onceOffRequest = new PayFastRequest(this.payFastSettings.PassPhrase);

            // Merchant Details
            onceOffRequest.merchant_id = this.payFastSettings.MerchantId;
            onceOffRequest.merchant_key = this.payFastSettings.MerchantKey;
            onceOffRequest.return_url = this.payFastSettings.ReturnUrl;
            onceOffRequest.cancel_url = this.payFastSettings.CancelUrl;
            onceOffRequest.notify_url = this.payFastSettings.NotifyUrl;

            // Buyer Details

            onceOffRequest.email_address = "sbtu01@payfast.co.za";
            //onceOffRequest.email_address = "sbtu01@payfast.co.za";
            // Transaction Details

            StudentApplication studentApplication = db.Studentapplications.Find(id);
            //var price = db.FeeTypes.Where(p => p.Id == priceId.FeeTypeId && p.Name == "Deposit").FirstOrDefault();
            studentApplication.Status = "Tuition Paid";

            db.Entry(studentApplication).State = EntityState.Modified;
            db.SaveChanges();
            Student student = db.Students.Find(id);
            //student.deposit = SchoolLogic.getPrice("Deposit", (priceId.FeeTypeId + 1));
            //student.deposit = price.FeeAmount;
            

            var mailTo = new List<MailAddress>();
            mailTo.Add(new MailAddress(studentApplication.GuardianEmail, studentApplication.GuardianName));
            var body = $"Hello {studentApplication.GuardianName}, Amount Paid {student.Tuition}, Amount Remaining {student.BalanceDue}, Student Number {studentApplication.StudentNumber}, First Name {studentApplication.Name}, Student Surname {studentApplication.Surname}, Date:{System.DateTime.Now.Date}<br/> Regards,<br/><br/> Bright Ideas <br/> .";

            SchoolManagement.Models.EmailService emailService = new SchoolManagement.Models.EmailService();
            emailService.SendEmail(new EmailContent()
            {
                mailTo = mailTo,
                mailCc = new List<MailAddress>(),
                mailSubject = "Application Statement | Ref No.:" + studentApplication.Id,
                mailBody = body,
                mailFooter = "<br/> Many Thanks, <br/> <b>Bright Ideas</b>",
                mailPriority = MailPriority.High,
                mailAttachments = new List<Attachment>()

            });
            onceOffRequest.m_payment_id = "";
            onceOffRequest.amount = Convert.ToDouble(student.BalanceDue);
            onceOffRequest.item_name = "Tuition payment";
            onceOffRequest.item_description = "Some details about the once off payment";

            student.BalanceDue = student.Tuition - student.Tuition;
            student.date = DateTime.UtcNow.Date.ToString();
            student.Status = "Paid Full Amount";
            db.Entry(student).State = EntityState.Modified;
            db.SaveChanges();

            // Transaction Options
            onceOffRequest.email_confirmation = true;
            onceOffRequest.confirmation_address = "sbtu01@payfast.co.za";

            var redirectUrl = $"{this.payFastSettings.ProcessUrl}{onceOffRequest.ToString()}";
            return Redirect(redirectUrl);
        }

        public ActionResult AdHoc()
        {
            var adHocRequest = new PayFastRequest(this.payFastSettings.PassPhrase);

            // Merchant Details
            adHocRequest.merchant_id = this.payFastSettings.MerchantId;
            adHocRequest.merchant_key = this.payFastSettings.MerchantKey;
            adHocRequest.return_url = this.payFastSettings.ReturnUrl;
            adHocRequest.cancel_url = this.payFastSettings.CancelUrl;
            adHocRequest.notify_url = this.payFastSettings.NotifyUrl;

            // Buyer Details
            adHocRequest.email_address = "sbtu01@payfast.co.za";

            // Transaction Details
            adHocRequest.m_payment_id = "";
            adHocRequest.amount = 70;
            adHocRequest.item_name = "Adhoc Agreement";
            adHocRequest.item_description = "Some details about the adhoc agreement";

            // Transaction Options
            adHocRequest.email_confirmation = true;
            adHocRequest.confirmation_address = "sbtu01@payfast.co.za";

            // Recurring Billing Details
            adHocRequest.subscription_type = SubscriptionType.AdHoc;

            var redirectUrl = $"{this.payFastSettings.ProcessUrl}{adHocRequest.ToString()}";

            return Redirect(redirectUrl);
        }

        public ActionResult Return()
        {
            return View();
        }

        public ActionResult Cancel()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Notify([ModelBinder(typeof(PayFastNotifyModelBinder))] PayFastNotify payFastNotifyViewModel)
        {
            payFastNotifyViewModel.SetPassPhrase(this.payFastSettings.PassPhrase);

            var calculatedSignature = payFastNotifyViewModel.GetCalculatedSignature();

            var isValid = payFastNotifyViewModel.signature == calculatedSignature;

            System.Diagnostics.Debug.WriteLine($"Signature Validation Result: {isValid}");

            // The PayFast Validator is still under developement
            // Its not recommended to rely on this for production use cases
            var payfastValidator = new PayFastValidator(this.payFastSettings, payFastNotifyViewModel, IPAddress.Parse(this.HttpContext.Request.UserHostAddress));

            var merchantIdValidationResult = payfastValidator.ValidateMerchantId();

            System.Diagnostics.Debug.WriteLine($"Merchant Id Validation Result: {merchantIdValidationResult}");

            var ipAddressValidationResult = payfastValidator.ValidateSourceIp();

            System.Diagnostics.Debug.WriteLine($"Ip Address Validation Result: {merchantIdValidationResult}");

            // Currently seems that the data validation only works for successful payments
            if (payFastNotifyViewModel.payment_status == PayFastStatics.CompletePaymentConfirmation)
            {
                var dataValidationResult = await payfastValidator.ValidateData();

                System.Diagnostics.Debug.WriteLine($"Data Validation Result: {dataValidationResult}");
            }

            if (payFastNotifyViewModel.payment_status == PayFastStatics.CancelledPaymentConfirmation)
            {
                System.Diagnostics.Debug.WriteLine($"Subscription was cancelled");
            }

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        public ActionResult Error()
        {
            return View();
        }

        #endregion Methods
    }
}

