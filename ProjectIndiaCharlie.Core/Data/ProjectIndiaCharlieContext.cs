﻿using System;
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
        public virtual DbSet<Coordinator> Coordinators { get; set; } = null!;
        public virtual DbSet<Grade> Grades { get; set; } = null!;
        public virtual DbSet<Person> People { get; set; } = null!;
        public virtual DbSet<PersonPassword> PersonPasswords { get; set; } = null!;
        public virtual DbSet<PersonRole> PersonRoles { get; set; } = null!;
        public virtual DbSet<Professor> Professors { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<Schedule> Schedules { get; set; } = null!;
        public virtual DbSet<Section> Sections { get; set; } = null!;
        public virtual DbSet<Student> Students { get; set; } = null!;
        public virtual DbSet<Subject> Subjects { get; set; } = null!;
        public virtual DbSet<SubjectClassroom> SubjectClassrooms { get; set; } = null!;
        public virtual DbSet<SubjectStudent> SubjectStudents { get; set; } = null!;
        public virtual DbSet<VRole> VRoles { get; set; } = null!;
        public virtual DbSet<VStudent> VStudents { get; set; } = null!;
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

                entity.Property(e => e.CoordinatorId).HasColumnName("CoordinatorID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.HasOne(d => d.Coordinator)
                    .WithMany(p => p.Careers)
                    .HasForeignKey(d => d.CoordinatorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Career__Coordina__440B1D61");
            });

            modelBuilder.Entity<Classroom>(entity =>
            {
                entity.ToTable("Classroom", "Academic");

                entity.Property(e => e.ClassroomId).HasColumnName("ClassroomID");

                entity.Property(e => e.Code).HasMaxLength(5);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Coordinator>(entity =>
            {
                entity.HasKey(e => e.PersonId)
                    .HasName("PK__Coordina__AA2FFB85C89921D2");

                entity.ToTable("Coordinator", "Academic");

                entity.Property(e => e.PersonId)
                    .ValueGeneratedNever()
                    .HasColumnName("PersonID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.HasOne(d => d.Person)
                    .WithOne(p => p.Coordinator)
                    .HasForeignKey<Coordinator>(d => d.PersonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Coordinat__Perso__3A81B327");
            });

            modelBuilder.Entity<Grade>(entity =>
            {
                entity.ToTable("Grade", "Academic");

                entity.Property(e => e.GradeId).HasColumnName("GradeID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Grade1)
                    .HasMaxLength(2)
                    .HasColumnName("Grade");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Person>(entity =>
            {
                entity.ToTable("Person", "Person");

                entity.HasIndex(e => e.DocNo, "UQ__Person__3EF1E05762DC3E3E")
                    .IsUnique();

                entity.HasIndex(e => e.Email, "UQ__Person__A9D10534323B41CD")
                    .IsUnique();

                entity.Property(e => e.PersonId).HasColumnName("PersonID");

                entity.Property(e => e.BirthDate).HasColumnType("date");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DocNo).HasMaxLength(13);

                entity.Property(e => e.Email).HasMaxLength(255);

                entity.Property(e => e.FirstName).HasMaxLength(50);

                entity.Property(e => e.FirstSurname).HasMaxLength(50);

                entity.Property(e => e.Gender).HasMaxLength(1);

                entity.Property(e => e.MiddleName).HasMaxLength(50);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.SecondSurname).HasMaxLength(50);
            });

            modelBuilder.Entity<PersonPassword>(entity =>
            {
                entity.HasKey(e => e.PersonId)
                    .HasName("PK__PersonPa__AA2FFB85A68D50A5");

                entity.ToTable("PersonPassword", "Person");

                entity.Property(e => e.PersonId)
                    .ValueGeneratedNever()
                    .HasColumnName("PersonID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.PasswordHash).HasMaxLength(64);

                entity.Property(e => e.PasswordSalt).HasMaxLength(5);

                entity.HasOne(d => d.Person)
                    .WithOne(p => p.PersonPassword)
                    .HasForeignKey<PersonPassword>(d => d.PersonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__PersonPas__Perso__2C3393D0");
            });

            modelBuilder.Entity<PersonRole>(entity =>
            {
                entity.HasKey(e => new { e.PersonId, e.RoleId })
                    .HasName("PK__PersonRo__128057663AC2935E");

                entity.ToTable("PersonRole", "Person");

                entity.Property(e => e.PersonId).HasColumnName("PersonID");

                entity.Property(e => e.RoleId).HasColumnName("RoleID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.HasOne(d => d.Person)
                    .WithMany(p => p.PersonRoles)
                    .HasForeignKey(d => d.PersonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__PersonRol__Perso__34C8D9D1");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.PersonRoles)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__PersonRol__RoleI__35BCFE0A");
            });

            modelBuilder.Entity<Professor>(entity =>
            {
                entity.HasKey(e => e.PersonId)
                    .HasName("PK__Professo__AA2FFB85DD171814");

                entity.ToTable("Professor", "Academic");

                entity.Property(e => e.PersonId)
                    .ValueGeneratedNever()
                    .HasColumnName("PersonID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.HasOne(d => d.Person)
                    .WithOne(p => p.Professor)
                    .HasForeignKey<Professor>(d => d.PersonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Professor__Perso__3F466844");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role", "Person");

                entity.Property(e => e.RoleId).HasColumnName("RoleID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.RoleName).HasMaxLength(64);
            });

            modelBuilder.Entity<Schedule>(entity =>
            {
                entity.ToTable("Schedule", "Academic");

                entity.Property(e => e.ScheduleId).HasColumnName("ScheduleID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.SubjectDetailId).HasColumnName("SubjectDetailID");

                entity.Property(e => e.WeekdayId).HasColumnName("WeekdayID");

                entity.HasOne(d => d.SubjectDetail)
                    .WithMany(p => p.Schedules)
                    .HasForeignKey(d => d.SubjectDetailId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Schedule__Subjec__72C60C4A");

                entity.HasOne(d => d.Weekday)
                    .WithMany(p => p.Schedules)
                    .HasForeignKey(d => d.WeekdayId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Schedule__Weekda__73BA3083");
            });

            modelBuilder.Entity<Section>(entity =>
            {
                entity.HasKey(e => e.SubjectDetailId)
                    .HasName("PK__Section__09EA6B55C5AB7F20");

                entity.ToTable("Section", "Academic");

                entity.Property(e => e.SubjectDetailId).HasColumnName("SubjectDetailID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.ProfessorId).HasColumnName("ProfessorID");

                entity.Property(e => e.Section1).HasColumnName("Section");

                entity.Property(e => e.SubjectId).HasColumnName("SubjectID");

                entity.HasOne(d => d.Professor)
                    .WithMany(p => p.Sections)
                    .HasForeignKey(d => d.ProfessorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Section__Profess__59FA5E80");

                entity.HasOne(d => d.Subject)
                    .WithMany(p => p.Sections)
                    .HasForeignKey(d => d.SubjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Section__Subject__5AEE82B9");
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasKey(e => e.PersonId)
                    .HasName("PK__Student__AA2FFB8540F45CC5");

                entity.ToTable("Student", "Academic");

                entity.Property(e => e.PersonId)
                    .ValueGeneratedNever()
                    .HasColumnName("PersonID");

                entity.Property(e => e.CareerId).HasColumnName("CareerID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.EnrolementDate).HasColumnType("date");

                entity.Property(e => e.GeneralIndex).HasColumnType("decimal(3, 2)");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.TrimestralIndex).HasColumnType("decimal(3, 2)");

                entity.HasOne(d => d.Career)
                    .WithMany(p => p.Students)
                    .HasForeignKey(d => d.CareerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Student__CareerI__4D94879B");

                entity.HasOne(d => d.Person)
                    .WithOne(p => p.Student)
                    .HasForeignKey<Student>(d => d.PersonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Student__PersonI__4CA06362");
            });

            modelBuilder.Entity<Subject>(entity =>
            {
                entity.ToTable("Subject", "Academic");

                entity.Property(e => e.SubjectId).HasColumnName("SubjectID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.SubjectCode).HasMaxLength(4);
            });

            modelBuilder.Entity<SubjectClassroom>(entity =>
            {
                entity.HasKey(e => new { e.SubjectDetailId, e.ClassroomId })
                    .HasName("PK__SubjectC__B8FC73BD9B46C289");

                entity.ToTable("SubjectClassroom", "Academic");

                entity.Property(e => e.SubjectDetailId).HasColumnName("SubjectDetailID");

                entity.Property(e => e.ClassroomId).HasColumnName("ClassroomID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.HasOne(d => d.Classroom)
                    .WithMany(p => p.SubjectClassrooms)
                    .HasForeignKey(d => d.ClassroomId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SubjectCl__Class__6A30C649");

                entity.HasOne(d => d.SubjectDetail)
                    .WithMany(p => p.SubjectClassrooms)
                    .HasForeignKey(d => d.SubjectDetailId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SubjectCl__Subje__693CA210");
            });

            modelBuilder.Entity<SubjectStudent>(entity =>
            {
                entity.HasKey(e => new { e.SubjectDetailId, e.StudentId })
                    .HasName("PK__SubjectS__8AC639F284F283EC");

                entity.ToTable("SubjectStudent", "Academic");

                entity.Property(e => e.SubjectDetailId).HasColumnName("SubjectDetailID");

                entity.Property(e => e.StudentId).HasColumnName("StudentID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.GradeId).HasColumnName("GradeID");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.SubjectStudents)
                    .HasForeignKey(d => d.StudentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SubjectSt__Stude__5FB337D6");

                entity.HasOne(d => d.SubjectDetail)
                    .WithMany(p => p.SubjectStudents)
                    .HasForeignKey(d => d.SubjectDetailId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SubjectSt__Subje__60A75C0F");
            });

            modelBuilder.Entity<VRole>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vRoles", "Person");

                entity.Property(e => e.RoleId).ValueGeneratedOnAdd();

                entity.Property(e => e.RoleName).HasMaxLength(64);
            });

            modelBuilder.Entity<VStudent>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vStudents", "Academic");

                entity.Property(e => e.BirthDate).HasColumnType("date");

                entity.Property(e => e.Career).HasMaxLength(50);

                entity.Property(e => e.Code).HasMaxLength(3);

                entity.Property(e => e.DocNo).HasMaxLength(13);

                entity.Property(e => e.Email).HasMaxLength(255);

                entity.Property(e => e.EnrolementDate).HasColumnType("date");

                entity.Property(e => e.FirstName).HasMaxLength(50);

                entity.Property(e => e.FirstSurname).HasMaxLength(50);

                entity.Property(e => e.Gender).HasMaxLength(1);

                entity.Property(e => e.GeneralIndex).HasColumnType("decimal(3, 2)");

                entity.Property(e => e.MiddleName).HasMaxLength(50);

                entity.Property(e => e.PersonId).HasColumnName("PersonID");

                entity.Property(e => e.SecondSurname).HasMaxLength(50);

                entity.Property(e => e.TrimestralIndex).HasColumnType("decimal(3, 2)");
            });

            modelBuilder.Entity<Weekday>(entity =>
            {
                entity.ToTable("Weekday", "Academic");

                entity.Property(e => e.WeekdayId).HasColumnName("WeekdayID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(9);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
