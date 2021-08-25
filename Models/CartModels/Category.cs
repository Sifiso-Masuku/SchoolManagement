using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace  SchoolManagement.Models.CartModels
{
    public class Category
    {
        [Key]
        [Display(Name = "ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Category_ID { get; set; }
        [Required]
        [Display(Name = "Name")]
        [Index("Category_Index", IsUnique = true)]
        [MinLength(3)]
        [MaxLength(80)]
        public string Name { get; set; }
        public virtual ICollection<Item> Items { get; set; }
    }
}