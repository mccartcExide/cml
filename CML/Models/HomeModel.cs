using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CML.Models
{
    public class HomeModel
    {
        public int MyOpenCount { get; set; }
        public int RequestAssigned { get; set; }
        public int RequestsForApproval { get; set; }
        public int TestsStarted { get; set; }
        public int TestsNotStarted { get; set; }
        public int AwaitingApproval { get; set; }
        public int RequestsTesting { get; set; }
        public int RequestTestsComplete { get; set; }

        public int UserID { get; set; }
    }
}