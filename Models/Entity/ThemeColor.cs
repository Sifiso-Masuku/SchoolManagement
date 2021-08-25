using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.Model.Entity
{
    public class ThemeColor
    {
        [Key]
        public int colorID { get; set; }
        [DisplayName("Color Name")]
        public string colorName { get; set; }
        public bool archived { get; set; }
    }
}