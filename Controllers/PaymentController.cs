//using PayFast;
using PayFast.AspNet;
using System.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PayFast;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using SchoolManagement.Models.CartModels;
using Microsoft.AspNet.Identity;
using System.Net.Mail;
using SchoolManagement.Services.CartServices;
using SchoolManagement.Models;
using SchoolManagement.Services;
using System.Data.Entity;
using IdentitySample.Models;

namespace SchoolManagement.Controllers
{
    public class PaymentController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();
        private Order_Service order_Service;



        // GET: Payment
        public ActionResult Index()
        {
            return View();
        }

        private readonly PayFastSettings payFastSettings;

        #region Constructor

        public PaymentController()
        {
            this.payFastSettings = new PayFastSettings
            {
                MerchantId = ConfigurationManager.AppSettings["MerchantId"],
                MerchantKey = ConfigurationManager.AppSettings["MerchantKey"],
                PassPhrase = ConfigurationManager.AppSettings["PassPhrase"],
                ProcessUrl = ConfigurationManager.AppSettings["ProcessUrl"],
                ValidateUrl = ConfigurationManager.AppSettings["ValidateUrl"],
                ReturnUrl = ConfigurationManager.AppSettings["ReturnUrl"],
                CancelUrl = ConfigurationManager.AppSettings["CancelUrl"],
                NotifyUrl = ConfigurationManager.AppSettings["NotifyUrl"]
            };
            order_Service = new Order_Service();

        }

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

        public ActionResult OnceOff(string id)
        {
            var order = order_Service.GetOrder(id);
            var onceOffRequest = new PayFastRequest(this.payFastSettings.PassPhrase);
            Order order1 = db.Orders.Find(id);
            order1.status = "Paid";
            db.Entry(order1).State = EntityState.Modified;
            db.SaveChanges();
            //var order=
            // Merchant Details
            onceOffRequest.merchant_id = this.payFastSettings.MerchantId;
            onceOffRequest.merchant_key = this.payFastSettings.MerchantKey;
            onceOffRequest.return_url = this.payFastSettings.ReturnUrl;
            onceOffRequest.cancel_url = this.payFastSettings.CancelUrl;
            onceOffRequest.notify_url = this.payFastSettings.NotifyUrl;

            // Buyer Details
            onceOffRequest.email_address = "sbtu01@payfast.co.za";
            //double amount = Convert.ToDouble(db.Items.Select(x => x.CostPrice).FirstOrDefault());
            //var products = db.Items.Select(x => x.Name).ToList();
            // Transaction Details
            var userName = User.Identity.GetUserName();
            onceOffRequest.m_payment_id = "";
            onceOffRequest.amount = order_Service.GetOrderTotal(order.Order_ID);
            onceOffRequest.item_name = "Once off option";
            onceOffRequest.item_description = "Some details about the once off payment";


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
            #endregion Methods
            // Buyer Details
            adHocRequest.email_address = "sbtu01@payfast.co.za";
            //double amount = Convert.ToDouble(db.FoodOrders.Select(x => x.Total).FirstOrDefault());
            //var products = db.FoodOrders.Select(x => x.UserEmail).ToList();
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
    }
}