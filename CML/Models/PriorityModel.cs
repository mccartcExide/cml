using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CML.Models
{
    public class PriorityModel
    {
        [ScaffoldColumn(false)]
        public int ID { get; set; }
        [Required]
        public string Priority { get; set; }
    }
}