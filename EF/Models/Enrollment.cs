using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace SWARM.EF.Models
{
    [Table("ENROLLMENT")]
    public partial class Enrollment
    {
        public Enrollment()
        {
            Grades = new HashSet<Grade>();
        }

        [Key]
        [Column("GUID_ID")]
        [StringLength(36)]
        public string GuidId { get; set; }
        [Required]
        [Column("SECTION_GUID_ID")]
        [StringLength(36)]
        public string SectionGuidId { get; set; }
        [Required]
        [Column("STUDENT_GUID_ID")]
        [StringLength(36)]
        public string StudentGuidId { get; set; }
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

        [ForeignKey(nameof(SectionGuidId))]
        [InverseProperty(nameof(Section.Enrollments))]
        public virtual Section SectionGuid { get; set; }
        [ForeignKey(nameof(StudentGuidId))]
        [InverseProperty(nameof(Student.Enrollments))]
        public virtual Student StudentGuid { get; set; }
        [InverseProperty(nameof(Grade.EnrollmentGuid))]
        public virtual ICollection<Grade> Grades { get; set; }
    }
}
