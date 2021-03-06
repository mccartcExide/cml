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
    
    public partial class Sample
    {
        public int ID { get; set; }
        public Nullable<int> TestID { get; set; }
        public Nullable<int> SampleTypeID { get; set; }
        public Nullable<int> TotalSamples { get; set; }
        public string NegComment { get; set; }
        public string NegOtherID { get; set; }
        public Nullable<int> NegOtherNumber { get; set; }
        public string NegStandardIDs { get; set; }
        public Nullable<int> NegStandardNbr { get; set; }
        public string NegTMBIds { get; set; }
        public Nullable<int> NegTMBNbr { get; set; }
        public Nullable<int> Number { get; set; }
        public string OxideType { get; set; }
        public string PosComment { get; set; }
        public string PosOtherID { get; set; }
        public Nullable<int> PosOtherNumber { get; set; }
        public string PosStandardIDs { get; set; }
        public Nullable<int> PosStandardNbr { get; set; }
        public string PosTMBIds { get; set; }
        public Nullable<int> PosTMBNbr { get; set; }
        public string SampleDesc { get; set; }
        public string SampleIDs { get; set; }
        public string SampleName { get; set; }
        public string SeperatorType { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
    
        public virtual CML_SampleType CML_SampleType { get; set; }
        public virtual Test Test { get; set; }
    }
}
