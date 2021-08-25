using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SchoolManagement.Model.Entity
{
    public class Guardian
    {
        [Required]
        public string Name { get; set; }
        public string Surname { get; set; }
        [ DisplayName("National ID NO")]
        public string NID { get; set; }
        [Required]
        public string Phone { get; set; }
        [Key]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        //[RegularExpression(pattern: @"^\w+[\w-\.]*\@\w+((-\w+)|(\w*))\.[a-z]{2,3}$", ErrorMessage = "Email not valid")]
        public string Email { get; set; }
        [DisplayName("Home Address")]
        public string HomeAddress { get; set; }





      

    }
}