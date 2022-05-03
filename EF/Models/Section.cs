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
            GradeTypeWeights = new HashSet<GradeTypeWeight>();
        }

        [Key]
        [Column("SECTION_ID")]
        public int SectionId { get; set; }
        [Key]
        [Column("SCHOOL_ID")]
        public int SchoolId { get; set; }
        [Column("COURSE_NO")]
        public int CourseNo { get; set; }
        [Column("SECTION_NO")]
        public byte SectionNo { get; set; }
        [Column("START_DATE_TIME", TypeName = "DATE")]
        public DateTime? StartDateTime { get; set; }
        [Column("LOCATION")]
        [StringLength(50)]
        public string Location { get; set; }
        [Column("INSTRUCTOR_ID")]
        public int InstructorId { get; set; }
        [Column("CAPACITY")]
        public byte? Capacity { get; set; }
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

        [ForeignKey("CourseNo,SchoolId")]
        [InverseProperty("Sections")]
        public virtual Course Course { get; set; }
        [ForeignKey("SchoolId,InstructorId")]
        [InverseProperty("Sections")]
        public virtual Instructor Instructor { get; set; }
        [ForeignKey(nameof(SchoolId))]
        [InverseProperty("Sections")]
        public virtual School School { get; set; }
        [InverseProperty(nameof(Enrollment.S))]
        public virtual ICollection<Enrollment> Enrollments { get; set; }
        [InverseProperty(nameof(GradeTypeWeight.S))]
        public virtual ICollection<GradeTypeWeight> GradeTypeWeights { get; set; }
    }
}
