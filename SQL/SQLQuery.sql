USE master
GO
DROP DATABASE IF EXISTS ProjectIndiaCharlie
GO
CREATE DATABASE ProjectIndiaCharlie
GO
USE ProjectIndiaCharlie
GO

CREATE SCHEMA Person
GO

CREATE SCHEMA Academic
GO

-- Person
CREATE TABLE Person.Person(
	PersonID int primary key identity(1101200, 1),
	DocNo nvarchar(13) not null unique,
	FirstName nvarchar(50) not null,
	MiddleName nvarchar(50) null,
	LastName nvarchar(50) not null,
	Gender nvarchar(1) not null,
	BirthDate date not null,
	EnrollmentDate date not null default getdate(),
	ModifiedDate datetime not null default getdate()
)
GO

-- Academic
CREATE TABLE Academic.Building(
	BuildingID int primary key identity,
	Name nvarchar(50) not null,
	Code nvarchar(2) not null,
	ModifiedDate datetime not null default getdate()
)
GO

CREATE TABLE Academic.Classroom(
	ClassroomID int primary key identity,
	Code nvarchar(6) not null unique,
	BuildingID int not null,
	IsLab bit not null default 0,
	Capacity numeric(2, 0) not null default 0,
	ModifiedDate datetime not null default getdate(),

	foreign key (BuildingID) references Academic.Building(BuildingID)
)
GO

CREATE TABLE Academic.Subject(
	SubjectID int primary key identity,
	Code nvarchar(6) not null unique,
	Name nvarchar(50) not null,
	Credits numeric(1, 0) not null default 0,
	ModifiedDate datetime not null default getdate()
)
GO

CREATE TABLE Academic.Coordinator(
	PersonID int primary key,
	PasswordHash nvarchar(64) not null,
	ModifiedDate datetime not null default getdate(),

	foreign key (PersonID) references Person.Person(PersonID)
)
GO

CREATE TABLE Academic.Career(
	CareerID int primary key identity,
	CoordinatorID int not null,
	Name nvarchar(50) not null,
	Code nvarchar(3) not null,
	Subjects numeric(3, 0) not null default 0,
	Credits numeric(3, 0) not null default 0,
	Year nvarchar(4) not null,
	IsActive bit not null default 0,
	ModifiedDate datetime not null default getdate(),

	foreign key (CoordinatorID) references Academic.Coordinator(PersonID)
)
GO

CREATE TABLE Academic.Professor(
	PersonID int primary key,
	PasswordHash nvarchar(64) not null,
	ModifiedDate datetime not null default getdate(),

	foreign key (PersonID) references Person.Person(PersonID)
)
GO

CREATE TABLE Academic.Student(
	PersonID int primary key,
	CareerID int not null,
	GeneralGrade decimal(3, 2) not null default 0,
	Trimester numeric(2, 0) not null default 0,
	PasswordHash nvarchar(64) not null,
	ModifiedDate datetime not null default getdate(),

	foreign key (PersonID) references Person.Person(PersonID),
	foreign key (CareerID) references Academic.Career(CareerID)
)
GO

CREATE TABLE Academic.SubjectDetail(
	SubjectDetailID int primary key identity,
	ProfessorID int not null,
	SubjectID int not null,
	Section nvarchar(1) not null,
	Trimester nvarchar(2) not null,
	Year nvarchar(4) not null,
	ModifiedDate datetime not null default getdate(),

	foreign key (ProfessorID) references Academic.Professor(PersonID),
	foreign key (SubjectID) references Academic.Subject(SubjectID)
)
GO

CREATE TABLE Academic.SubjectStudent(
	SubjectStudentID int primary key identity,
	StudentID int not null,
	Grade nvarchar(2) null,
	ModifiedDate datetime not null default getdate(),

	foreign key (StudentID) references Academic.Student(PersonID)
)
GO

CREATE TABLE Academic.Schedule(
	ScheduleID int primary key identity,
	SubjectID int not null,
	Weekday nvarchar(3) not null,
	StartTime nvarchar(2) not null,
	EndTime nvarchar(2) not null,
	ModifiedDate datetime not null default getdate(),

	foreign key (SubjectID) references Academic.SubjectDetail(SubjectDetailID)
)
GO

CREATE TABLE Academic.SubjectClassroom(
	SubjectID int,
	ClassroomID int,
	ModifiedDate datetime not null default getdate(),

	primary key (SubjectID, ClassroomID),
	foreign key (SubjectID) references Academic.Subject(SubjectID),
	foreign key (ClassroomID) references Academic.Classroom(ClassroomID)
)
GO

--Stored Procedures
CREATE PROCEDURE person.spInsertPerson
