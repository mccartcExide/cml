//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CML.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Notification
    {
        public int ID { get; set; }
        public Nullable<int> RequestID { get; set; }
        public Nullable<int> TestID { get; set; }
        public int Recipient { get; set; }
        public string MessageType { get; set; }
        public System.DateTime LoggedOn { get; set; }
        public Nullable<System.DateTime> SentOn { get; set; }
        public bool Sent { get; set; }
    
        public virtual CML_User CML_User { get; set; }
        public virtual Request Request { get; set; }
        public virtual Test Test { get; set; }
    }
}
