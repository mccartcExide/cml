using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CML.Models
{
    public class PieChartStatus
    {
        public int ID { get; set; }
        public string Status { get; set; }
        public int count { get; set; }
    }
    
    public class CMLRequest
    {
        [ScaffoldColumn(false)]
        public int RequestID { get; set; }
        [Required]
        public string Name { get; set; }
        
        [Display(Name="CML Number")]
        public string CMLNumber { get; set; }
        [Display(Name = "Location")]
        [Range(1, int.MaxValue, ErrorMessage ="Please select a Location")]
        public int LocationID { get; set; }
        [Required]
        [Display(Name = "Requested by")]
        public int? RequestedBy { get; set; }
        [Required]
        [Display(Name = "Business Unit")]
        public int BusinessUnitID { get; set; }
        [Required]
        [Display(Name = "Project#")]
        public string ProjectNumber { get; set; }
        [Required]
        [Display(Name = "Deviation#")]
        public string DeviationNumber { get; set; }
        [Required]
        [Display(Name = "EWR#")]
        public string EWRNumber { get; set; }
        [Display(Name ="Watch List")]
        public string[] WatchList { get; set; }
        [Display(Name ="Closure Notes")]
        public string ClosureNotes { get; set; }
        public Nullable<bool> DirectorApprovalRequired { get; set; }
        [Display(Name = "Status")]
        public int StatusID { get; set; }
        [Required]
        [Display(Name = "Priority")]
        public int PriorityID { get; set; }
        [Required]
        [Display(Name = "Disposition")]
        public int DispositionID { get; set; }
        [Display(Name = "Retention date")]
        public Nullable<System.DateTime> RetentionDate { get; set; }
        public string Phone { get; set; }
        [Required]
        
        [Display(Name = "Test Objectives")]
        public string TestObjectives { get; set; }
        [Display(Name = "Created")]
        public Nullable<System.DateTime> CreatedOn { get; set; }
        [Display(Name = "Required by")]
        public Nullable<System.DateTime> DateRequired { get; set; }
        [Display(Name = "Created by")]
        public string CreatedBy { get; set; }
        [Display(Name = "Updated on")]
        public Nullable<System.DateTime> UpdateOn { get; set; }
        [Display(Name = "Updated on")]
        public string UpdatedBy { get; set; }
        [Display(Name = "Tests started on")]
        public Nullable<System.DateTime> TestsStarted { get; set; }
        [Display(Name = "Tests completed on")]
        public Nullable<System.DateTime> TestsFinished { get; set; }
        [Required]
        [Display(Name = "Request type")]
        public int RequestTypeID { get; set; }
        [Display(Name = "Assigned to")]
        public int? AssignedTo { get; set; }
        [Display(Name ="Total Number of Samples")]
        public Nullable<int> TotalSamples { get; set; }
        public int RequestStatus { get; set; }
        public string Note { get; set; }
        public bool IsManager { get; set; }
        public bool AssigneeeChanged { get; set; }
        public virtual ICollection<Note> RequestNotes { get; set; }
        public virtual AttachmentsModel Attachments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CML_Approvals> CML_Approvals { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CML_Attachments> CML_Attachments { get; set; }
        public virtual CML_BusinessUnit CML_BusinessUnit { get; set; }
        public virtual CML_Disposition CML_Disposition { get; set; }
        public virtual CML_Location CML_Location { get; set; }
        public virtual CML_Priority CML_Priority { get; set; }
        public virtual CML_RequestType CML_RequestType { get; set; }
        public virtual CML_Status CML_Status { get; set; }
        public virtual CML_User CML_User { get; set; }
        public virtual Request Request1 { get; set; }
        public virtual Request Request2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Test> Tests { get; set; }
        public CMLRequest Convert(Request req)
        {
            this.AssignedTo = req.AssignedTo;
            this.BusinessUnitID = req.BusinessUnitID;
            this.CMLNumber = req.CMLNumber;
            this.CML_BusinessUnit = req.CML_BusinessUnit;
            this.CML_Disposition = req.CML_Disposition;
            this.CML_Location = req.CML_Location;
            this.CML_Priority = req.CML_Priority;
            this.CML_RequestType = req.CML_RequestType;
            this.CML_Status = req.CML_Status;
            this.CreatedBy = req.CreatedBy;
            this.CreatedOn = req.CreatedOn;
            this.DateRequired = req.DateRequired;
            this.DeviationNumber = req.DeviationNumber;
            this.DirectorApprovalRequired = req.DirectorApprovalRequired;
            this.DispositionID = req.DispositionID;
            this.EWRNumber = req.EWRNumber;
            this.LocationID = req.LocationID;
            this.Name = req.Name;
            this.Phone = req.Phone;
            this.PriorityID = req.PriorityID;
            this.ProjectNumber = req.ProjectNumber;
            this.RequestedBy = req.RequestedBy;
            this.RequestID = req.RequestID;
            this.RequestTypeID = req.RequestTypeID;
            this.RetentionDate = req.RetentionDate;
            this.StatusID = req.StatusID;
            this.TestObjectives = req.TestObjectives;
            this.Tests = this.Tests;
            this.TestsFinished = req.TestsFinished;
            this.TestsStarted = req.TestsStarted;
            this.UpdatedBy = req.UpdatedBy;
            this.UpdateOn = req.UpdateOn;
            this.TotalSamples = req.TotalSamples;
            if(!string.IsNullOrEmpty(req.WatchList))
                this.WatchList = req.WatchList.Split( ',' );

            this.ClosureNotes = req.ClosureNotes;
            this.CML_Status = new CML_Status
            {   
                ID = req.CML_Status.ID,
                Status = req.CML_Status.Status

            };
            return this;
        }

    }












}
