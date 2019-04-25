using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CML.Models
{
    public class RequestTypeModel
    {
        [ScaffoldColumn(false)]
        public int ID { set; get; }

        [Required]
        public string RequestType { set; get; }


    }
}