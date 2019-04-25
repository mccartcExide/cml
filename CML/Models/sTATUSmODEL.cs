using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CML.Models
{
    public class StatusModel
    {
        [ScaffoldColumn(false)]
        public int ID { set; get; }

        [Required]
        public string Status { get; set; }
    }
}