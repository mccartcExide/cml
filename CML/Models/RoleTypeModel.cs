using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CML.Models
{
    public class RoleTypeModel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public RoleTypeModel()
        {
            this.CML_User = new HashSet<UserModel>();
        }

        [ScaffoldColumn(false)]
        public int ID { get; set; }
        [Required]
        [Display(Name ="Role")]
        public string RoleType { get; set; }
        

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserModel> CML_User { get; set; }
    }
}