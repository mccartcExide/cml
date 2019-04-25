using System.ComponentModel.DataAnnotations;

namespace CML.Controllers
{
    [MetadataType(typeof(PriorityMetaData))]
    public partial  class Priority
    {
    }

    public class PriorityMetaData
    {
        [ScaffoldColumn(false)]
        public int ID;
        [Required]
        public string Priority;
    }
}