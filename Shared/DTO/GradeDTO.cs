using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace SWARM.Shared.DTO
{
    public class GradeDTO
    {
       public string GuidId { get; set; }
        public string EnrollmentGuidId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int StudentId { get; set; }
        public int CourseNo { get; set; }
        public int SectionNo { get; set; }
        public int Grade { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
