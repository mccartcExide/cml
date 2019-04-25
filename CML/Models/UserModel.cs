using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CML.Models
{
    public class UserModel
    {
        public UserModel()
        {
            this.CML_Approvals = new HashSet<CML_Approvals>();
            this.Requests = new HashSet<Request>();
        }

        [ScaffoldColumn(false)]
        public int ID { get; set; }
        [Required]
        public string UserID { get; set; }
        [Required]
        public string Email { get; set; }
        public int RoleTypeID { get; set; }
        [Required]
        public string DisplayName { get; set; }
        public virtual ICollection<CML_Approvals> CML_Approvals { get; set; }

        public virtual CML_RoleType CML_RoleType { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Request> Requests { get; set; }
    }
}