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
    }
}