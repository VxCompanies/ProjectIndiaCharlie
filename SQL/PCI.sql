USE master
GO
ALTER DATABASE ProjectIndiaCharlie
	SET SINGLE_USER
	WITH ROLLBACK IMMEDIATE
GO
DROP DATABASE IF EXISTS ProjectIndiaCharlie
GO
CREATE DATABASE ProjectIndiaCharlie
GO
---- Login
--DROP LOGIN ProjectIndiaCharlieAPI
--GO
--CREATE LOGIN ProjectIndiaCharlieAPI
--WITH Password = 'picAPI',
--DEFAULT_DATABASE = ProjectIndiaCharlie
--GO

USE ProjectIndiaCharlie
GO

CREATE SCHEMA Academic
GO
CREATE SCHEMA Person
GO

-- API Role
CREATE ROLE API
GO
ALTER AUTHORIZATION ON SCHEMA::db_datareader
	TO API
GO
ALTER AUTHORIZATION ON SCHEMA::db_datawriter
	TO API
GO

-- Application User
CREATE USER projectIndiaCharlie
FOR LOGIN projectIndiaCharlieAPI
WITH DEFAULT_SCHEMA = Academic
GO
ALTER ROLE API
ADD MEMBER projectIndiaCharlie
GO

---- Application Role
--CREATE APPLICATION ROLE Academics
--	WITH PASSWORD='p1c4P1',
--	DEFAULT_SCHEMA = Academic
--GO
--ALTER AUTHORIZATION ON SCHEMA::Academic
--TO Academics
--GO
--ALTER AUTHORIZATION ON SCHEMA::Person
--TO Academics
--GO

-- Tables
CREATE TABLE Person.Person(
	PersonID int identity(1110201, 1),
	DocNo nvarchar(13) NOT NULL UNIQUE,
	FirstName nvarchar(50) NOT NULL,
	MiddleName nvarchar(50),
	FirstSurname nvarchar(50) NOT NULL,
	SecondSurname nvarchar(50),
	Gender nvarchar(1) NOT NULL,
	BirthDate date NOT NULL,
	Email nvarchar(255) NOT NULL UNIQUE,
	CreatedDate datetime NOT NULL DEFAULT GETDATE(),
	ModifiedDate datetime NOT NULL DEFAULT GETDATE(),

	PRIMARY KEY(PersonID)
)
GO

CREATE TABLE Person.PersonPassword(
	PersonID int,
	PasswordHash nvarchar(64) NOT NULL,
	PasswordSalt nvarchar(5) NOT NULL,
	CreatedDate datetime NOT NULL DEFAULT GETDATE(),
	ModifiedDate datetime NOT NULL DEFAULT GETDATE(),

	PRIMARY KEY(PersonID),
	FOREIGN KEY(PersonID) REFERENCES Person.Person(PersonID)
)
GO

CREATE TABLE Academic.Professor(
	PersonID int,
	CreatedDate datetime NOT NULL DEFAULT GETDATE(), 
	ModifiedDate datetime NOT NULL DEFAULT GETDATE(),

	PRIMARY KEY(PersonID),
	FOREIGN KEY(PersonID) REFERENCES Person.Person(PersonID)
)
GO

CREATE TABLE Academic.Career(
	CareerID int identity,
	Name nvarchar(50) NOT NULL,
	Code nvarchar(3) NOT NULL,
	Subjects int NOT NULL,
	Credits int NOT NULL,
	Year int NOT NULL,
	IsActive bit NOT NULL,
	CreatedDate datetime NOT NULL DEFAULT GETDATE(), 
	ModifiedDate datetime NOT NULL DEFAULT GETDATE(),

	PRIMARY KEY(CareerID)
)
GO

CREATE TABLE Academic.Student(
	PersonID int,
	CareerID int NOT NULL,
	GeneralIndex decimal(3, 2) NOT NULL DEFAULT 0,
	TrimestralIndex decimal(3, 2) NOT NULL DEFAULT 0,
	Trimester int NOT NULL DEFAULT 1,
	EnrollementDate date NOT NULL DEFAULT GETDATE(),
	CreatedDate datetime NOT NULL DEFAULT GETDATE(), 
	ModifiedDate datetime NOT NULL DEFAULT GETDATE(),

	PRIMARY KEY(PersonID),
	FOREIGN KEY(PersonID) REFERENCES Person.Person(PersonID), 
	FOREIGN KEY(CareerID) REFERENCES Academic.Career(CareerID)
)
GO

