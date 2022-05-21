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
        public virtual DbSet<PersistedGrant> PersistedGrants { get; set; }
        public virtual DbSet<School> Schools { get; set; }
        public virtual DbSet<Section> Sections { get; set; }
        public virtual DbSet<Student> Students { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("C##_63710_GROUP10")
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
                entity.HasKey(e => e.GuidId)
                    .HasName("COURSE_PK");

                entity.Property(e => e.GuidId)
                    .IsUnicode(false)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.CourseName).IsUnicode(false);

                entity.Property(e => e.CourseNo).HasPrecision(8);

                entity.Property(e => e.CreatedBy)
                    .IsUnicode(false)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.CreatedDate).ValueGeneratedOnAdd();

                entity.Property(e => e.ModifiedBy)
                    .IsUnicode(false)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.ModifiedDate).ValueGeneratedOnAdd();

                entity.Property(e => e.PrereqGuidId).IsUnicode(false);

                entity.Property(e => e.SchoolGuidId).IsUnicode(false);

                entity.HasOne(d => d.PrereqGuid)
                    .WithMany(p => p.InversePrereqGuid)
                    .HasForeignKey(d => d.PrereqGuidId)
                    .HasConstraintName("COURSE_FK2");

                entity.HasOne(d => d.SchoolGuid)
                    .WithMany(p => p.Courses)
                    .HasForeignKey(d => d.SchoolGuidId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("COURSE_FK1");
            });

            modelBuilder.Entity<DeviceCode>(entity =>
            {
                entity.Property(e => e.CreationTime).HasPrecision(7);

                entity.Property(e => e.Expiration).HasPrecision(7);
            });

            modelBuilder.Entity<Enrollment>(entity =>
            {
                entity.HasKey(e => e.GuidId)
                    .HasName("ENROLLMENT_PK");

                entity.Property(e => e.GuidId)
                    .IsUnicode(false)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.CreatedBy)
                    .IsUnicode(false)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.CreatedDate).ValueGeneratedOnAdd();

                entity.Property(e => e.ModifiedBy)
                    .IsUnicode(false)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.ModifiedDate).ValueGeneratedOnAdd();

                entity.Property(e => e.SectionGuidId).IsUnicode(false);

                entity.Property(e => e.StudentGuidId).IsUnicode(false);

                entity.HasOne(d => d.SectionGuid)
                    .WithMany(p => p.Enrollments)
                    .HasForeignKey(d => d.SectionGuidId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ENROLLMENT_FK1");

                entity.HasOne(d => d.StudentGuid)
                    .WithMany(p => p.Enrollments)
                    .HasForeignKey(d => d.StudentGuidId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ENROLLMENT_FK2");
            });

            modelBuilder.Entity<Grade>(entity =>
            {
                entity.HasKey(e => e.GuidId)
                    .HasName("GRADES_PK");

                entity.Property(e => e.GuidId)
                    .IsUnicode(false)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.CreatedBy)
                    .IsUnicode(false)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.CreatedDate).ValueGeneratedOnAdd();

                entity.Property(e => e.EnrollmentGuidId).IsUnicode(false);

                entity.Property(e => e.Grade1).HasPrecision(3);

                entity.Property(e => e.ModifiedBy)
                    .IsUnicode(false)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.ModifiedDate).ValueGeneratedOnAdd();

                entity.HasOne(d => d.Guid)
                    .WithOne(p => p.Grade)
                    .HasForeignKey<Grade>(d => d.GuidId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("GRADES_FK1");
            });

            modelBuilder.Entity<PersistedGrant>(entity =>
            {
                entity.Property(e => e.ConsumedTime).HasPrecision(7);

                entity.Property(e => e.CreationTime).HasPrecision(7);

                entity.Property(e => e.Expiration).HasPrecision(7);
            });

            modelBuilder.Entity<School>(entity =>
            {
                entity.HasKey(e => e.GuidId)
                    .HasName("SCHOOL_PK");

                entity.Property(e => e.GuidId)
                    .IsUnicode(false)
                    .ValueGeneratedOnAdd();

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
                entity.HasKey(e => e.GuidId)
                    .HasName("SECTION_PK");

                entity.Property(e => e.GuidId)
                    .IsUnicode(false)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.CourseGuidId).IsUnicode(false);

                entity.Property(e => e.CreatedBy)
                    .IsUnicode(false)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.CreatedDate).ValueGeneratedOnAdd();

                entity.Property(e => e.ModifiedBy)
                    .IsUnicode(false)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.ModifiedDate).ValueGeneratedOnAdd();

                entity.Property(e => e.SectionNo).HasPrecision(8);

                entity.HasOne(d => d.CourseGuid)
                    .WithMany(p => p.Sections)
                    .HasForeignKey(d => d.CourseGuidId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("SECTION_FK1");
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasKey(e => e.GuidId)
                    .HasName("STUDENTS_PK");

                entity.Property(e => e.GuidId)
                    .IsUnicode(false)
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

                entity.Property(e => e.StudentId).HasPrecision(8);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
