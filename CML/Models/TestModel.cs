using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CML.Models
{
    public class TestStartResultModel
    {
        public Nullable<System.DateTime> time { get; set; }
        public int status { get; set; }
        public string message { get; set; }
    }
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
        public Nullable<double> SamplePreparations { get; set; }
        public Nullable<double> SampleAnalysis { get; set; }
        public Nullable<double> DataAnalysis { get; set; }
        public Nullable<double> Reports { get; set; }
        public Nullable<double> Cleaning { get; set; }
        public Nullable<double> Total { get; set; }
        public string Name { get; set; }
        public string Abbrev { get; set; }
        public string Determines { get; set; }
        public string SampleType { get; set; }
        public string SampleSize { get; set; }
        public bool IsFromRequest { get; set; }
        public bool SaveAndStay { get; set; }
        public bool AssignedChanged { get; set; }
        public string Note { get; set; }
        public bool IsManager { get; set; }
        public string ClosureNote { get; set; }
        public virtual AttachmentsModel Attachments { get; set; }
        public virtual ICollection<Note> TestNotes { get; set; }
        public virtual CML_TestDefinition CML_TestDefinition { get; set; }
        public virtual Request Request { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Sample> Samples { get; set; }
        public virtual CML_User CML_User { get; set; }
        public virtual CML_Status CML_Status { get; set; }

        //public TestModel Convert(Test t )
        //{
        //    this.Abbrev = t.Abbrev;
        //    this.AssignedTo = t.AssignedTo;
        //    this.Cleaning = t.Cleaning;
        //    this.CMLRequest = t.CMLRequest;
        //    this.CML_Status = t.CML_Status;
        //    this.CML_TestDefinition = t.CML_TestDefinition;
        //    this.CML_User = t.CML_User;
        //    this.Comments = t.Comments;
        //    this.CreatedBy = t.CreatedBy;
        //    this.CreatedOn = t.CreatedOn;
        //    this.DataAnalysis = t.DataAnalysis;
        //    this.Determines = t.Determines;
        //    this.Name = t.Name;
        //    this.Note = t.


        //    return this;
        //}
    }
}