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
        [StringLength(32)]
        public string GuidId { get; set; }
        [Required]
        [Column("COURSE_GUID_ID")]
        [StringLength(32)]
        public string CourseGuidId { get; set; }

        [ForeignKey(nameof(CourseGuidId))]
        [InverseProperty(nameof(Course.Sections))]
        public virtual Course CourseGuid { get; set; }
        [InverseProperty(nameof(Enrollment.SectionGuid))]
        public virtual ICollection<Enrollment> Enrollments { get; set; }
    }
}
