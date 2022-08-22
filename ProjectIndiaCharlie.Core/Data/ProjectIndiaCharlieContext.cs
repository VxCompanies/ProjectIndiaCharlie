using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using ProjectIndiaCharlie.Core.Models;

namespace ProjectIndiaCharlie.Core.Data
{
    public partial class ProjectIndiaCharlieContext : DbContext
    {
        public ProjectIndiaCharlieContext()
        {
        }

        public ProjectIndiaCharlieContext(DbContextOptions<ProjectIndiaCharlieContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Building> Buildings { get; set; } = null!;
        public virtual DbSet<Career> Careers { get; set; } = null!;
        public virtual DbSet<Classroom> Classrooms { get; set; } = null!;
        public virtual DbSet<Coordinator> Coordinators { get; set; } = null!;
        public virtual DbSet<Person> People { get; set; } = null!;
        public virtual DbSet<Professor> Professors { get; set; } = null!;
        public virtual DbSet<Schedule> Schedules { get; set; } = null!;
        public virtual DbSet<Student> Students { get; set; } = null!;
        public virtual DbSet<Subject> Subjects { get; set; } = null!;
        public virtual DbSet<SubjectClassroom> SubjectClassrooms { get; set; } = null!;
        public virtual DbSet<SubjectDetail> SubjectDetails { get; set; } = null!;
        public virtual DbSet<SubjectStudent> SubjectStudents { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseSqlServer("Name=ConnectionStrings:DefaultConnection");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Building>(entity =>
            {
                entity.ToTable("Building", "Academic");

                entity.Property(e => e.BuildingId).HasColumnName("BuildingID");

                entity.Property(e => e.Code).HasMaxLength(2);

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<Career>(entity =>
            {
                entity.ToTable("Career", "Academic");

                entity.Property(e => e.CareerId).HasColumnName("CareerID");

                entity.Property(e => e.Code).HasMaxLength(3);

                entity.Property(e => e.CoordinatorId).HasColumnName("CoordinatorID");

                entity.Property(e => e.Credits).HasColumnType("numeric(3, 0)");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Subjects).HasColumnType("numeric(3, 0)");

                entity.Property(e => e.Year).HasMaxLength(4);

                entity.HasOne(d => d.Coordinator)
                    .WithMany(p => p.Careers)
                    .HasForeignKey(d => d.CoordinatorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Career__Coordina__3F466844");
            });

            modelBuilder.Entity<Classroom>(entity =>
            {
                entity.ToTable("Classroom", "Academic");

                entity.HasIndex(e => e.Code, "UQ__Classroo__A25C5AA7B9EDA527")
                    .IsUnique();

                entity.Property(e => e.ClassroomId).HasColumnName("ClassroomID");

                entity.Property(e => e.BuildingId).HasColumnName("BuildingID");

                entity.Property(e => e.Capacity).HasColumnType("numeric(2, 0)");

                entity.Property(e => e.Code).HasMaxLength(6);

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Building)
                    .WithMany(p => p.Classrooms)
                    .HasForeignKey(d => d.BuildingId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Classroom__Build__300424B4");
            });

            modelBuilder.Entity<Coordinator>(entity =>
            {
                entity.HasKey(e => e.PersonId)
                    .HasName("PK__Coordina__AA2FFB85E6E5C371");

                entity.ToTable("Coordinator", "Academic");

                entity.Property(e => e.PersonId)
                    .ValueGeneratedNever()
                    .HasColumnName("PersonID");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.PasswordHash).HasMaxLength(64);

                entity.HasOne(d => d.Person)
                    .WithOne(p => p.Coordinator)
                    .HasForeignKey<Coordinator>(d => d.PersonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Coordinat__Perso__38996AB5");
            });

            modelBuilder.Entity<Person>(entity =>
            {
                entity.ToTable("Person", "Person");

                entity.HasIndex(e => e.DocNo, "UQ__Person__3EF1E057C85F7620")
                    .IsUnique();

                entity.Property(e => e.PersonId).HasColumnName("PersonID");

                entity.Property(e => e.BirthDate).HasColumnType("date");

                entity.Property(e => e.DocNo).HasMaxLength(13);

                entity.Property(e => e.EnrollmentDate)
                    .HasColumnType("date")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.FirstName).HasMaxLength(50);

                entity.Property(e => e.Gender).HasMaxLength(1);

                entity.Property(e => e.LastName).HasMaxLength(50);

                entity.Property(e => e.MiddleName).HasMaxLength(50);

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<Professor>(entity =>
            {
                entity.HasKey(e => e.PersonId)
                    .HasName("PK__Professo__AA2FFB85AD874ACA");

                entity.ToTable("Professor", "Academic");

                entity.Property(e => e.PersonId)
                    .ValueGeneratedNever()
                    .HasColumnName("PersonID");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.PasswordHash).HasMaxLength(64);

                entity.HasOne(d => d.Person)
                    .WithOne(p => p.Professor)
                    .HasForeignKey<Professor>(d => d.PersonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Professor__Perso__4316F928");
            });

            modelBuilder.Entity<Schedule>(entity =>
            {
                entity.ToTable("Schedule", "Academic");

                entity.Property(e => e.ScheduleId).HasColumnName("ScheduleID");

                entity.Property(e => e.EndTime).HasMaxLength(2);

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.StartTime).HasMaxLength(2);

                entity.Property(e => e.SubjectId).HasColumnName("SubjectID");

                entity.Property(e => e.Weekday).HasMaxLength(3);

                entity.HasOne(d => d.Subject)
                    .WithMany(p => p.Schedules)
                    .HasForeignKey(d => d.SubjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Schedule__Subjec__5629CD9C");
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasKey(e => e.PersonId)
                    .HasName("PK__Student__AA2FFB857ABD16B7");

                entity.ToTable("Student", "Academic");

                entity.Property(e => e.PersonId)
                    .ValueGeneratedNever()
                    .HasColumnName("PersonID");

                entity.Property(e => e.CareerId).HasColumnName("CareerID");

                entity.Property(e => e.GeneralGrade).HasColumnType("decimal(3, 2)");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.PasswordHash).HasMaxLength(64);

                entity.Property(e => e.Trimester).HasColumnType("numeric(2, 0)");

                entity.HasOne(d => d.Career)
                    .WithMany(p => p.Students)
                    .HasForeignKey(d => d.CareerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Student__CareerI__49C3F6B7");

                entity.HasOne(d => d.Person)
                    .WithOne(p => p.Student)
                    .HasForeignKey<Student>(d => d.PersonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Student__PersonI__48CFD27E");
            });

            modelBuilder.Entity<Subject>(entity =>
            {
                entity.ToTable("Subject", "Academic");

                entity.HasIndex(e => e.Code, "UQ__Subject__A25C5AA743E170CE")
                    .IsUnique();

                entity.Property(e => e.SubjectId).HasColumnName("SubjectID");

                entity.Property(e => e.Code).HasMaxLength(6);

                entity.Property(e => e.Credits).HasColumnType("numeric(1, 0)");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<SubjectClassroom>(entity =>
            {
                entity.HasKey(e => new { e.SubjectId, e.ClassroomId })
                    .HasName("PK__SubjectC__1D0DBB601D735EEA");

                entity.ToTable("SubjectClassroom", "Academic");

                entity.Property(e => e.SubjectId).HasColumnName("SubjectID");

                entity.Property(e => e.ClassroomId).HasColumnName("ClassroomID");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Classroom)
                    .WithMany(p => p.SubjectClassrooms)
                    .HasForeignKey(d => d.ClassroomId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SubjectCl__Class__5AEE82B9");

                entity.HasOne(d => d.Subject)
                    .WithMany(p => p.SubjectClassrooms)
                    .HasForeignKey(d => d.SubjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SubjectCl__Subje__59FA5E80");
            });

            modelBuilder.Entity<SubjectDetail>(entity =>
            {
                entity.ToTable("SubjectDetail", "Academic");

                entity.Property(e => e.SubjectDetailId).HasColumnName("SubjectDetailID");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ProfessorId).HasColumnName("ProfessorID");

                entity.Property(e => e.Section).HasMaxLength(1);

                entity.Property(e => e.SubjectId).HasColumnName("SubjectID");

                entity.Property(e => e.Trimester).HasMaxLength(2);

                entity.Property(e => e.Year).HasMaxLength(4);

                entity.HasOne(d => d.Professor)
                    .WithMany(p => p.SubjectDetails)
                    .HasForeignKey(d => d.ProfessorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SubjectDe__Profe__4D94879B");

                entity.HasOne(d => d.Subject)
                    .WithMany(p => p.SubjectDetails)
                    .HasForeignKey(d => d.SubjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SubjectDe__Subje__4E88ABD4");
            });

            modelBuilder.Entity<SubjectStudent>(entity =>
            {
                entity.ToTable("SubjectStudent", "Academic");

                entity.Property(e => e.SubjectStudentId).HasColumnName("SubjectStudentID");

                entity.Property(e => e.Grade).HasMaxLength(2);

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.StudentId).HasColumnName("StudentID");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.SubjectStudents)
                    .HasForeignKey(d => d.StudentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SubjectSt__Stude__52593CB8");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
