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
        [StringLength(32)]
        public string GuidId { get; set; }

        [InverseProperty(nameof(Course.SchoolGuid))]
        public virtual ICollection<Course> Courses { get; set; }
    }
}
