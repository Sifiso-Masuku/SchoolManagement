using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using SchoolManagement.Model.Entity;
using SchoolManagement.Model.ViewModels;
using PagedList;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;
using System.Net.Mail;
using SchoolManagement.Models;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Configuration;
using PayFast;
using PayFast.AspNet;
using IdentitySample.Models;
using System.Threading.Tasks;

namespace SchoolManagement.Controllers
{
    public class StudentApplicationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private StudentApplicationVM stdApp = new StudentApplicationVM();
        // GET: StudentApplications
        public ActionResult Index2()
        {
            var studentapplications = db.Studentapplications.Include(s => s.ClassName);
            var username = User.Identity.GetUserName();
            return View(studentapplications.ToList().Where(x=>x.GuardianEmail == username));
        }
     
        public ActionResult Index(string option, string search,int? pageNumber, string sort)
        {
            var studentapplications = db.Studentapplications.Include(s => s.ClassName).Include(s=>s.applicationDocuments);


            //if a user choose the radio button option as Subject  
            if (option == "Surname")
            {
                //Index action method will return a view with a student records based on what a user specify the value in textbox  
                return View(studentapplications.Where(x=>x.Surname.StartsWith(search) || search == null).ToList().ToPagedList(pageNumber ?? 1, 5));
            }
            else if (option == "Status")
            {
                return View(studentapplications.Where(x => x.Status.StartsWith(search)|| search == null).ToList().ToPagedList(pageNumber ?? 1, 5));
            }
            else
            {
                return View(studentapplications.Where(x => x.Name.StartsWith(search) || search == null).ToList().ToPagedList(pageNumber ?? 1, 3));
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
        // GET: StudentApplications/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentApplication studentApplication = db.Studentapplications.Find(id);
            if (studentApplication == null)
            {
                return HttpNotFound();
            }
            return View(studentApplication);
        }
        public ActionResult ApproveApplication(int? id)
        {    
            StudentApplication studentApplication = db.Studentapplications.Find(id);
            if (studentApplication.Status == "Accepted" || studentApplication.Status == "Rejected")
            {
                TempData["AlertMessage"] = "The application has already been Rejected/Accepted";
                return RedirectToAction("Index");
            }
            else
            {
                studentApplication.Status = "Accepted";           
                db.Entry(studentApplication).State = EntityState.Modified;
              // db.Studentapplications.Add(studentApplication);
               db.SaveChanges();
                var mailTo = new List<MailAddress>();
                mailTo.Add(new MailAddress(studentApplication.GuardianEmail, studentApplication.GuardianName));
                var body = $"Hello {studentApplication.GuardianName}, Your child has been accepted to study at our school next year, please make sure you accept the offer within 7 days to avoid getting your offer forfeited. If you have accepted the offer, you will then be allowed to pay the deposit fee within 30 days to be unblocked for registration. <br/> Regards,<br/><br/> Nqabakazulu Comphrehesive High School <br/> .";

                SchoolManagement.Models.EmailService emailService = new SchoolManagement.Models.EmailService();
                emailService.SendEmail(new EmailContent()
                {
                    mailTo = mailTo,
                    mailCc = new List<MailAddress>(),
                    mailSubject = "Application Statement | Ref No.:" + studentApplication.Id,
                    mailBody = body,
                    mailFooter = "<br/> Many Thanks, <br/> <b>Nqabakazulu Comphrehesive High School</b>",
                    mailPriority = MailPriority.High,
                    mailAttachments = new List<Attachment>()

                });
                return RedirectToAction("Index");
            }

        }
        public ActionResult AcceptOffer(int? id)
        {
            StudentApplication studentApplication = db.Studentapplications.Find(id);

                studentApplication.Status = "Offer Accepted";
                db.Entry(studentApplication).State = EntityState.Modified;
                // db.Studentapplications.Add(studentApplication);
                db.SaveChanges();
                return RedirectToAction("Index2");

        }

        public ActionResult EmailstudentInfor(int? id)
        {
            Session["bookID"] = id;

            StudentApplication studentApplication = db.Studentapplications.Where(p => p.Id == id).FirstOrDefault();
            var username = User.Identity.GetUserName();
            var attachments = new List<Attachment>();
            attachments.Add(new Attachment(new MemoryStream(GeneratePDF(id)), "Proof of registration", "application/pdf"));
            var mailTo = new List<MailAddress>();
                mailTo.Add(new MailAddress(username, studentApplication.GuardianName));
                var body = $"Hello {studentApplication.GuardianName}, \n\n {studentApplication.Name} has been successfully registered at our school. Please Find the attached registration information(Proof of Registration)<br/> Regards,<br/><br/> Nqabakazulu Comphrehesive High School <br/> .";

                SchoolManagement.Models.EmailService emailService = new SchoolManagement.Models.EmailService();
            emailService.SendEmail(new EmailContent()
            {
                mailTo = mailTo,
                mailCc = new List<MailAddress>(),
                mailSubject = "Application Statement | Ref No.:" + studentApplication.Id,
                mailBody = body,
                mailFooter = "<br/> Many Thanks, <br/> <b>Nqabakazulu Comphrehesive High School</b>",
                mailPriority = MailPriority.High,
                mailAttachments = attachments

            });
            TempData["AlertMessage"] = $"{studentApplication.Name} has been successfully Registered";

            return RedirectToAction("Index2");
        }
        
        public ActionResult register(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentApplication studentApplication = db.Studentapplications.Find(id);
            if (studentApplication == null)
            {
                return HttpNotFound();
            }
            
            studentApplication.StudentNumber = stdApp.generateStudenbtNumber(studentApplication.NID);
            studentApplication.Status = "Registered";
              db.Entry(studentApplication).State = EntityState.Modified;
                db.SaveChanges();

            var username = User.Identity.GetUserName();
            Student student = db.Students.Where(p => p.NID == studentApplication.NID).FirstOrDefault();
            student.StudentNumber = stdApp.generateStudenbtNumber(studentApplication.NID);
            student.Status = "Registered";
            db.Entry(student).State = EntityState.Modified;
            db.SaveChanges();
            return View(studentApplication);
            //return RedirectToAction("Index2");

        }
        public ActionResult RejectApplication(int? id)
        {
            StudentApplication studentApplication = db.Studentapplications.Find(id);
            if (studentApplication.Status == "Accepted" || studentApplication.Status == "Rejected")
            {
                TempData["AlertMessage"] = "The application has already been Rejected/Accepted";
                return RedirectToAction("Index");
            }
            else
            {
                studentApplication.Status = "Rejected";
                db.Entry(studentApplication).State = EntityState.Modified;
                db.SaveChanges();
                var mailTo = new List<MailAddress>();
                mailTo.Add(new MailAddress(studentApplication.GuardianEmail, studentApplication.GuardianName));
                var body = $"Hello {studentApplication.GuardianName}, Your child has been rejected to study at our school next year, this is due to not meeting the admissions criteria. <br/> Regards,<br/><br/> Nqabakazulu Comphrehesive High School</ <br/> .";

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
                return RedirectToAction("Index");
            }
        }

        // GET: StudentApplications/Create
        public ActionResult Create()
        {
            var studentClass = db.StudentClasses.Select(c => new
            {
                Id = c.Id,
                Name = c.ClassName.Name + " " + c.Section.Name
            }).OrderBy(o => o.Name).ToList();
            //ViewBag.ClassNameId = new SelectList(studentClass, "Id", "Name");
            ViewBag.ClassNameId = new SelectList(db.ClassNames, "Id", "Name");

            return View();
        }

        // POST: StudentApplications/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(StudentApplicationVM studentApplication)
        {
            var username = User.Identity.GetUserName();
            var name = db.Guardians.Where(p => p.Email == username).Select(p=>p.Name).FirstOrDefault();
            if (ModelState.IsValid)
            {
                if (BILogic.CheckDOB(studentApplication.DOB))
                {
                    studentApplication.GuardianEmail = username;
                    studentApplication.GuardianName = name;
                    studentApplication.subject = stdApp.getSubject();
                    studentApplication.Status = "Pending";
                    var stdApp2 = new StudentApplication()
                    {
                        Name = studentApplication.Name,
                        Surname = studentApplication.Surname,
                        DOB = studentApplication.DOB,
                        NID = studentApplication.NID,
                        Gender = studentApplication.Gender.ToString(),
                        HomeLanguage = studentApplication.HomeLanguage.ToString(),
                        Race = studentApplication.Race.ToString(),
                        Email = studentApplication.Email,
                        GuardianEmail = studentApplication.GuardianEmail,
                        GuardianName = studentApplication.GuardianName,
                        PresentAddress = studentApplication.PresentAddress,
                        ParmanentAddress = studentApplication.ParmanentAddress,
                        Religion = studentApplication.Religion.ToString(),
                        ClassNameId = studentApplication.ClassNameId
                    };
                    db.Studentapplications.Add(stdApp2);
                    db.SaveChanges();
                    //Student table             
                    Attendance attendance = new Attendance();
                    attendance.Id = studentApplication.Id;
                    attendance.StudentName = studentApplication.Name;
                    attendance.StudentSurname = studentApplication.Surname;
                    attendance.Subject = studentApplication.subject;
                    var grade = db.ClassNames.Where(x => x.Id == studentApplication.ClassNameId).Select(x => x.Name).FirstOrDefault();
                    attendance.Grade = grade;
                    attendance.GuardianEmail = studentApplication.GuardianEmail;
                    db.Attendances.Add(attendance);
                    db.SaveChanges();

                    var priceId = db.ClassFees.Where(p => p.ClassNameId == studentApplication.ClassNameId).FirstOrDefault();
                    Student student = new Student();
                    student.studentapp = studentApplication.Id;
                    student.Name = studentApplication.Name;
                    student.Surname = studentApplication.Surname;
                    student.DateOfBirth = studentApplication.DOB;
                    student.Gender = studentApplication.Gender.ToString();
                    student.Email = studentApplication.Email;
                    student.PresentAddress = studentApplication.PresentAddress;
                    student.NID = studentApplication.NID;
                    student.subject = studentApplication.subject;
                    student.Grade = grade;
                    student.Status = "Pending Deposit";
                    student.GuardianName = studentApplication.GuardianName;
                    student.Tuition = stdApp.getPrice("Tuition", priceId.FeeTypeId);
                    student.deposit = stdApp.getPrice("Deposit", (priceId.FeeTypeId + 1));
                    student.UserName = username;
                    db.Students.Add(student);
                    db.SaveChanges();
                    return RedirectToAction("Create", "ApplicationDocuments", new { id = stdApp2.Id });
                }
                else
                {
                    ModelState.AddModelError("", "Student must be older than 10 years");
                    ViewBag.ClassNameId = new SelectList(db.ClassNames, "Id", "Name", studentApplication.ClassNameId);
                    return View(studentApplication);
                }
            }
            else
            {
                ViewBag.ClassNameId = new SelectList(db.ClassNames, "Id", "Name", studentApplication.ClassNameId);
                return View(studentApplication);
            }
        }

        // GET: StudentApplications/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentApplication studentApplication = db.Studentapplications.Find(id);
            if (studentApplication == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClassNameId = new SelectList(db.StudentClasses, "Id", "Id", studentApplication.ClassNameId);
            return View(studentApplication);
        }

        // POST: StudentApplications/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Surname,FatherName,MotherName,DOB,NID,Gender,HomeLanguage,Race,Email,PresentAddress,ParmanentAddress,Religion,ClassNameId")] StudentApplication studentApplication)
        {
            if (ModelState.IsValid)
            {
                db.Entry(studentApplication).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ClassNameId = new SelectList(db.StudentClasses, "Id", "Id", studentApplication.ClassNameId);
            return View(studentApplication);
        }

        // GET: StudentApplications/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentApplication studentApplication = db.Studentapplications.Find(id);
            if (studentApplication == null)
            {
                return HttpNotFound();
            }
            return View(studentApplication);
        }

        // POST: StudentApplications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            StudentApplication studentApplication = db.Studentapplications.Find(id);
            db.Studentapplications.Remove(studentApplication);
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
        
        public byte[] GeneratePDF(int? ReservationID)
        {
            MemoryStream memoryStream = new MemoryStream();
            iTextSharp.text.Document document = new iTextSharp.text.Document(iTextSharp.text.PageSize.A5, 0, 0, 0, 0);
            PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
            document.Open();

            int ID = int.Parse(Session["bookID"].ToString());
            StudentApplication studentapplication = new StudentApplication();
            studentapplication = db.Studentapplications.Find(ID);

            //var tenant1 = db.Tenants.Find(roomBooking.TenantId);


            //var reservation = _iReservationService.Get(Convert.ToInt64(ReservationID));
            //var user = _iUserService.Get(reservation.UserID);

            iTextSharp.text.Font font_heading_3 = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 12, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.RED);
            iTextSharp.text.Font font_body = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.BaseColor.BLUE);