CREATE TABLE Academic.Grade(
	GradeID int identity,
	Grade nvarchar(2) NOT NULL,
	Points float NOT NULL,
	CreatedDate datetime NOT NULL DEFAULT GETDATE(), 
	ModifiedDate datetime NOT NULL DEFAULT GETDATE(),

	PRIMARY KEY(GradeID)
)
GO

CREATE TABLE Academic.Subject(
	SubjectID int identity,
	SubjectCode nvarchar(7) NOT NULL,
	Name nvarchar(100) NOT NULL,
	Credits tinyint NOT NULL,
	CreatedDate datetime NOT NULL DEFAULT GETDATE(), 
	ModifiedDate datetime NOT NULL DEFAULT GETDATE(),

	PRIMARY KEY(SubjectID)
)
GO

CREATE TABLE Academic.SubjectDetail(
	SubjectDetailID int identity,
	SubjectID int NOT NULL,
	ProfessorID int NOT NULL,
	Section int NOT NULL,
	Trimester int NOT NULL,
	Year int NOT NULL,
	CreatedDate datetime NOT NULL DEFAULT GETDATE(), 
	ModifiedDate datetime NOT NULL DEFAULT GETDATE(),

	PRIMARY KEY(SubjectDetailID),
	FOREIGN KEY(ProfessorID) REFERENCES Academic.Professor(PersonID),  
	FOREIGN KEY(SubjectID) REFERENCES Academic.Subject(SubjectID)
)
GO

CREATE TABLE Academic.StudentSubject(
	SubjectDetailID int,
	StudentID int,
	GradeID int NULL,
	CreatedDate datetime NOT NULL DEFAULT GETDATE(), 
	ModifiedDate datetime NOT NULL DEFAULT GETDATE(),

	PRIMARY KEY(SubjectDetailID, StudentID),
	FOREIGN KEY(StudentID) REFERENCES Academic.Student(PersonID),  
	FOREIGN KEY(SubjectDetailID) REFERENCES Academic.SubjectDetail(SubjectDetailID)
)
GO

CREATE TABLE Academic.Classroom(
	ClassroomID int identity,
	Code nvarchar(5) NOT NULL,
	IsLab bit NOT NULL,
	Capacity int NOT NULL,
	CreatedDate datetime NOT NULL DEFAULT GETDATE(), 
	ModifiedDate datetime NOT NULL DEFAULT GETDATE(),

	PRIMARY KEY(ClassroomID)
 )
GO

CREATE TABLE Academic.SubjectClassroom(
	SubjectDetailID int,
	ClassroomID int,
	CreatedDate datetime NOT NULL DEFAULT GETDATE(), 
	ModifiedDate datetime NOT NULL DEFAULT GETDATE(),

	FOREIGN KEY(SubjectDetailID) REFERENCES Academic.SubjectDetail(SubjectDetailID),
	FOREIGN KEY(ClassroomID) REFERENCES Academic.Classroom(ClassroomID),
	PRIMARY KEY(SubjectDetailID, ClassroomID)
)
GO

CREATE TABLE Academic.Weekday(
	WeekdayID int identity,
	Name nvarchar(9) NOT NULL,
	CreatedDate datetime NOT NULL DEFAULT GETDATE(), 
	ModifiedDate datetime NOT NULL DEFAULT GETDATE(),

	PRIMARY KEY(WeekdayID)
)
GO

CREATE TABLE Academic.Schedule(
	ScheduleID int identity,
	SubjectDetailID int NOT NULL,
	WeekdayID int NOT NULL,
	StartTime int NOT NULL,
	EndTime int NOT NULL,
	CreatedDate datetime NOT NULL DEFAULT GETDATE(), 
	ModifiedDate datetime NOT NULL DEFAULT GETDATE(),

	PRIMARY KEY(ScheduleID),
	FOREIGN KEY(SubjectDetailID) REFERENCES Academic.SubjectDetail(SubjectDetailID),
	FOREIGN KEY(WeekdayID) REFERENCES Academic.Weekday(WeekdayID)
)
GO

