using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SchoolManagement.Model.Entity;

namespace  SchoolManagement.Models.CartModels
{
    public class Order
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Order_ID { get; set; }
        public DateTime date_created { get; set; }
        public string Email { get; set; }
        public bool shipped { get; set; }
        public string status { get; set; }
        public bool packed { get; set; }

        public virtual ICollection<Order_Item> Order_Items { get; set; }
        public virtual ICollection<Shipping_Address> Shipping_Addresses { get; set; }
        //public virtual Guardian Guardian { get; set; }

        public virtual ICollection<Order_Tracking> Order_Tracking { get; set; }
    }
}