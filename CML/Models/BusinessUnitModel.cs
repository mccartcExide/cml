using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CML.Models
{
    public class BusinessUnitModel
    {
        [ScaffoldColumn(false)]
        public Int32 ID { get; set; }
        [Required]
        public String BusinessUnit { get; set; }
        public Nullable<int> ApproverID { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly" )]
        public virtual ICollection<Request> Requests { get; set; }
        public virtual CML_User CML_User { get; set; }
    }
}