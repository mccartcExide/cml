using CML.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CML.Utilities
{
    public class Result
    {
        public bool IsApprovalRequired { get; set; }
    }
    public class CalculateSamples
    {
        private int total = 0;
        private int requestid = 0;
        private int sampleid = 0;
        private int testid = 0;

       public Result CheckForApproval( int reqid, int priority )
        {
            Result result = new Result();
            result.IsApprovalRequired = false;
            using(CMLEntities db = new CMLEntities() )
            {
                var req = db.Requests.Find( reqid );

                var tests = req.Tests.Count();
                var samples = req.TotalSamples;

                if ( priority == 3 && ( tests > 3 || samples > 3 ) )
                {
                    result.IsApprovalRequired = true;
                }

                if ( priority == 2 && ( tests > 6 || samples > 6 ) )
                {
                    result.IsApprovalRequired = true;
                }



            }
            return result;
        }
        public CalculateSamples() { }
        public CalculateSamples(int sampleid)
        {
            CMLEntities db = new CMLEntities();
            this.sampleid = sampleid;

            var baseSample = db.Samples.Find( sampleid );
            // var basetest = db.Tests.Find( baseSample.TestID );
            requestid = baseSample.Test.CMLRequest.Value;

            //get base request
            var req = db.Requests.Find( requestid );

            //get tests for request

            var tests = req.Tests;
            var testTotal = 0;
            var samplesTotal = 0;

            foreach(Test currentTest in tests )
            {
                var samples = currentTest.Samples;
               
                foreach(Sample currentSample in samples )
                {
                    samplesTotal = samplesTotal + currentSample.TotalSamples.Value;
                }
                currentTest.TotalSamples = samplesTotal;
                db.SaveChanges();
                testTotal = testTotal + samplesTotal;
            }
            req.TotalSamples = testTotal;

            //do we now need diirector approval????
            req.DirectorApprovalRequired = false;
            if(req.PriorityID == 3 && (tests.Count() > 3 || req.TotalSamples > 3) )
            {
                req.DirectorApprovalRequired = true;
            }

            if(req.PriorityID == 2 && ( tests.Count() > 6 || req.TotalSamples > 6 ) )
            {
                req.DirectorApprovalRequired = true;
            }




            db.Requests.Attach( req );
            db.Entry( req ).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();


        }
    }
}