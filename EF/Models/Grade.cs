using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace SWARM.EF.Models
{
    [Table("GRADE")]
    public partial class Grade
    {
        [Key]
        [Column("GUID_ID")]
        [StringLength(36)]
        public string GuidId { get; set; }
        [Required]
        [Column("ENROLLMENT_GUID_ID")]
        [StringLength(20)]
        public string EnrollmentGuidId { get; set; }
        [Column("GRADE")]
        public byte Grade1 { get; set; }
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

        [ForeignKey(nameof(GuidId))]
        [InverseProperty(nameof(Enrollment.Grade))]
        public virtual Enrollment Guid { get; set; }
    }
}
