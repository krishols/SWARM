﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace SWARM.EF.Models
{
    [Table("COURSE")]
    [Index(nameof(CourseNo), Name = "COURSE_UK1", IsUnique = true)]
    public partial class Course
    {
        public Course()
        {
            InversePrereqGuid = new HashSet<Course>();
            Sections = new HashSet<Section>();
        }

        [Column("COURSE_NO")]
        public int CourseNo { get; set; }
        [Key]
        [Column("GUID_ID")]
        [StringLength(36)]
        public string GuidId { get; set; }
        [Required]
        [Column("COURSE_NAME")]
        [StringLength(20)]
        public string CourseName { get; set; }
        [Column("PREREQ_GUID_ID")]
        [StringLength(36)]
        public string PrereqGuidId { get; set; }
        [Required]
        [Column("SCHOOL_GUID_ID")]
        [StringLength(36)]
        public string SchoolGuidId { get; set; }
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

        [ForeignKey(nameof(PrereqGuidId))]
        [InverseProperty(nameof(Course.InversePrereqGuid))]
        public virtual Course PrereqGuid { get; set; }
        [ForeignKey(nameof(SchoolGuidId))]
        [InverseProperty(nameof(School.Courses))]
        public virtual School SchoolGuid { get; set; }
        [InverseProperty(nameof(Course.PrereqGuid))]
        public virtual ICollection<Course> InversePrereqGuid { get; set; }
        [InverseProperty(nameof(Section.CourseGuid))]
        public virtual ICollection<Section> Sections { get; set; }
    }
}
