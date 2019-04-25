using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CML.Models
{
    public class AttachmentsModel
    {
        public int ParentID { get; set; }
        public string ParentType { get; set; }

        public virtual ICollection<Files> Attachments { get; set; }
    }
    public class Files
    {
        public string Name { get; set; }
        public string Extension { get; set; }
        public int Size { get; set; }
        
    }
}