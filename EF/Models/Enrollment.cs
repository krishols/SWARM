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
        [Column("STUDENT_ID")]
        public int StudentId { get; set; }
        [Key]
        [Column("SECTION_ID")]
        public int SectionId { get; set; }
        [Key]
        [Column("SCHOOL_ID")]
        public int SchoolId { get; set; }
        [Column("ENROLL_DATE", TypeName = "DATE")]
        public DateTime EnrollDate { get; set; }
        [Column("FINAL_GRADE")]
        public byte? FinalGrade { get; set; }
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

        [ForeignKey("SectionId,SchoolId")]
        [InverseProperty(nameof(Section.Enrollments))]
        public virtual Section S { get; set; }
        [ForeignKey("StudentId,SchoolId")]
        [InverseProperty(nameof(Student.Enrollments))]
        public virtual Student SNavigation { get; set; }
        [ForeignKey(nameof(SchoolId))]
        [InverseProperty("Enrollments")]
        public virtual School School { get; set; }
        [InverseProperty(nameof(Grade.S))]
        public virtual ICollection<Grade> Grades { get; set; }
    }
}
