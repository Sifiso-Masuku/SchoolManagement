using IdentitySample.Models;
using SchoolManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace SchoolManagement.Services
{
    public class EmailSender
    {
        private static ApplicationDbContext db = new ApplicationDbContext();

        public static void ResidenceApplication(ResidenceApplication residenceApplication)
        {
            var mailTo = new List<MailAddress>();
            mailTo.Add(new MailAddress(residenceApplication.UeserEmail, residenceApplication.UeserEmail));
            var body = $"Good Day {residenceApplication.UeserEmail}," +
                $" We have received your residence application, this email provides a trail with your application status." +
                $" < br /> We will keep in contact with you as to the progress of your application." +
                $"<br/> This email confrims your residence application, if you have anny further enquiries feel free to contact us.";

            Models.EmailService emailService = new Models.EmailService();
            emailService.SendEmail(new EmailContent()
            {
                mailTo = mailTo,
                mailCc = new List<MailAddress>(),
                mailSubject = $"Residence Application ({residenceApplication.Status})!!  | Ref No.:" + residenceApplication.DateApplied,
                mailBody = body,
                mailFooter = $"<br/> Kind Regards, <br/> <b>Nqabakazulu Comprehensive High School </b>",
                mailPriority = MailPriority.High,
                mailAttachments = new List<Attachment>()

            });
        }
    }
}