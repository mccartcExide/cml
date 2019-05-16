using CML.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CML.Utilities
{
    public static class EntityExtension
    {
        public static string NextNumber(this CMLEntities db )
        {
            var next = db.CML_Settings.Where( a => a.Name.Equals( "NextNumber" ) ).First();  //db.CMLNextNumbers.Find( 1 );

            int n = next.IntVal.Value + 1;

            next.IntVal = n;
            db.CML_Settings.Attach( next );
            db.Entry( next ).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return n.ToString( "D3" );

        }

        public static CML_User GetLaboratoryManager(this CMLEntities db)
        {
            var labid = db.CML_Settings.Where( a => a.Name.Equals( "LabManager" ) ).First();
            var user = db.CML_User.Find( labid.IntVal.Value );
            return user;
        }

    }
}