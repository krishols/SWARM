using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace SWARM.EF.Models
{
    [Table("STUDENT")]
    [Index(nameof(StudentId), Name = "STUDENTS_UK1", IsUnique = true)]
    public partial class Student
    {
        public Student()
        {
            Enrollments = new HashSet<Enrollment>();
        }

        [Key]
        [Column("GUID_ID")]
        [StringLength(36)]
        public string GuidId { get; set; }
        [Column("STUDENT_ID")]
        public int StudentId { get; set; }
        [Column("FIRST_NAME")]
        [StringLength(20)]
        public string FirstName { get; set; }
        [Column("LAST_NAME")]
        [StringLength(20)]
        public string LastName { get; set; }
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

        [InverseProperty(nameof(Enrollment.StudentGuid))]
        public virtual ICollection<Enrollment> Enrollments { get; set; }
    }
}
