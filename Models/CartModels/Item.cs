using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace  SchoolManagement.Models.CartModels
{
    public class Item
    {
        [Key]
        [Display(Name = "ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ItemCode { get; set; }
        [Required]
        [ForeignKey("Category")]
        [Display(Name = "Category")]
        public int Category_ID { get; set; }

        [Required]
        [Display(Name = "Name")]
        [MinLength(3)]
        [MaxLength(80)]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        [MinLength(3)]
        [MaxLength(255)]
        public string Description { get; set; }
        [Display(Name = "Qty in Stock")]
        public int QuantityInStock { get; set; }
        //[Required]
        [Display(Name = "Picture")]
        //[DataType(DataType.Upload)]
        public byte[] Picture { get; set; }
        [Required]
        [Display(Name = "Price")]
        [DataType(DataType.Currency)]
        public double Price { get; set; }
        public int ReviewId { get; set; }
        public int Rate { get; set; }

        public virtual Category Category { get; set; }
        public virtual ICollection<Cart_Item> Cart_Items { get; set; }

        //public int getAverage()
        //{

        //}
    }
}