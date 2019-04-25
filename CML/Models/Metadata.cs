using System.ComponentModel.DataAnnotations;

namespace CML.Models
{
    public class Metadata
    {
        [ScaffoldColumn(false)]
        public int ID;
        [Required]
        [Display(Name = "Network ID")]
        public string UserID;
        [Required]
        public string Email;
        public int RoleTypeID;
        [Required]
        [Display(Name ="First Name")]
        public string FirstName;
        [Required]
        [Display(Name = "Last Name")]
        public string LastName;

    }
}