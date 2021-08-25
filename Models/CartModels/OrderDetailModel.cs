using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using SchoolManagement.Model.Entity;

namespace  SchoolManagement.Models.CartModels
{
    public class OrderDetailModel
    {
        public Guardian customer { get; set; }
        public Order order { get; set; }
        public string shipping_method { get; set; }
        public Order_Delivery delivery { get; set; }
        public Shipping_Address address { get; set; }
        [Display(Name = "Payment Method")]
        public string payment_Method { get; set; }
        public List<Order_Item> order_items { get; set; }
        [Display(Name = "Order Total")]
        [DataType(DataType.Currency)]
        public decimal order_total { get; set; }
        [DataType(DataType.Date)]
        public string date { get; set; }
    }
}