            // Create the heading paragraph with the headig font
            PdfPTable table1 = new PdfPTable(1);
            PdfPTable table2 = new PdfPTable(5);
            PdfPTable table3 = new PdfPTable(1);

            iTextSharp.text.pdf.draw.VerticalPositionMark seperator = new iTextSharp.text.pdf.draw.LineSeparator();
            seperator.Offset = -6f;
            // Remove table cell
            table1.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            table3.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;

            table1.WidthPercentage = 80;
            table1.SetWidths(new float[] { 100 });
            table2.WidthPercentage = 80;
            table3.SetWidths(new float[] { 100 });
            table3.WidthPercentage = 80;

            PdfPCell cell = new PdfPCell(new Phrase(""));
            cell.Colspan = 3;
            table1.AddCell("\n");
            table1.AddCell(cell);
            table1.AddCell("\n\n");
            table1.AddCell(
                "\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t" +
                "Nqabakazulu Comphrehesive High School \n" +
                "Email :brightideas@gmail.com" + "\n" +
                "\n" + "\n");
            table1.AddCell("------------Student Details--------------!");
            table1.AddCell("Class Name : \t" + studentapplication.ClassName);
            table1.AddCell("Student Number : \t" + studentapplication.StudentNumber);
            table1.AddCell("Full Name : \t" + studentapplication.Name);
            table1.AddCell("Last Name : \t" + studentapplication.Surname);
            table1.AddCell("Identity Number : \t" + studentapplication.NID);
            table1.AddCell("Date Of Birth : \t" + studentapplication.DOB);
            table1.AddCell("Booking # : \t" + ReservationID);
            table1.AddCell("Gender : \t" + studentapplication.Gender);
            table1.AddCell("Race: \t" + studentapplication.Race);
            table1.AddCell("Guardian Name : \t" + studentapplication.GuardianName);
            table1.AddCell("Subject : \t" + studentapplication.subject);

