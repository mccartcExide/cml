﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class CMLEntities : DbContext
    {
        public CMLEntities()
            : base("name=CMLEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<CML_Attachments> CML_Attachments { get; set; }
        public virtual DbSet<CML_Disposition> CML_Disposition { get; set; }
        public virtual DbSet<CML_Location> CML_Location { get; set; }
        public virtual DbSet<CML_Priority> CML_Priority { get; set; }
        public virtual DbSet<CML_RequestType> CML_RequestType { get; set; }
        public virtual DbSet<CML_Status> CML_Status { get; set; }
        public virtual DbSet<CML_TestDefinition> CML_TestDefinition { get; set; }
        public virtual DbSet<Request> Requests { get; set; }
        public virtual DbSet<Test> Tests { get; set; }
        public virtual DbSet<CML_RoleType> CML_RoleType { get; set; }
        public virtual DbSet<CML_User> CML_User { get; set; }
        public virtual DbSet<Note> Notes { get; set; }
        public virtual DbSet<CML_SampleType> CML_SampleType { get; set; }
        public virtual DbSet<Sample> Samples { get; set; }
        public virtual DbSet<CML_BusinessUnit> CML_BusinessUnit { get; set; }
        public virtual DbSet<CML_Approvals> CML_Approvals { get; set; }
        public virtual DbSet<CML_Roles> CML_Roles { get; set; }
        public virtual DbSet<CML_Settings> CML_Settings { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
    }
}