-- Views
CREATE OR ALTER VIEW Academic.vStudentDetails
AS
SELECT pp.PersonID,
	pp.DocNo,
	pp.FirstName,
	pp.MiddleName,
	pp.FirstSurname,
	pp.SecondSurname,
	pp.Gender,
	pp.BirthDate,
	pp.Email, 
    ast.GeneralIndex,
	ast.TrimestralIndex,
	ast.Trimester,
	ast.EnrollementDate,
	ac.Name Career,
	ac.Code,
	ac.Year Pensum
FROM Person.Person pp
	INNER JOIN Academic.Student ast ON pp.PersonID = ast.PersonID
	INNER JOIN Academic.Career ac ON ast.CareerID = ac.CareerID
GO

CREATE OR ALTER VIEW Academic.vProfessorDetails
AS
SELECT pp.PersonID,
	pp.DocNo,
	pp.FirstName,
	pp.MiddleName,
	pp.FirstSurname,
	pp.SecondSurname,
	pp.Gender,
	pp.BirthDate,
	pp.Email
FROM Person.Person pp
	INNER JOIN Academic.Professor ap ON pp.PersonID = ap.PersonID
GO

CREATE OR ALTER VIEW Academic.vPeopleDetails
AS
SELECT pp.PersonID,
	pp.DocNo,
	pp.FirstName,
	pp.MiddleName,
	pp.FirstSurname,
	pp.SecondSurname,
	pp.Gender,
	pp.BirthDate,
	pp.Email
FROM Person.Person pp
	INNER JOIN Academic.Professor ap ON pp.PersonID = ap.PersonID
GO

-- Procedures
CREATE OR ALTER PROCEDURE Person.SP_PasswordUpsert
	@PersonID		int,
	@PasswordHash	nvarchar(64),
	@PasswordSalt	nvarchar(5)
AS  
	IF EXISTS(
		SELECT 1
		FROM Person.PersonPassword pp
		WHERE pp.PersonID = @personID
	)
    BEGIN
        UPDATE Person.PersonPassword
        SET PasswordHash = @passwordHash
        WHERE PersonID = @personID
		RETURN 0
    END

	INSERT INTO Person.PersonPassword(PersonID, PasswordHash, PasswordSalt)
	VALUES(@PersonID, @PasswordHash, @PasswordSalt)
	RETURN 1
GO  

CREATE OR ALTER PROCEDURE Academic.SP_CareerRegistration
	@Name			nvarchar(50),
	@Code			nvarchar(3),
	@Subjects		int,
	@Credits		int,
	@Year			int,
	@IsActive		bit 
AS   
	INSERT INTO Academic.Career(Name, Code, Subjects, Credits, Year, IsActive)
	VALUES(@Name, @Code, @Subjects, @Credits, @Year, @IsActive)
	RETURN 1
GO

CREATE OR ALTER PROCEDURE Person.SP_PersonRegistration
	@DocNo nvarchar(11),
	@FirstName nvarchar(50),
	@MiddleName nvarchar(50) = null,
	@FirstSurname nvarchar(50),
	@SecondSurname nvarchar(50) = null,
	@Gender nvarchar(1),
	@BirthDate date,
	@Email nvarchar(255)
AS
	INSERT INTO Person.Person(DocNo, FirstName, MiddleName, FirstSurname, SecondSurname,
				Gender, BirthDate, Email)
	VALUES(FORMAT(CAST(@DocNo as bigint), '000-0000000-0'), @FirstName, @MiddleName, @FirstSurname, @SecondSurname,
			UPPER(@Gender), @BirthDate, LOWER(@Email))
	RETURN 1
GO

CREATE OR ALTER PROCEDURE Academic.SP_StudentRegistration
	@PersonID int,
	@CareerID int
AS
	INSERT INTO Academic.Student(PersonID, CareerID)
	VALUES(@personId, @CareerId)
	RETURN 1
GO

CREATE OR ALTER PROCEDURE Academic.SP_ProfessorRegistration
	@PersonID int
AS
	INSERT INTO Academic.Professor(PersonID)
	VALUES(@personId)
	RETURN 1
GO

CREATE OR ALTER PROCEDURE Academic.SP_SubjectSelection
	@SubjectDetailID int,
	@StudentID int
AS
	INSERT INTO Academic.StudentSubject(SubjectDetailID, StudentID)
	VALUES(@SubjectDetailID, @StudentID)
	RETURN 1
GO

