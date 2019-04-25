using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CML.Models
{
    public class RequestedItem
    {
        public int SelectedTestID { get; set; }
        public int RequestID { get; set; }

        public CMLRequest RequestItem { get; set; }
        public IEnumerable<Test> Tests {get; set;}
        public IEnumerable<Sample> Samples { get; set; }
       // public IEnumerable<CML_TestDefinition> Definitions { get; set; }

    }
}