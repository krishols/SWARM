using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace SWARM.EF.Models
{
    [Table("SCHOOL")]
    public partial class School
    {
        public School()
        {
            Courses = new HashSet<Course>();
        }

        [Required]
        [Column("SCHOOL_NAME")]
        [StringLength(20)]
        public string SchoolName { get; set; }
        [Key]
        [Column("GUID_ID")]
        [StringLength(36)]
        public string GuidId { get; set; }
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

        [InverseProperty(nameof(Course.SchoolGuid))]
        public virtual ICollection<Course> Courses { get; set; }
    }
}
