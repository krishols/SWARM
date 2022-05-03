using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using SWARM.EF.Models;

#nullable disable

namespace SWARM.EF.Data
{
    public partial class SWARMOracleContext : DbContext
    {
        public SWARMOracleContext()
        {
        }

        public SWARMOracleContext(DbContextOptions<SWARMOracleContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserRole> AspNetUserRoles { get; set; }
        public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }
        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<DeviceCode> DeviceCodes { get; set; }
        public virtual DbSet<Enrollment> Enrollments { get; set; }
        public virtual DbSet<Grade> Grades { get; set; }
        public virtual DbSet<GradeConversion> GradeConversions { get; set; }
        public virtual DbSet<GradeType> GradeTypes { get; set; }
        public virtual DbSet<GradeTypeWeight> GradeTypeWeights { get; set; }
        public virtual DbSet<Instructor> Instructors { get; set; }
        public virtual DbSet<PersistedGrant> PersistedGrants { get; set; }
        public virtual DbSet<School> Schools { get; set; }
        public virtual DbSet<Section> Sections { get; set; }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<Zipcode> Zipcodes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("C##_UD_KRIS")
                .HasAnnotation("Relational:Collation", "USING_NLS_COMP");

            modelBuilder.Entity<AspNetRoleClaim>(entity =>
            {
                entity.Property(e => e.Id).HasPrecision(10);
            });

            modelBuilder.Entity<AspNetUser>(entity =>
            {
                entity.Property(e => e.AccessFailedCount).HasPrecision(10);

                entity.Property(e => e.EmailConfirmed).HasPrecision(1);

                entity.Property(e => e.LockoutEnabled).HasPrecision(1);

                entity.Property(e => e.LockoutEnd).HasPrecision(7);

                entity.Property(e => e.PhoneNumberConfirmed).HasPrecision(1);

                entity.Property(e => e.TwoFactorEnabled).HasPrecision(1);
            });

            modelBuilder.Entity<AspNetUserClaim>(entity =>
            {
                entity.Property(e => e.Id).HasPrecision(10);
            });

