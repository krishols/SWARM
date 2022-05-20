using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace SWARM.Shared.DTO
{
    public class EnrollmentDTO
    {
        public string GuidId { get; set; }
        public string SectionGuidId { get; set; }
        public int SectionNo { get; set; }
        public int CourseNo { get; set; }
        public string StudentGuidId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }

    }
}
