using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CML.Models
{
    
    public class LocationModel
    {
        [ScaffoldColumn(false)]
        public int LocationID { get; set; }
        [Required]
        public string Location { get; set; }
        public string Code { get; set; }
    }


}