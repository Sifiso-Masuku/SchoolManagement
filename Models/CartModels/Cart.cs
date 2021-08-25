using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace  SchoolManagement.Models.CartModels
{
    public class Cart
    {
        [Key]
        public string cart_id { get; set; }
        public DateTime date_created { get; set; }

        public virtual ICollection<Cart_Item> Cart_Items { get; set; }
    }
}