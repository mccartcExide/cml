using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CML.Models
{
    public class ApprovalModel
    {
        public CML_Approvals Approval { get; set; }

        public CMLRequest Request { get; set; }
        public int ReqiestID { get; set; }
        public string RequestedBy { get; set; }
        public string AssignedTo { get; set; }
        public string ActionTaken { get; set; }
        public int ApprovalID { get; set; }
        public string Comments { get; set; }
        public bool IsAllowed { get; set; }
    }
}