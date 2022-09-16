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

        public virtual DbSet<VPeopleDetail> VPeopleDetails { get; set; } = null!;
        public virtual DbSet<VProfessorDetail> VProfessorDetails { get; set; } = null!;
        public virtual DbSet<VStudentDetail> VStudentDetails { get; set; } = null!;
        public virtual DbSet<VStudentSubject> VStudentSubjects { get; set; } = null!;
        public virtual DbSet<VSubjectSchedule> VSubjectSchedules { get; set; } = null!;
        public virtual DbSet<VSubjectSectionDetail> VSubjectSectionDetails { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Name=ConnectionStrings:DefaultConnection");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("Academic");

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

                entity.Property(e => e.PersonId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("PersonID");

                entity.Property(e => e.SecondSurname).HasMaxLength(50);
            });

            modelBuilder.Entity<VProfessorDetail>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vProfessorDetails");

                entity.Property(e => e.BirthDate).HasColumnType("date");

                entity.Property(e => e.DocNo).HasMaxLength(13);

                entity.Property(e => e.Email).HasMaxLength(255);

                entity.Property(e => e.FirstName).HasMaxLength(50);

                entity.Property(e => e.FirstSurname).HasMaxLength(50);

                entity.Property(e => e.Gender).HasMaxLength(1);

                entity.Property(e => e.MiddleName).HasMaxLength(50);

                entity.Property(e => e.PersonId).HasColumnName("PersonID");

                entity.Property(e => e.SecondSurname).HasMaxLength(50);
            });

            modelBuilder.Entity<VStudentDetail>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vStudentDetails");

                entity.Property(e => e.BirthDate).HasColumnType("date");

                entity.Property(e => e.Career).HasMaxLength(50);

                entity.Property(e => e.CareerCode).HasMaxLength(3);

                entity.Property(e => e.DocNo).HasMaxLength(13);

                entity.Property(e => e.Email).HasMaxLength(255);

                entity.Property(e => e.EnrollementDate).HasColumnType("date");

                entity.Property(e => e.FirstName).HasMaxLength(50);

                entity.Property(e => e.FirstSurname).HasMaxLength(50);

                entity.Property(e => e.Gender).HasMaxLength(1);

                entity.Property(e => e.GeneralIndex).HasColumnType("decimal(3, 2)");

                entity.Property(e => e.MiddleName).HasMaxLength(50);

                entity.Property(e => e.PersonId).HasColumnName("PersonID");

                entity.Property(e => e.SecondSurname).HasMaxLength(50);

                entity.Property(e => e.TrimestralIndex).HasColumnType("decimal(3, 2)");
            });

            modelBuilder.Entity<VStudentSubject>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vStudentSubjects");

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

                entity.Property(e => e.SubjectDetailId).HasColumnName("SubjectDetailID");

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

            modelBuilder.Entity<VSubjectSchedule>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vSubjectSchedule");

                entity.Property(e => e.SubjectDetailId).HasColumnName("SubjectDetailID");

                entity.Property(e => e.WeekdayId).HasColumnName("WeekdayID");
            });

            modelBuilder.Entity<VSubjectSectionDetail>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vSubjectSectionDetails");

                entity.Property(e => e.Capacity)
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.ClassroomCode).HasMaxLength(7);

                entity.Property(e => e.Friday)
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.Monday)
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.Professor).HasMaxLength(203);

                entity.Property(e => e.Saturday)
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.SubjectCode).HasMaxLength(7);

                entity.Property(e => e.SubjectDetailId).HasColumnName("SubjectDetailID");

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

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
