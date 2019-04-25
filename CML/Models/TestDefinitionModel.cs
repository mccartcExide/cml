using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CML.Models
{
    public class TestDefinitionModel
    {
        [ScaffoldColumn(false)]
        public Int32 TestDefinitionID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Abbrev { get; set; }
        [Required]
        public string Determines { get; set; }
        [Required]
        public string SampleType { get; set; }
        [Required]
        public string RequiredSampleSize { get; set; }

    }
}