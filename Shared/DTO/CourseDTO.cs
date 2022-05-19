using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWARM.Shared.DTO
{
    public class CourseDTO
    {
        public int CourseNo { get; set; }
        public string SchoolName { get; set; }
        public string GuidId { get; set;}
        public string CourseName { get; set; }
        public string? PrereqGuidId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string SchoolGuidId { get; set; }
    }
}
