using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CML.Models
{
    public class SampleModel
    {
        [Display( Name = "Sample ID" )]
        public int ID { get; set; }
        [Display( Name = "Test ID" )]
        public Nullable<int> TestID { get; set; }
        [Display( Name = "Negative Comment" )]
        public string NegComment { get; set; }
        [Display( Name = "Negative Other IDs" )]
        public string NegOtherID { get; set; }
        [Display( Name = "Negative Other Amount" )]
        public Nullable<int> NegOtherNumber { get; set; }
        [Display( Name = "Negative IDs" )]
        public string NegStandardIDs { get; set; }
        [Display( Name = "Negative Amount" )]
        public Nullable<int> NegStandardNbr { get; set; }
        [Display( Name = "Negative Top Middle Bottom IDs" )]
        public string NegTMBIds { get; set; }
        [Display( Name = "Negative Top Middle Bottom Amount" )]
        public Nullable<int> NegTMBNbr { get; set; }
        [Display( Name = "Amount" )]
        public Nullable<int> Number { get; set; }
        [Display( Name = "Oxide Type" )]
        public string OxideType { get; set; }
        [Display( Name = "Positive Comment" )]
        public string PosComment { get; set; }
        [Display( Name = "Positive Other IDs" )]
        public string PosOtherID { get; set; }
        [Display( Name = "Positive Other Amount" )]
        public Nullable<int> PosOtherNumber { get; set; }
        [Display( Name = "Positive IDs" )]
        public string PosStandardIDs { get; set; }
        [Display( Name = "Positive Amount" )]
        public Nullable<int> PosStandardNbr { get; set; }
        [Display( Name = "Positive Top Middle Bottom IDs" )]
        public string PosTMBIds { get; set; }
        [Display( Name = "Positive Top Middle Bottom Amount" )]
        public Nullable<int> PosTMBNbr { get; set; }
        [Display( Name = "Comments" )]
        public string SampleDesc { get; set; }
        [Display( Name = "IDs" )]
        public string SampleIDs { get; set; }
        [Display( Name = "Name" )]
        public string SampleName { get; set; }
        [Display( Name = "Separator Type" )]
        public string SeperatorType { get; set; }

        public bool IsNewSample { get; set; }
        [Display( Name = "Total Samples" )]
        public Nullable<int> TotalSamples { get; set; }
        [Display( Name = "Sample Type" )]
        public string SampleType { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly" )]
        public virtual ICollection<SampleModel> Samples { get; set; }

        public virtual Test Test { get; set; }
    }
}