            table1.AddCell("\n");

            table3.AddCell("------------Looking forward to hear from you soon--------------!");

            //////Intergrate information into 1 document
            //var qrCode = iTextSharp.text.Image.GetInstance(reservation.QrCodeImage);
            //qrCode.ScaleToFit(200, 200);
            table1.AddCell(cell);
            document.Add(table1);
            //document.Add(qrCode);
            document.Add(table3);
            document.Close();

            byte[] bytes = memoryStream.ToArray();
            memoryStream.Close();
            return bytes;
        }
        public StudentApplicationsController()
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
            StudentApplication studentApplication = db.Studentapplications.Find(id);
            var priceId = db.ClassFees.Where(p => p.ClassNameId == studentApplication.ClassNameId).FirstOrDefault();
            //var price = db.FeeTypes.Where(p => p.Id == priceId.FeeTypeId && p.Name == "Deposit").FirstOrDefault();
            studentApplication.Status = "Deposit Paid";

            db.Entry(studentApplication).State = EntityState.Modified;
            db.SaveChanges();

            // Transaction Details
            Student student = db.Students.Where(p=>p.NID==studentApplication.NID).FirstOrDefault();
            //student.deposit = SchoolLogic.getPrice("Deposit", (priceId.FeeTypeId + 1));
            //student.deposit = price.FeeAmount;
 

            onceOffRequest.m_payment_id = "";
            onceOffRequest.amount = Convert.ToDouble(student.deposit);
            onceOffRequest.item_name = "Deposit payment";
            onceOffRequest.item_description = "Some details about the once off payment";

            student.BalanceDue = student.Tuition - student.deposit;
            student.Status = "Deposit Paid";
            db.Entry(student).State = EntityState.Modified;
            db.SaveChanges();

            var mailTo = new List<MailAddress>();
            mailTo.Add(new MailAddress(studentApplication.GuardianEmail, studentApplication.GuardianName));
            var body = $"Hello {studentApplication.GuardianName}, Amount Paid {student.deposit}, Amount Remaining {student.BalanceDue}, Student Number {studentApplication.StudentNumber}, First Name {studentApplication.Name}, Student Surname {studentApplication.Surname}, Date:{System.DateTime.Now.Date}<br/> Regards,<br/><br/> Bright Ideas <br/> .";

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
