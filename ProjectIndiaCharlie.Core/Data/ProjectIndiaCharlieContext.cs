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

        public virtual DbSet<Career> Careers { get; set; } = null!;
        public virtual DbSet<Classroom> Classrooms { get; set; } = null!;
        public virtual DbSet<Grade> Grades { get; set; } = null!;
        public virtual DbSet<Person> People { get; set; } = null!;
        public virtual DbSet<PersonPassword> PersonPasswords { get; set; } = null!;
        public virtual DbSet<Professor> Professors { get; set; } = null!;
        public virtual DbSet<Student> Students { get; set; } = null!;
        public virtual DbSet<StudentSubject> StudentSubjects { get; set; } = null!;
        public virtual DbSet<Subject> Subjects { get; set; } = null!;
        public virtual DbSet<SubjectClassroom> SubjectClassrooms { get; set; } = null!;
        public virtual DbSet<SubjectDetail> SubjectDetails { get; set; } = null!;
        public virtual DbSet<SubjectSchedule> SubjectSchedules { get; set; } = null!;
        public virtual DbSet<VPeopleDetail> VPeopleDetails { get; set; } = null!;
        public virtual DbSet<VProfessorDetail> VProfessorDetails { get; set; } = null!;
        public virtual DbSet<VStudentDetail> VStudentDetails { get; set; } = null!;
        public virtual DbSet<VStudentSubject> VStudentSubjects { get; set; } = null!;
        public virtual DbSet<Weekday> Weekdays { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Name=ConnectionStrings:DefaultConnection");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Career>(entity =>
            {
                entity.ToTable("Career", "Academic");

                entity.Property(e => e.CareerId).HasColumnName("CareerID");

                entity.Property(e => e.Code).HasMaxLength(3);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<Classroom>(entity =>
            {
                entity.ToTable("Classroom", "Academic");

                entity.Property(e => e.ClassroomId).HasColumnName("ClassroomID");

                entity.Property(e => e.Code).HasMaxLength(7);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<Grade>(entity =>
            {
                entity.ToTable("Grade", "Academic");

                entity.Property(e => e.GradeId).HasColumnName("GradeID");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Grade1)
                    .HasMaxLength(2)
                    .HasColumnName("Grade");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<Person>(entity =>
            {
                entity.ToTable("Person", "Person");

                entity.HasIndex(e => e.DocNo, "UQ__Person__3EF1E057199BD494")
                    .IsUnique();

                entity.HasIndex(e => e.Email, "UQ__Person__A9D10534E7988C31")
                    .IsUnique();

                entity.Property(e => e.PersonId).HasColumnName("PersonID");

                entity.Property(e => e.BirthDate).HasColumnType("date");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DocNo).HasMaxLength(13);

                entity.Property(e => e.Email).HasMaxLength(255);

                entity.Property(e => e.FirstName).HasMaxLength(50);

                entity.Property(e => e.FirstSurname).HasMaxLength(50);

                entity.Property(e => e.Gender).HasMaxLength(1);

                entity.Property(e => e.MiddleName).HasMaxLength(50);

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.SecondSurname).HasMaxLength(50);
            });

            modelBuilder.Entity<PersonPassword>(entity =>
            {
                entity.HasKey(e => e.PersonId)
                    .HasName("PK__PersonPa__AA2FFB857B7F15E5");

                entity.ToTable("PersonPassword", "Person");

                entity.Property(e => e.PersonId)
                    .ValueGeneratedNever()
                    .HasColumnName("PersonID");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.PasswordHash).HasMaxLength(64);

                entity.Property(e => e.PasswordSalt).HasMaxLength(5);

                entity.HasOne(d => d.Person)
                    .WithOne(p => p.PersonPassword)
                    .HasForeignKey<PersonPassword>(d => d.PersonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__PersonPas__Perso__2C3393D0");
            });

            modelBuilder.Entity<Professor>(entity =>
            {
                entity.HasKey(e => e.PersonId)
                    .HasName("PK__Professo__AA2FFB8584C1EACD");

                entity.ToTable("Professor", "Academic");

                entity.Property(e => e.PersonId)
                    .ValueGeneratedNever()
                    .HasColumnName("PersonID");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Person)
                    .WithOne(p => p.Professor)
                    .HasForeignKey<Professor>(d => d.PersonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Professor__Perso__30F848ED");
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasKey(e => e.PersonId)
                    .HasName("PK__Student__AA2FFB8576156E4F");

                entity.ToTable("Student", "Academic");

                entity.Property(e => e.PersonId)
                    .ValueGeneratedNever()
                    .HasColumnName("PersonID");

                entity.Property(e => e.CareerId).HasColumnName("CareerID");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.EnrollementDate)
                    .HasColumnType("date")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.GeneralIndex).HasColumnType("decimal(3, 2)");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Trimester).HasDefaultValueSql("((1))");

                entity.Property(e => e.TrimestralIndex).HasColumnType("decimal(3, 2)");

                entity.HasOne(d => d.Career)
                    .WithMany(p => p.Students)
                    .HasForeignKey(d => d.CareerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Student__CareerI__3E52440B");

                entity.HasOne(d => d.Person)
                    .WithOne(p => p.Student)
                    .HasForeignKey<Student>(d => d.PersonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Student__PersonI__3D5E1FD2");
            });

            modelBuilder.Entity<StudentSubject>(entity =>
            {
                entity.HasKey(e => new { e.SubjectDetailId, e.StudentId })
                    .HasName("PK__StudentS__8AC639F29493073F");

                entity.ToTable("StudentSubject", "Academic");

                entity.Property(e => e.SubjectDetailId).HasColumnName("SubjectDetailID");

                entity.Property(e => e.StudentId).HasColumnName("StudentID");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.GradeId).HasColumnName("GradeID");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.StudentSubjects)
                    .HasForeignKey(d => d.StudentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__StudentSu__Stude__5070F446");

                entity.HasOne(d => d.SubjectDetail)
                    .WithMany(p => p.StudentSubjects)
                    .HasForeignKey(d => d.SubjectDetailId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__StudentSu__Subje__5165187F");
            });

            modelBuilder.Entity<Subject>(entity =>
            {
                entity.ToTable("Subject", "Academic");

                entity.Property(e => e.SubjectId).HasColumnName("SubjectID");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.SubjectCode).HasMaxLength(7);
            });

            modelBuilder.Entity<SubjectClassroom>(entity =>
            {
                entity.HasKey(e => new { e.SubjectDetailId, e.ClassroomId })
                    .HasName("PK__SubjectC__B8FC73BD94AB7A86");

                entity.ToTable("SubjectClassroom", "Academic");

                entity.Property(e => e.SubjectDetailId).HasColumnName("SubjectDetailID");

                entity.Property(e => e.ClassroomId).HasColumnName("ClassroomID");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Classroom)
                    .WithMany(p => p.SubjectClassrooms)
                    .HasForeignKey(d => d.ClassroomId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SubjectCl__Class__5AEE82B9");

                entity.HasOne(d => d.SubjectDetail)
                    .WithMany(p => p.SubjectClassrooms)
                    .HasForeignKey(d => d.SubjectDetailId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SubjectCl__Subje__59FA5E80");
            });

            modelBuilder.Entity<SubjectDetail>(entity =>
            {
                entity.ToTable("SubjectDetail", "Academic");

                entity.Property(e => e.SubjectDetailId).HasColumnName("SubjectDetailID");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ProfessorId).HasColumnName("ProfessorID");

                entity.Property(e => e.SubjectId).HasColumnName("SubjectID");

                entity.HasOne(d => d.Professor)
                    .WithMany(p => p.SubjectDetails)
                    .HasForeignKey(d => d.ProfessorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SubjectDe__Profe__4AB81AF0");

                entity.HasOne(d => d.Subject)
                    .WithMany(p => p.SubjectDetails)
                    .HasForeignKey(d => d.SubjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SubjectDe__Subje__4BAC3F29");
            });

            modelBuilder.Entity<SubjectSchedule>(entity =>
            {
                entity.ToTable("SubjectSchedule", "Academic");

                entity.Property(e => e.SubjectScheduleId).HasColumnName("SubjectScheduleID");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.SubjectDetailId).HasColumnName("SubjectDetailID");

                entity.Property(e => e.WeekdayId).HasColumnName("WeekdayID");

                entity.HasOne(d => d.SubjectDetail)
                    .WithMany(p => p.SubjectSchedules)
                    .HasForeignKey(d => d.SubjectDetailId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SubjectSc__Subje__6383C8BA");

                entity.HasOne(d => d.Weekday)
                    .WithMany(p => p.SubjectSchedules)
                    .HasForeignKey(d => d.WeekdayId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SubjectSc__Weekd__6477ECF3");
            });

            modelBuilder.Entity<VPeopleDetail>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vPeopleDetails", "Person");

                entity.Property(e => e.BirthDate).HasColumnType("date");

                entity.Property(e => e.DocNo).HasMaxLength(13);

                entity.Property(e => e.Email).HasMaxLength(255);

                entity.Property(e => e.FirstName).HasMaxLength(50);

                entity.Property(e => e.FirstSurname).HasMaxLength(50);

                entity.Property(e => e.Gender).HasMaxLength(1);

                entity.Property(e => e.MiddleName).HasMaxLength(50);

                entity.Property(e => e.PasswordSalt).HasMaxLength(5);

                entity.Property(e => e.PersonId).HasColumnName("PersonID");

                entity.Property(e => e.SecondSurname).HasMaxLength(50);
            });

            modelBuilder.Entity<VProfessorDetail>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vProfessorDetails", "Academic");

                entity.Property(e => e.BirthDate).HasColumnType("date");

                entity.Property(e => e.DocNo).HasMaxLength(13);

                entity.Property(e => e.Email).HasMaxLength(255);

                entity.Property(e => e.FirstName).HasMaxLength(50);

                entity.Property(e => e.FirstSurname).HasMaxLength(50);

                entity.Property(e => e.Gender).HasMaxLength(1);

                entity.Property(e => e.MiddleName).HasMaxLength(50);

                entity.Property(e => e.PasswordSalt).HasMaxLength(5);

                entity.Property(e => e.PersonId).HasColumnName("PersonID");

                entity.Property(e => e.SecondSurname).HasMaxLength(50);
            });

            modelBuilder.Entity<VStudentDetail>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vStudentDetails", "Academic");

                entity.Property(e => e.BirthDate).HasColumnType("date");

                entity.Property(e => e.Career).HasMaxLength(50);

                entity.Property(e => e.Code).HasMaxLength(3);

                entity.Property(e => e.DocNo).HasMaxLength(13);

                entity.Property(e => e.Email).HasMaxLength(255);

                entity.Property(e => e.EnrollementDate).HasColumnType("date");

                entity.Property(e => e.FirstName).HasMaxLength(50);

                entity.Property(e => e.FirstSurname).HasMaxLength(50);

                entity.Property(e => e.Gender).HasMaxLength(1);

                entity.Property(e => e.GeneralIndex).HasColumnType("decimal(3, 2)");

                entity.Property(e => e.MiddleName).HasMaxLength(50);

                entity.Property(e => e.PasswordSalt).HasMaxLength(5);

                entity.Property(e => e.PersonId).HasColumnName("PersonID");

                entity.Property(e => e.SecondSurname).HasMaxLength(50);

                entity.Property(e => e.TrimestralIndex).HasColumnType("decimal(3, 2)");
            });

            modelBuilder.Entity<VStudentSubject>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vStudentSubjects", "Academic");

                entity.Property(e => e.Classroom).HasMaxLength(7);

                entity.Property(e => e.Friday)
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.Grade).HasMaxLength(2);

                entity.Property(e => e.Monday)
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.Professor).HasMaxLength(203);

                entity.Property(e => e.Saturday)
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.StudentId).HasColumnName("StudentID");

                entity.Property(e => e.Subject).HasMaxLength(100);

                entity.Property(e => e.SubjectCode).HasMaxLength(7);

                entity.Property(e => e.Thursday)
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.Tuesday)
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.Wednesday)
                    .HasMaxLength(25)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Weekday>(entity =>
            {
                entity.ToTable("Weekday", "Academic");

                entity.Property(e => e.WeekdayId).HasColumnName("WeekdayID");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name).HasMaxLength(9);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
