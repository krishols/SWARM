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
        [Key]
        [Column("GUID_ID")]
        [StringLength(32)]
        public string GuidId { get; set; }
        [Required]
        [Column("FIRST_NAME")]
        [StringLength(20)]
        public string FirstName { get; set; }
        [Required]
        [Column("LAST_NAME")]
        [StringLength(20)]
        public string LastName { get; set; }
        [Required]
        [Column("SECTION_GUID_ID")]
        [StringLength(32)]
        public string SectionGuidId { get; set; }

        [ForeignKey(nameof(SectionGuidId))]
        [InverseProperty(nameof(Section.Enrollments))]
        public virtual Section SectionGuid { get; set; }
    }
}
