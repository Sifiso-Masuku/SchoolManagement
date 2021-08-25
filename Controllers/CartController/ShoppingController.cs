using IdentitySample.Models;
using Microsoft.AspNet.Identity;
using SchoolManagement.Models;
using SchoolManagement.Models.CartModels;
using SchoolManagement.Services.CartServices;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace SchoolManagement.Controllers.CartController
{
    public class ShoppingController : Controller
    {
        private Cart_Service cart_Service;
        private Item_Service item_Service;
        private Order_Service order_Service;
        private Address_Service address_Service;
        Category_Service Category_Service;
        private ApplicationDbContext db = new ApplicationDbContext();

        public ShoppingController()
        {
            this.cart_Service = new Cart_Service();
            this.item_Service = new Item_Service();
            this.order_Service = new Order_Service();
            this.address_Service = new Address_Service();
            this.Category_Service = new Category_Service();
        }
        public ActionResult Index(int? id)
        {
            var items_results = new List<Item>();
            try
            {
                if (id != null)
                {
                    if (id == 0)
                    {
                        items_results = item_Service.GetItems();
                        ViewBag.Department = "All Categories";
                    }
                    else
                    {
                        items_results = item_Service.GetItems().Where(x => x.Category_ID == (int)id).ToList();
                        ViewBag.Department = Category_Service.GetCategory(id).Name;
                    }
                }
                else
                {
                    items_results = item_Service.GetItems();
                    ViewBag.Department = "All Categories";
                }
            }
            catch (Exception ex) { }
            return View(items_results);
        }
        public ActionResult add_to_cart(int id)
        {
            var item = item_Service.GetItem(id);
            if (item != null)
            {
                cart_Service.AddItemToCart(id);
                return RedirectToAction("Index");
            }
            else
                return RedirectToAction("Not_Found", "Error");
        }
        public ActionResult remove_from_cart(string id)
        {
            var item = cart_Service.GetCartItems().FirstOrDefault(x => x.cart_item_id == id);
            if (item != null)
            {
                cart_Service.RemoveItemFromCart(id: id);
                return RedirectToAction("ShoppingCart");
            }
            else
                return RedirectToAction("Not_Found", "Error");
        }
        public ActionResult ShoppingCart()
        {
            ViewBag.Total = cart_Service.GetCartTotal(cart_Service.GetCartID());
            ViewBag.TotalQTY = cart_Service.GetCartItems().FindAll(x => x.cart_id == cart_Service.GetCartID()).Sum(q => q.quantity);
            return View(cart_Service.GetCartItems().FindAll(x => x.cart_id == cart_Service.GetCartID()));
        }
        [HttpPost]
        public ActionResult ShoppingCart(List<Cart_Item> items)
        {
            foreach (var i in items)
            {
                cart_Service.UpdateCart(i.cart_item_id, i.quantity);
            }
            return RedirectToAction("ShoppingCart");
        }
        public ActionResult countCartItems()
        {
            int qty = cart_Service.GetCartItems().Sum(x => x.quantity);
            return Content(qty.ToString());
        }
        public ActionResult Checkout()
        {
            if (cart_Service.GetCartItems().Count == 0)
            {
                ViewBag.Err = "Opps... you should have atleat one cart item, please shop a few items";
                return RedirectToAction("Index");
            }
            else
                return RedirectToAction("PlaceOrder", new { id = "collect" });

        }
        [Authorize]
        public ActionResult HowToGetMyOrder()
        {
            return View();
        }
        [HttpPost]
        public ActionResult HowToGetMyOrder(string street_number, string street_name, string City, string State, string ZipCode, string Country)
        {
            Session["street_number"] = street_number;
            Session["street_name"] = street_name;
            Session["City"] = City;
            Session["State"] = State;
            Session["ZipCode"] = ZipCode;
            Session["Country"] = Country;
            return RedirectToAction("PlaceOrder", new { id = "deliver" });
        }
        [Authorize]
        public ActionResult PlaceOrder(string id)
        {

            var userName = User.Identity.GetUserName();
            /* Find the details of the customer placing the order*/
            var customer = db.Guardians.Where(x => x.Email == userName).FirstOrDefault();
            //var customer = db.Customers.Where(x => x.Email == userName).FirstOrDefault();
            /* Place the order */
            order_Service.AddOrder(customer);
            /* Get the last placed order by the cus.tomer */
            var order = order_Service.GetOrders()
                .FindAll(x => x.Email == userName)
                .OrderByDescending(x => x.date_created)
                .FirstOrDefault();
            /* If the customer requests delivery, save order address */
            if (id == "deliver")
            {
                address_Service.AddShippingAddress(new Shipping_Address()
                {
                    Order_ID = order.Order_ID,
                    street_number = Convert.ToInt16(Session["street_number"].ToString()),
                    street_name = Session["street_name"].ToString(),
                    City = Session["City"].ToString(),
                    State = Session["State"].ToString(),
                    ZipCode = Session["ZipCode"].ToString(),
                    Country = Session["Country"].ToString(),

                    Building_Name = "",
                    Floor = "",
                    Contact_Number = "",
                    Comments = "",
                    Address_Type = ""
                });
            }
            /* Migrate cart items to map as order items */
            order_Service.AddOrderItems(order, cart_Service.GetCartItems());
            /* Empty the cart items */
            cart_Service.EmptyCart();
            /* Update Order Tracking Report */
            order_Service.AddOrderTrackingReport(new Order_Tracking()
            {
                order_ID = order.Order_ID,
                date = DateTime.Now,
                status = "Awaiting Payment",
                Recipient = ""
            });


            //Redirect to payment
            return RedirectToAction("Payment", new { id = order.Order_ID });
        }
        public ActionResult Payment(string id)
        {
            return View(order_Service.GetOrderDetail(id));
        }
        public ActionResult Secure_Payment(string id)
        {
            var order = order_Service.GetOrder(id);
            return Redirect(PaymentLink(order_Service.GetOrderTotal(order.Order_ID).ToString(), "Order Payment | Order No: " + order.Order_ID, order.Order_ID));
        }
        public ActionResult Return_Url(string id)
        {
            var order = order_Service.GetOrder(id);

            ViewBag.Order = order;
            ViewBag.Account = db.Guardians.Where(x => x.Email == order.Email).FirstOrDefault();
            ViewBag.Address = address_Service.GetOrderAddresses().Find(x => x.Order_ID == order.Order_ID);
            ViewBag.Items = order_Service.GetOrderItems(order.Order_ID);
            ViewBag.Total = order_Service.GetOrderTotal(order.Order_ID);
            return View();
        }
        public ActionResult Payment_Successfull(string id)
        {
            try
            {
                var order = order_Service.GetOrder(id);
                order_Service.Update_Stock(id);
                order_Service.EstimateDeliveryDateReport(order);
            }
            catch (Exception ex) { }
            return View();
        }

        public string PaymentLink(string totalCost, string paymentSubjetc, string order_id)
        {

            string paymentMode = ConfigurationManager.AppSettings["PaymentMode"], site, merchantId, merchantKey, returnUrl, cancelUrl, PF_NotifyURL;

            if (paymentMode == "test")
            {
                site = "https://sandbox.payfast.co.za/eng/process?";
                merchantId = "10005631";
                merchantKey = "znbndc034nb6b";
            }
            else if (paymentMode == "live")
            {
                site = "https://www.payfast.co.za/eng/process?";
                merchantId = ConfigurationManager.AppSettings["PF_MerchantID"];
                merchantKey = ConfigurationManager.AppSettings["PF_MerchantKey"];
            }
            else
            {
                throw new InvalidOperationException("Payment method unknown.");
            }
            var stringBuilder = new StringBuilder();

            PF_NotifyURL = Url.Action("Payment_Successfull", "Shopping",
                new System.Web.Routing.RouteValueDictionary(new { id = order_id }),
                "http", Request.Url.Host);
            returnUrl = Url.Action("Order_Details", "Orders",
                new System.Web.Routing.RouteValueDictionary(new { id = order_id }),
                "http", Request.Url.Host);
            cancelUrl = Url.Action("Payment", "Shopping",
                new System.Web.Routing.RouteValueDictionary(new { id = order_id }),
                "http", Request.Url.Host);

            /* mechant details */
            stringBuilder.Append("&merchant_id=" + HttpUtility.HtmlEncode(merchantId));
            stringBuilder.Append("&merchant_key=" + HttpUtility.HtmlEncode(merchantKey));
            stringBuilder.Append("&return_url=" + HttpUtility.HtmlEncode(returnUrl));
            stringBuilder.Append("&cancel_url=" + HttpUtility.HtmlEncode(cancelUrl));
            stringBuilder.Append("&notify_url=" + HttpUtility.HtmlEncode(PF_NotifyURL));
            /* buyer details */
            var customer = order_Service.GetOrders().FirstOrDefault(x => x.Email == User.Identity.GetUserName());
            if (customer != null)
            {
                stringBuilder.Append("&name_first=" + HttpUtility.HtmlEncode(customer.Email));
                stringBuilder.Append("&name_last=" + HttpUtility.HtmlEncode(customer.Email));
                stringBuilder.Append("&email_address=" + HttpUtility.HtmlEncode(customer.Email));
                stringBuilder.Append("&cell_number=" + HttpUtility.HtmlEncode(customer.Email));
            }
            /* Transaction details */
            var order = order_Service.GetOrder(order_id);
            if (order != null)
            {
                stringBuilder.Append("&m_payment_id=" + HttpUtility.HtmlEncode(order.Order_ID));
                stringBuilder.Append("&amount=" + HttpUtility.HtmlEncode((decimal)order_Service.GetOrderTotal(order.Order_ID)));
                stringBuilder.Append("&item_name=" + HttpUtility.HtmlEncode(paymentSubjetc));
                stringBuilder.Append("&item_description=" + HttpUtility.HtmlEncode(paymentSubjetc));

                stringBuilder.Append("&email_confirmation=" + HttpUtility.HtmlEncode("1"));
                stringBuilder.Append("&confirmation_address=" + HttpUtility.HtmlEncode(ConfigurationManager.AppSettings["PF_ConfirmationAddress"]));
            }

            return (site + stringBuilder);
        }
    }
}