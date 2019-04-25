using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CML.Models
{
    public class TestModel
    {
        public TestModel()
        {
            this.Samples = new HashSet<Sample>();
        }

        public int TestID { get; set; }
        public Nullable<int> CMLRequest { get; set; }
        public Nullable<int> TestDef { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public int? AssignedTo { get; set; }
        public string assigned { get; set; }
        public string Comments { get; set; }
        public Nullable<int> TotalSamples { get; set; }
        public Nullable<int> StatusID { get; set; }
        public string Status { get; set; }
        public Nullable<System.DateTime> TestStarted { get; set; }
        public Nullable<System.DateTime> TestFinished { get; set; }
        public string SamplePreparations { get; set; }
        public string SampleAnalysis { get; set; }
        public string DataAnalysis { get; set; }
        public string Reports { get; set; }
        public string Cleaning { get; set; }
        public string Total { get; set; }
        public string Name { get; set; }
        public string Abbrev { get; set; }
        public string Determines { get; set; }
        public string SampleType { get; set; }
        public string SampleSize { get; set; }
        public bool IsFromRequest { get; set; }
        public bool SaveAndStay { get; set; }
        public string Note { get; set; }
        public virtual AttachmentsModel Attachments { get; set; }
        public virtual ICollection<Note> TestNotes { get; set; }
        public virtual CML_TestDefinition CML_TestDefinition { get; set; }
        public virtual Request Request { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Sample> Samples { get; set; }
        public virtual CML_User CML_User { get; set; }
        public virtual CML_Status CML_Status { get; set; }
    }
}