            modelBuilder.Entity<AspNetUserLogin>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });
            });

            modelBuilder.Entity<AspNetUserRole>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });
            });

            modelBuilder.Entity<AspNetUserToken>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });
            });

            modelBuilder.Entity<Course>(entity =>
            {
                entity.HasKey(e => new { e.CourseNo, e.SchoolId })
                    .HasName("COURSE_PK");

                entity.Property(e => e.CourseNo)
                    .HasPrecision(8)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.SchoolId).HasPrecision(8);

                entity.Property(e => e.CreatedBy)
                    .IsUnicode(false)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.CreatedDate).ValueGeneratedOnAdd();

                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.ModifiedBy)
                    .IsUnicode(false)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.ModifiedDate).ValueGeneratedOnAdd();

                entity.Property(e => e.Prerequisite).HasPrecision(8);

                entity.Property(e => e.PrerequisiteSchoolId).HasPrecision(8);

                entity.HasOne(d => d.School)
                    .WithMany(p => p.Courses)
                    .HasForeignKey(d => d.SchoolId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("COURSE_FK2");

                entity.HasOne(d => d.PrerequisiteNavigation)
                    .WithMany(p => p.InversePrerequisiteNavigation)
                    .HasForeignKey(d => new { d.Prerequisite, d.PrerequisiteSchoolId })
                    .HasConstraintName("COURSE_FK1");
            });

            modelBuilder.Entity<DeviceCode>(entity =>
            {
                entity.Property(e => e.CreationTime).HasPrecision(7);

                entity.Property(e => e.Expiration).HasPrecision(7);
            });

            modelBuilder.Entity<Enrollment>(entity =>
            {
                entity.HasKey(e => new { e.SectionId, e.StudentId, e.SchoolId })
                    .HasName("ENROLLMENT_PK");

                entity.Property(e => e.SectionId).HasPrecision(8);

                entity.Property(e => e.StudentId).HasPrecision(8);

                entity.Property(e => e.SchoolId).HasPrecision(8);

                entity.Property(e => e.CreatedBy)
                    .IsUnicode(false)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.CreatedDate).ValueGeneratedOnAdd();

                entity.Property(e => e.FinalGrade).HasPrecision(3);

                entity.Property(e => e.ModifiedBy)
                    .IsUnicode(false)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.ModifiedDate).ValueGeneratedOnAdd();

                entity.HasOne(d => d.School)
                    .WithMany(p => p.Enrollments)
                    .HasForeignKey(d => d.SchoolId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ENROLLMENT_FK3");

                entity.HasOne(d => d.S)
                    .WithMany(p => p.Enrollments)
                    .HasForeignKey(d => new { d.SectionId, d.SchoolId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ENROLLMENT_FK1");

                entity.HasOne(d => d.SNavigation)
                    .WithMany(p => p.Enrollments)
                    .HasForeignKey(d => new { d.StudentId, d.SchoolId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ENROLLMENT_FK2");
            });

            modelBuilder.Entity<Grade>(entity =>
            {
                entity.HasKey(e => new { e.SchoolId, e.StudentId, e.SectionId, e.GradeTypeCode, e.GradeCodeOccurrence })
                    .HasName("GRADE_PK");

                entity.Property(e => e.SchoolId).HasPrecision(8);

                entity.Property(e => e.StudentId).HasPrecision(8);

                entity.Property(e => e.SectionId).HasPrecision(8);

                entity.Property(e => e.GradeTypeCode)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.GradeCodeOccurrence).HasPrecision(3);

                entity.Property(e => e.CreatedBy)
                    .IsUnicode(false)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.CreatedDate).ValueGeneratedOnAdd();

                entity.Property(e => e.ModifiedBy)
                    .IsUnicode(false)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.ModifiedDate).ValueGeneratedOnAdd();

                entity.HasOne(d => d.School)
                    .WithMany(p => p.Grades)
                    .HasForeignKey(d => d.SchoolId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("GRADE_FK1");

                entity.HasOne(d => d.GradeTypeWeight)
                    .WithMany(p => p.Grades)
                    .HasForeignKey(d => new { d.SchoolId, d.SectionId, d.GradeTypeCode })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("GRADE_FK3");

                entity.HasOne(d => d.S)
                    .WithMany(p => p.Grades)
                    .HasForeignKey(d => new { d.SectionId, d.StudentId, d.SchoolId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("GRADE_FK2");
            });

            modelBuilder.Entity<GradeConversion>(entity =>
            {
                entity.HasKey(e => new { e.SchoolId, e.LetterGrade })
                    .HasName("GRADE_CONVERSION_PK");

                entity.Property(e => e.SchoolId).HasPrecision(8);

                entity.Property(e => e.LetterGrade).IsUnicode(false);

                entity.Property(e => e.CreatedBy)
                    .IsUnicode(false)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.CreatedDate).ValueGeneratedOnAdd();

                entity.Property(e => e.MaxGrade).HasPrecision(3);

                entity.Property(e => e.MinGrade).HasPrecision(3);

                entity.Property(e => e.ModifiedBy)
                    .IsUnicode(false)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.ModifiedDate).ValueGeneratedOnAdd();

                entity.HasOne(d => d.School)
                    .WithMany(p => p.GradeConversions)
                    .HasForeignKey(d => d.SchoolId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("GRADE_CONVERSION_FK1");
            });

            modelBuilder.Entity<GradeType>(entity =>
            {
                entity.HasKey(e => new { e.SchoolId, e.GradeTypeCode })
                    .HasName("GRADE_TYPE_PK");

                entity.Property(e => e.SchoolId).HasPrecision(8);

                entity.Property(e => e.GradeTypeCode)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.CreatedBy)
                    .IsUnicode(false)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.CreatedDate).ValueGeneratedOnAdd();

                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.ModifiedBy)
                    .IsUnicode(false)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.ModifiedDate).ValueGeneratedOnAdd();

                entity.HasOne(d => d.School)
                    .WithMany(p => p.GradeTypes)
                    .HasForeignKey(d => d.SchoolId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("GRADE_TYPE_FK1");
            });

            modelBuilder.Entity<GradeTypeWeight>(entity =>
            {
                entity.HasKey(e => new { e.SchoolId, e.SectionId, e.GradeTypeCode })
                    .HasName("GRADE_TYPE_WEIGHT_PK");

                entity.Property(e => e.SchoolId).HasPrecision(8);

                entity.Property(e => e.SectionId).HasPrecision(8);

                entity.Property(e => e.GradeTypeCode)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.CreatedBy)
                    .IsUnicode(false)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.CreatedDate).ValueGeneratedOnAdd();

                entity.Property(e => e.DropLowest).HasPrecision(1);

                entity.Property(e => e.ModifiedBy)
                    .IsUnicode(false)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.ModifiedDate).ValueGeneratedOnAdd();

                entity.Property(e => e.NumberPerSection).HasPrecision(3);

                entity.Property(e => e.PercentOfFinalGrade).HasPrecision(3);

                entity.HasOne(d => d.School)
                    .WithMany(p => p.GradeTypeWeights)
                    .HasForeignKey(d => d.SchoolId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("GRADE_TYPE_WEIGHT_FK1");

                entity.HasOne(d => d.GradeType)
                    .WithMany(p => p.GradeTypeWeights)
                    .HasForeignKey(d => new { d.SchoolId, d.GradeTypeCode })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("GRADE_TYPE_WEIGHT_FK2");

                entity.HasOne(d => d.S)
                    .WithMany(p => p.GradeTypeWeights)
                    .HasForeignKey(d => new { d.SectionId, d.SchoolId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("GRADE_TYPE_WEIGHT_FK3");
            });

            modelBuilder.Entity<Instructor>(entity =>
            {
                entity.HasKey(e => new { e.SchoolId, e.InstructorId })
                    .HasName("INSTRUCTOR_PK");

                entity.Property(e => e.SchoolId).HasPrecision(8);

                entity.Property(e => e.InstructorId)
                    .HasPrecision(8)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.CreatedBy)
                    .IsUnicode(false)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.CreatedDate).ValueGeneratedOnAdd();

                entity.Property(e => e.FirstName).IsUnicode(false);

                entity.Property(e => e.LastName).IsUnicode(false);

                entity.Property(e => e.ModifiedBy)
                    .IsUnicode(false)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.ModifiedDate).ValueGeneratedOnAdd();

                entity.Property(e => e.Phone).IsUnicode(false);

                entity.Property(e => e.Salutation).IsUnicode(false);

                entity.Property(e => e.StreetAddress).IsUnicode(false);

                entity.Property(e => e.Zip).IsUnicode(false);

                entity.HasOne(d => d.School)
                    .WithMany(p => p.Instructors)
                    .HasForeignKey(d => d.SchoolId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("INSTRUCTOR_FK1");

                entity.HasOne(d => d.ZipNavigation)
                    .WithMany(p => p.Instructors)
                    .HasForeignKey(d => d.Zip)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("INSTRUCTOR_FK2");
            });

            modelBuilder.Entity<PersistedGrant>(entity =>
            {
                entity.Property(e => e.ConsumedTime).HasPrecision(7);

                entity.Property(e => e.CreationTime).HasPrecision(7);

                entity.Property(e => e.Expiration).HasPrecision(7);
            });

            modelBuilder.Entity<School>(entity =>
            {
                entity.Property(e => e.SchoolId)
                    .HasPrecision(8)
                    .ValueGeneratedNever();

                entity.Property(e => e.CreatedBy)
                    .IsUnicode(false)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.CreatedDate).ValueGeneratedOnAdd();

                entity.Property(e => e.ModifiedBy)
                    .IsUnicode(false)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.ModifiedDate).ValueGeneratedOnAdd();

                entity.Property(e => e.SchoolName).IsUnicode(false);
            });

            modelBuilder.Entity<Section>(entity =>
            {
                entity.HasKey(e => new { e.SectionId, e.SchoolId })
                    .HasName("SECTION_PK");

                entity.Property(e => e.SectionId)
                    .HasPrecision(8)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.SchoolId).HasPrecision(8);

                entity.Property(e => e.Capacity).HasPrecision(3);

                entity.Property(e => e.CourseNo).HasPrecision(8);

                entity.Property(e => e.CreatedBy)
                    .IsUnicode(false)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.CreatedDate).ValueGeneratedOnAdd();

                entity.Property(e => e.InstructorId).HasPrecision(8);

                entity.Property(e => e.Location).IsUnicode(false);

                entity.Property(e => e.ModifiedBy)
                    .IsUnicode(false)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.ModifiedDate).ValueGeneratedOnAdd();

                entity.Property(e => e.SectionNo).HasPrecision(3);

                entity.HasOne(d => d.School)
                    .WithMany(p => p.Sections)
                    .HasForeignKey(d => d.SchoolId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("SECTION_FK2");

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.Sections)
                    .HasForeignKey(d => new { d.CourseNo, d.SchoolId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("SECTION_FK1");

                entity.HasOne(d => d.Instructor)
                    .WithMany(p => p.Sections)
                    .HasForeignKey(d => new { d.SchoolId, d.InstructorId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("SECTION_FK3");
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasKey(e => new { e.StudentId, e.SchoolId })
                    .HasName("STUDENT_PK");

                entity.Property(e => e.StudentId)
                    .HasPrecision(8)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.SchoolId).HasPrecision(8);

                entity.Property(e => e.CreatedBy)
                    .IsUnicode(false)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.CreatedDate).ValueGeneratedOnAdd();

                entity.Property(e => e.Employer).IsUnicode(false);

                entity.Property(e => e.FirstName).IsUnicode(false);

                entity.Property(e => e.LastName).IsUnicode(false);

                entity.Property(e => e.ModifiedBy)
                    .IsUnicode(false)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.ModifiedDate).ValueGeneratedOnAdd();

                entity.Property(e => e.Phone).IsUnicode(false);

                entity.Property(e => e.Salutation).IsUnicode(false);

                entity.Property(e => e.StreetAddress).IsUnicode(false);

                entity.Property(e => e.Zip).IsUnicode(false);

                entity.HasOne(d => d.School)
                    .WithMany(p => p.Students)
                    .HasForeignKey(d => d.SchoolId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("STUDENT_FK1");
            });

            modelBuilder.Entity<Zipcode>(entity =>
            {
                entity.HasKey(e => e.Zip)
                    .HasName("ZIP_PK");

                entity.Property(e => e.Zip).IsUnicode(false);

                entity.Property(e => e.City).IsUnicode(false);

                entity.Property(e => e.CreatedBy)
                    .IsUnicode(false)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.CreatedDate).ValueGeneratedOnAdd();

                entity.Property(e => e.ModifiedBy)
                    .IsUnicode(false)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.ModifiedDate).ValueGeneratedOnAdd();

                entity.Property(e => e.State)
                    .IsUnicode(false)
                    .IsFixedLength(true);
            });

            modelBuilder.HasSequence("COURSE_SEQ");

            modelBuilder.HasSequence("INSTRUCTOR_SEQ");

            modelBuilder.HasSequence("SECTION_SEQ");

            modelBuilder.HasSequence("STUDENT_SEQ");

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}