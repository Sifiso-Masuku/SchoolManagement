using SchoolManagement.Model.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SchoolManagement.Model.Entity
{
    public class ApplicationDocuments
    {
        [Key]
        public int Id { get; set; }
        [DisplayName("Application Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? ApplicationDate { get; set; }
        public int StudentApplicationId { get; set; }
        public virtual StudentApplication StudentApplication { get; set; }
        
        [Required, DisplayName("Previous School Name ")]
        public string PreviousSchool { get; set; }
        
        [Required, DisplayName("Previous School Address ")]
        public string PreviousSchoolAddrs { get; set; }
       
        [DisplayName("Latest Report ")]
        public byte[] PreviousSchoolDocument { get; set; }
        [DisplayName("Certified Birth Certificate ")]
        public byte[] Certificate { get; set; }
        [DisplayName("Certified ID/Passport ")]
        public byte[] CertifiedID { get; set; }
        [DisplayName("Proof of Home Address ")]
        public byte[] HomeAddress { get; set; }
        
    }
}