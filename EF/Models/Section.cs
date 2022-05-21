using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace SWARM.EF.Models
{
    [Table("SECTION")]
    public partial class Section
    {
        public Section()
        {
            Enrollments = new HashSet<Enrollment>();
        }

        [Column("SECTION_NO")]
        public int SectionNo { get; set; }
        [Key]
        [Column("GUID_ID")]
        [StringLength(36)]
        public string GuidId { get; set; }
        [Required]
        [Column("COURSE_GUID_ID")]
        [StringLength(36)]
        public string CourseGuidId { get; set; }
        [Required]
        [Column("CREATED_BY")]
        [StringLength(30)]
        public string CreatedBy { get; set; }
        [Column("CREATED_DATE", TypeName = "DATE")]
        public DateTime CreatedDate { get; set; }
        [Required]
        [Column("MODIFIED_BY")]
        [StringLength(30)]
        public string ModifiedBy { get; set; }
        [Column("MODIFIED_DATE", TypeName = "DATE")]
        public DateTime ModifiedDate { get; set; }

        [ForeignKey(nameof(CourseGuidId))]
        [InverseProperty(nameof(Course.Sections))]
        public virtual Course CourseGuid { get; set; }
        [InverseProperty(nameof(Enrollment.SectionGuid))]
        public virtual ICollection<Enrollment> Enrollments { get; set; }
    }
}
