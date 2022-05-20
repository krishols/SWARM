using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace SWARM.Shared.DTO
{
    public class SectionDTO
    {
        public int SectionNo { get; set; }
        public string GuidId { get; set; }
        public int CourseNo { get; set; }
        public string CourseName { get; set; }
        public string CourseGuidId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