CREATE OR ALTER PROCEDURE Academic.SP_SubjectClassroomAssignment
	@SubjectDetailID int,
	@ClassroomID int
AS
	INSERT INTO Academic.SubjectClassroom(SubjectDetailID, ClassroomID)
	VALUES(@SubjectDetailID, @ClassroomID)
	RETURN 1
GO

CREATE OR ALTER PROCEDURE Academic.SP_ScheduleAssignment
	@SubjectDetailID int,
	@WeekdayID int,
	@StartTime int,
	@EndTime int
AS
	INSERT INTO Academic.Schedule(SubjectDetailID, WeekdayID, StartTime, EndTime)
	VALUES(@SubjectDetailID, @WeekdayID, @StartTime, @EndTime)
	RETURN 1
GO
DECLARE @gg bit
SELECT Person.F_ScheduleValidation(1, 3, 5)
-- Functions
CREATE OR ALTER FUNCTION Person.F_DocNoValidation(
	@DocNo nvarchar(11)
)
RETURNS BIT
AS
	BEGIN
		IF NOT EXISTS(
			SELECT 1
			FROM Person.Person pp
			WHERE pp.DocNo = FORMAT(CAST(@DocNo as bigint), '000-0000000-0')
		)
			RETURN 0
		RETURN 1
	END
GO

CREATE OR ALTER FUNCTION Academic.F_StudentValidation(
	@PersonID int
)
RETURNS BIT
AS
	BEGIN
		IF NOT EXISTS(
			SELECT 1
			FROM Academic.Student Ast
			WHERE Ast.PersonID = @PersonID
		)
			RETURN 0
		RETURN 1
	END
GO

CREATE OR ALTER FUNCTION Academic.F_ProfessorValidation(
	@PersonID int
)
RETURNS BIT
AS
	BEGIN
		IF NOT EXISTS(
			SELECT 1
			FROM Academic.Professor Apr
			WHERE Apr.PersonID = @PersonID
		)
			RETURN 0
		RETURN 1
	END
GO

CREATE OR ALTER FUNCTION Person.F_PasswordValidation(
	@PersonID int,
	@PasswordHash nvarchar(64)
)
RETURNS BIT
AS
BEGIN
    IF EXISTS(
		SELECT * 
		FROM Person.PersonPassword ppp
		WHERE ppp.PersonID = @PersonID AND
			ppp.PasswordHash = @PasswordHash
	)
		RETURN 1
	RETURN 0
END
GO

CREATE OR ALTER FUNCTION Academic.F_ScheduleValidation(
	@WeekdayID int,
	@StartTime int,
	@EndTime int
)
RETURNS BIT
AS
BEGIN
    IF EXISTS(
		SELECT * 
		FROM Academic.Schedule Asch
		WHERE Asch.WeekdayID = @WeekdayID AND (
			(Asch.StartTime BETWEEN @StartTime AND @EndTime) OR
			(Asch.EndTime BETWEEN @StartTime AND @EndTime)
		)
	)
		RETURN 1
	RETURN 0
END
GO

-- Login
CREATE OR ALTER FUNCTION Person.F_StudentLogin(
	@PersonID int,
	@PasswordHash nvarchar(64)
)
RETURNS TABLE
AS
	RETURN
		SELECT Ast.*
		FROM Academic.Student Ast
		WHERE Ast.PersonID = @PersonID AND
			1 = (SELECT Person.F_PasswordValidation(@PersonID, @PasswordHash))

GO

CREATE OR ALTER FUNCTION Person.F_ProfessorLogin(
	@PersonID int,
	@PasswordHash nvarchar(64)
)
RETURNS TABLE
AS
	RETURN
		SELECT Ap.*
		FROM Academic.Professor Ap
		WHERE Ap.PersonID = @PersonID
GO

-- Triggers
CREATE TRIGGER DateModifiedPersonPerson
    ON Person.Person
    AFTER UPDATE
AS
BEGIN
    UPDATE Person.Person
    SET ModifiedDate = GETDATE()
    WHERE PersonID =(Select PersonID from Inserted)
END 
GO

CREATE TRIGGER PasswordModifiedPersonPerson
    ON Person.PersonPassword
    AFTER UPDATE
AS
BEGIN
    UPDATE Person.PersonPassword 
    SET ModifiedDate = GETDATE()
    WHERE PersonID =(Select PersonID from Inserted)
END 
GO
