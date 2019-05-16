using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CML.Models
{
    public class ApprovalGridModel
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public string Outcome { get; set; }
        public System.DateTime DateAssigned { get; set; }
        public Nullable<System.DateTime> DateActioned { get; set; }
        public string Comments { get; set; }
        public int RequestID { get; set; }
        public string CMLNumber { get; set; }
        public string Name { get; set; }

    }
}