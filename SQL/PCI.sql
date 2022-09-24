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

USE ProjectIndiaCharlie
GO

CREATE SCHEMA Academic
GO
CREATE SCHEMA Person
GO

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
	Points decimal(8, 4) NOT NULL,
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
	Code nvarchar(7) NOT NULL,
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

CREATE TABLE Academic.SubjectSchedule(
	SubjectScheduleID int identity,
	SubjectDetailID int NOT NULL,
	WeekdayID int NOT NULL,
	StartTime int NOT NULL,
	EndTime int NOT NULL,
	CreatedDate datetime NOT NULL DEFAULT GETDATE(), 
	ModifiedDate datetime NOT NULL DEFAULT GETDATE(),

	PRIMARY KEY(SubjectScheduleID),
	FOREIGN KEY(SubjectDetailID) REFERENCES Academic.SubjectDetail(SubjectDetailID),
	FOREIGN KEY(WeekdayID) REFERENCES Academic.Weekday(WeekdayID)
)
GO

CREATE TABLE Academic.Administrator(
	PersonID int,
	CreatedDate datetime NOT NULL DEFAULT GETDATE(), 
	ModifiedDate datetime NOT NULL DEFAULT GETDATE(),

	PRIMARY KEY(PersonID),
	FOREIGN KEY(PersonID) REFERENCES Person.Person(PersonID)
)
GO

CREATE TABLE Academic.GradeRevision(
    PersonID int NOT NULL,
    SubjectDetailID int NOT NULL,
    DateRequested datetime NOT NULL DEFAULT GETDATE(), 
    GradeID int NOT NULL,
    ModifiedGradeID int,
    AdminID int,
    ProfessorID int,
    DateModified datetime,

    PRIMARY KEY(SubjectDetailID, PersonID),
    FOREIGN KEY(SubjectDetailID) REFERENCES Academic.SubjectDetail(SubjectDetailID),
    FOREIGN KEY(GradeID) REFERENCES Academic.Grade(GradeID),
    FOREIGN KEY(ModifiedGradeID) REFERENCES Academic.Grade(GradeID),
    FOREIGN KEY(AdminID) REFERENCES Academic.Administrator(PersonID),
    FOREIGN KEY(ProfessorID) REFERENCES Academic.Professor(PersonID),

    FOREIGN KEY(PersonID) REFERENCES Academic.Student(PersonID)
)
GO


CREATE TABLE Academic.IndexHistory(
IndexHistoryID int PRIMARY KEY IDENTITY,
PersonID int,
CareerID int,
Year int,
Trimester int,
CreditsTrimester int,
CreditsSumm int,
PointsTrimester float,
PontsSumm float,
TrimesteralIndex float,
GeneralIndex float,
ModifiedDate datetime DEFAULT GETDATE,

FOREIGN KEY(PersonID) REFERENCES Person.Person(PersonID),
FOREIGN KEY(CareerID) REFERENCES Academic.Career(CareerID)
);
GO


-- Views
CREATE OR ALTER VIEW Academic.vGradeRevision
AS
SELECT Agr.PersonID,
	CONCAT(Pper.FirstName, IIF(Pper.MiddleName IS NULL, '', ' '), Pper.MiddleName, ' ', Pper.FirstSurname, IIF(Pper.SecondSurname IS NULL, '', ' '), Pper.SecondSurname) Student,
	Agr.SubjectDetailID,
	CONCAT(Asub.Name,'-', AsubDet.Section) Section,
	Agr.DateRequested,
	Agr.GradeID,
	Ag.Grade,
	Agr.ModifiedGradeID,
	Agm.Grade ModifiedGrade,
	Aad.PersonID AdminId,
	IIF(Padm.PersonID IS NULL, NULL, CONCAT(Padm.FirstName, IIF(Padm.MiddleName IS NULL, '', ' '), Padm.MiddleName, ' ', Padm.FirstSurname, IIF(Padm.SecondSurname IS NULL, '', ' '), Padm.SecondSurname)) Admin,
	Pprof.PersonID ProfessorId,
	IIF(Pprof.PersonID IS NULL, NULL, CONCAT(Pprof.FirstName, IIF(Pprof.MiddleName IS NULL, '', ' '), Pprof.MiddleName, ' ', Pprof.FirstSurname, IIF(Pprof.SecondSurname IS NULL, '', ' '), Pprof.SecondSurname)) Professor
FROM Academic.GradeRevision Agr
	INNER JOIN Academic.Student Ast ON Ast.PersonID = Agr.PersonID
	INNER JOIN Person.Person Pper ON Pper.PersonID = Ast.PersonID
	INNER JOIN Academic.SubjectDetail AsubDet ON AsubDet.SubjectDetailID = Agr.SubjectDetailID
	INNER JOIN Academic.Subject Asub ON Asub.SubjectID = AsubDet.SubjectID
	INNER JOIN Academic.Grade Ag ON Ag.GradeID = Agr.GradeID
	LEFT JOIN Academic.Grade Agm ON Agm.GradeID = Agr.ModifiedGradeID
	LEFT JOIN Academic.Administrator Aad ON Aad.PersonID = Agr.AdminID
	LEFT JOIN Person.Person Padm ON Padm.PersonID = Aad.PersonID
	LEFT JOIN Academic.Professor Aprof ON Aprof.PersonID = Agr.AdminID
	LEFT JOIN Person.Person Pprof ON Pprof.PersonID = Aprof.PersonID
GO

CREATE OR ALTER VIEW Person.vPeopleDetails
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
FROM Person.Person Pp
GO

CREATE OR ALTER VIEW Academic.vStudentDetails
AS
SELECT Pvpd.*,
    ast.GeneralIndex,
	ast.TrimestralIndex,
	ast.Trimester,
	ast.EnrollementDate,
	ac.Name Career,
	ac.Code CareerCode,
	ac.Year Pensum
FROM Person.vPeopleDetails Pvpd
	INNER JOIN Academic.Student ast ON Pvpd.PersonID = ast.PersonID
	INNER JOIN Academic.Career ac ON ast.CareerID = ac.CareerID
GO

CREATE OR ALTER VIEW Academic.vGrades
AS
	SELECT GradeID, Grade
	FROM Academic.Grade 
GO

CREATE OR ALTER VIEW Academic.vProfessorDetails
AS
SELECT Pvpd.*
FROM Person.vPeopleDetails Pvpd
	INNER JOIN Academic.Professor ap ON Pvpd.PersonID = ap.PersonID
GO

CREATE OR ALTER VIEW Academic.vSubjectSectionDetails
AS
SELECT Asdet.SubjectDetailID,
	Asub.SubjectCode,
	Asub.Name,
	Asub.Credits,
	Asdet.Section,
	Asdet.Year,
	Asdet.Trimester,
	Avprof.PersonID ProfessorID,
	CONCAT(Avprof.FirstName, IIF(Avprof.MiddleName IS NULL, '', ' '), Avprof.MiddleName, ' ', Avprof.FirstSurname, IIF(Avprof.SecondSurname IS NULL, '', ' '), Avprof.SecondSurname) Professor,
	CONCAT(COUNT(AstuSub.StudentID), '/', Acl.Capacity) Capacity,
	Acl.Code ClassroomCode,
	MAX(CASE WHEN AW.WeekdayID = 1 AND AsubSch.StartTime IS NOT NULL THEN CONCAT(AsubSch.StartTime, '/', AsubSch.EndTime) END) Monday,
	MAX(CASE WHEN AW.WeekdayID = 2 AND AsubSch.StartTime IS NOT NULL THEN CONCAT(AsubSch.StartTime, '/', AsubSch.EndTime) END) Tuesday,
	MAX(CASE WHEN AW.WeekdayID = 3 AND AsubSch.StartTime IS NOT NULL THEN CONCAT(AsubSch.StartTime, '/', AsubSch.EndTime) END) Wednesday,
	MAX(CASE WHEN AW.WeekdayID = 4 AND AsubSch.StartTime IS NOT NULL THEN CONCAT(AsubSch.StartTime, '/', AsubSch.EndTime) END) Thursday,
	MAX(CASE WHEN AW.WeekdayID = 5 AND AsubSch.StartTime IS NOT NULL THEN CONCAT(AsubSch.StartTime, '/', AsubSch.EndTime) END) Friday,
	MAX(CASE WHEN AW.WeekdayID = 6 AND AsubSch.StartTime IS NOT NULL THEN CONCAT(AsubSch.StartTime, '/', AsubSch.EndTime) END) Saturday
FROM Academic.Subject Asub
	INNER JOIN Academic.SubjectDetail Asdet ON Asdet.SubjectID = Asub.SubjectID
	INNER JOIN Academic.vProfessorDetails Avprof ON Avprof.PersonID = Asdet.ProfessorID
	INNER JOIN Academic.SubjectSchedule AsubSch ON AsubSch.SubjectDetailID = Asdet.SubjectDetailID
	INNER JOIN Academic.Weekday Aw ON Aw.WeekdayID = AsubSch.WeekdayID
	INNER JOIN Academic.SubjectClassroom Ascl ON Ascl.SubjectDetailID = Asdet.SubjectDetailID
	INNER JOIN Academic.Classroom Acl ON Acl.ClassroomID = Ascl.ClassroomID
	LEFT JOIN Academic.StudentSubject AstuSub ON AstuSub.SubjectDetailID = Asdet.SubjectDetailID
	GROUP BY Asdet.SubjectDetailID,
	Asub.SubjectCode,
	Asub.Name,
	CONCAT(Avprof.FirstName, IIF(Avprof.MiddleName IS NULL, '', ' '), Avprof.MiddleName, ' ', Avprof.FirstSurname, IIF(Avprof.SecondSurname IS NULL, '', ' '), Avprof.SecondSurname),
	Asub.Credits,
	Asdet.Section,
	Asdet.Year,
	Asdet.Trimester,
	Avprof.PersonID,
	Acl.Capacity,
	Acl.Code
GO

CREATE OR ALTER VIEW Academic.vAdministratorDetails
AS
SELECT Pvpd.*
FROM Person.vPeopleDetails Pvpd
	INNER JOIN Academic.Administrator ad ON Pvpd.PersonID = ad.PersonID
GO

CREATE OR ALTER VIEW Academic.vAvailableCareers
AS
	SELECT Ac.CareerID,
		Ac.Code,
		Ac.Name,
		Ac.Credits,
		Ac.Year
	FROM Academic.Career Ac
	WHERE Ac.IsActive = 1
GO

CREATE OR ALTER VIEW Academic.vSubjectSchedule
AS
	SELECT AsubSche.SubjectDetailID,
		AsubSche.WeekdayID,
		AsubSche.StartTime,
		AsubSche.EndTime
	FROM Academic.SubjectSchedule AsubSche
GO

CREATE OR ALTER VIEW Academic.vStudentSubjects
AS
SELECT Ass.StudentID,
	Asd.SubjectDetailID,
	Asu.SubjectCode,
	Asd.Section,
	Asu.Name Subject,
	CONCAT(Pprof.FirstName, IIF(Pprof.MiddleName IS NULL, '', ' '), Pprof.MiddleName, ' ', Pprof.FirstSurname, IIF(Pprof.SecondSurname IS NULL, '', ' '), Pprof.SecondSurname) Professor,
	Asu.Credits,
	Ac.Code ClassroomCode,
	Asd.Trimester,
	Asd.Year,
	MAX(CASE WHEN AW.WeekdayID = 1 AND AsubSch.StartTime IS NOT NULL THEN CONCAT(AsubSch.StartTime, '/', AsubSch.EndTime) END) Monday,
	MAX(CASE WHEN AW.WeekdayID = 2 AND AsubSch.StartTime IS NOT NULL THEN CONCAT(AsubSch.StartTime, '/', AsubSch.EndTime) END) Tuesday,
	MAX(CASE WHEN AW.WeekdayID = 3 AND AsubSch.StartTime IS NOT NULL THEN CONCAT(AsubSch.StartTime, '/', AsubSch.EndTime) END) Wednesday,
	MAX(CASE WHEN AW.WeekdayID = 4 AND AsubSch.StartTime IS NOT NULL THEN CONCAT(AsubSch.StartTime, '/', AsubSch.EndTime) END) Thursday,
	MAX(CASE WHEN AW.WeekdayID = 5 AND AsubSch.StartTime IS NOT NULL THEN CONCAT(AsubSch.StartTime, '/', AsubSch.EndTime) END) Friday,
	MAX(CASE WHEN AW.WeekdayID = 6 AND AsubSch.StartTime IS NOT NULL THEN CONCAT(AsubSch.StartTime, '/', AsubSch.EndTime) END) Saturday,
	Ag.Grade,
	(Ag.Points * Asu.Credits) Points
FROM Academic.StudentSubject Ass
	INNER JOIN Academic.SubjectDetail Asd ON Asd.SubjectDetailID = Ass.SubjectDetailID
	INNER JOIN Academic.Subject Asu ON Asu.SubjectID = Asd.SubjectID
	INNER JOIN Academic.SubjectSchedule AsubSch ON AsubSch.SubjectDetailID = Asd.SubjectDetailID
	INNER JOIN Academic.Weekday Aw ON Aw.WeekdayID = AsubSch.WeekdayID
	INNER JOIN Academic.Professor Ap ON Ap.PersonID = Asd.ProfessorID
	INNER JOIN Person.Person Pprof ON Pprof.PersonID = Ap.PersonID
	INNER JOIN Academic.SubjectClassroom Ascl ON Ascl.SubjectDetailID = Asd.SubjectDetailID
	INNER JOIN Academic.Classroom Ac ON Ac.ClassroomID = Ascl.ClassroomID
	LEFT JOIN Academic.Grade Ag ON Ag.GradeID = Ass.GradeID
	GROUP BY  Ass.StudentID,
	Asd.SubjectDetailID,
	Asu.SubjectCode,
	Asd.Section,
	Asu.Name,
	CONCAT(Pprof.FirstName, IIF(Pprof.MiddleName IS NULL, '', ' '), Pprof.MiddleName, ' ', Pprof.FirstSurname, IIF(Pprof.SecondSurname IS NULL, '', ' '), Pprof.SecondSurname),
	Asu.Credits,
	Ac.Code,
	Asd.Trimester,
	Asd.Year,
	Ag.Grade,
	Ag.Points * Asu.Credits
GO

-- Procedures
CREATE OR ALTER PROCEDURE Person.SP_PasswordUpsert
	@PersonID int,
	@PasswordHash nvarchar(64),
	@PasswordSalt nvarchar(5)
AS  
	IF EXISTS(
		SELECT 1
		FROM Person.PersonPassword pp
		WHERE pp.PersonID = @personID
	)
    BEGIN
        UPDATE Person.PersonPassword
        SET PasswordHash = @PasswordHash,
			PasswordSalt = @PasswordSalt
        WHERE PersonID = @personID
		RETURN 0
    END

	INSERT INTO Person.PersonPassword(PersonID, PasswordHash, PasswordSalt)
	VALUES(@PersonID, @PasswordHash, @PasswordSalt)
	RETURN 1
GO  

CREATE OR ALTER PROCEDURE Academic.SP_CareerRegistration
	@Name nvarchar(50),
	@Code nvarchar(3),
	@Subjects int,
	@Credits int,
	@Year int,
	@IsActive bit 
AS   
	INSERT INTO Academic.Career(Name, Code, Subjects, Credits, Year, IsActive)
	VALUES(@Name, @Code, @Subjects, @Credits, @Year, @IsActive)
	RETURN 1
GO

CREATE OR ALTER PROCEDURE Person.SP_PersonRegistration
	@DocNo nvarchar(13),
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
	VALUES(@DocNo, @FirstName, @MiddleName, @FirstSurname, @SecondSurname,
			UPPER(@Gender), @BirthDate, LOWER(@Email))
	RETURN 1
GO

--Grade revision
CREATE OR ALTER PROCEDURE Academic.SP_SubjectRetirement
	@StudentID int,
	@SubjectDetailID int
AS
BEGIN
	Update Academic.StudentSubject
	SET  GradeID = 8
	WHERE StudentID = @StudentID AND
		SubjectDetailID = @SubjectDetailID
END
GO

CREATE OR ALTER PROCEDURE Academic.SP_StudentRegistration
	@PersonID int,
	@CareerID int
AS
	INSERT INTO Academic.Student(PersonID, CareerID)
	VALUES(@personId, @CareerId)
	RETURN 1
GO

CREATE OR ALTER PROCEDURE Academic.SP_ProcessGradeRevision
	@StudentID int,
	@SubjectDetailID int,
	@ModifiedGradeID int,
	@AdminID int
AS
BEGIN
	DECLARE @ProfessorID int

	IF NOT EXISTS(
		SELECT 1 SubjectDetailID FROM Academic.GradeRevision
		WHERE ModifiedGradeID is null and SubjectDetailID = @SubjectDetailID and PersonId = @StudentID
	)
			RETURN 0
	BEGIN TRY

		SET @ProfessorID = (SELECT TOP(1) ProfessorID FROM Academic.SubjectDetail WHERE SubjectDetailID = @SubjectDetailID)
				PRINT('Set prof')

		BEGIN TRAN

		UPDATE Academic.GradeRevision
		SET ModifiedGradeID = @ModifiedGradeID, AdminID = @AdminID, ProfessorID = @ProfessorID, DateModified = GETDATE()
		WHERE SubjectDetailID = @SubjectDetailID and PersonId = @StudentID

		UPDATE Academic.StudentSubject
		SET GradeID = @ModifiedGradeID
		WHERE SubjectDetailID = @SubjectDetailID and StudentID = @StudentID;
		COMMIT
		RETURN 1
	END TRY
	BEGIN CATCH
		ROLLBACK
		RETURN 0
	END CATCH	
END
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
	IF (Person.F_ClassAvalibilityValidation(@SubjectDetailID) != 1)
		RETURN 0
	INSERT INTO Academic.StudentSubject(SubjectDetailID, StudentID)
	VALUES(@SubjectDetailID, @StudentID)
	RETURN 1
GO

CREATE OR ALTER PROCEDURE Academic.SP_SubjectElimination
	@SubjectDetailID int,
	@StudentID int
AS
	DELETE FROM Academic.StudentSubject
	WHERE SubjectDetailID = @SubjectDetailID AND  StudentID = @StudentID
GO

CREATE OR ALTER PROCEDURE Academic.SP_SubjectClassroomAssignment
	@SubjectDetailID int,
	@ClassroomID int
AS
	INSERT INTO Academic.SubjectClassroom(SubjectDetailID, ClassroomID)
	VALUES(@SubjectDetailID, @ClassroomID)
	RETURN 1
GO

CREATE OR ALTER PROCEDURE Academic.SP_SubjectScheduleAssignment
	@SubjectDetailID int,
	@WeekdayID int,
	@StartTime int,
	@EndTime int
AS
	INSERT INTO Academic.SubjectSchedule(SubjectDetailID, WeekdayID, StartTime, EndTime)
	VALUES(@SubjectDetailID, @WeekdayID, @StartTime, @EndTime)
	RETURN 1
GO

CREATE OR ALTER PROCEDURE Academic.SP_RequestGradeRevision
	@StudentID int,
	@SubjectDetailID int
AS
BEGIN
	DECLARE @GradeID int
	SET @GradeID = (
			SELECT Top(1) GradeID
			FROM Academic.StudentSubject
			WHERE StudentID = @StudentID AND
				SubjectDetailID = @SubjectDetailID
		)

	IF (@GradeID is null)
		RETURN 0

	INSERT INTO Academic.GradeRevision(PersonID, SubjectDetailID, DateRequested, GradeID)
	VALUES (@StudentID, @SubjectDetailID, GETDATE(), @GradeID)

	RETURN 1
END
GO

CREATE OR ALTER PROCEDURE Academic.SP_PublishGrade
	@StudentID int,
	@SubjectDetailID int,
	@GradeID int
AS
BEGIN
	UPDATE Academic.StudentSubject
	SET GradeID = @GradeID
	WHERE SubjectDetailID = @SubjectDetailID AND
		@StudentID = StudentID
END
GO

-- Functions
CREATE OR ALTER FUNCTION Person.F_ClassAvalibilityValidation(
	@SubjectDetailID int
)
RETURNS BIT
AS
	BEGIN
		IF (
			SELECT COUNT(StudentID) 
			FROM Academic.StudentSubject
			WHERE SubjectDetailID = @SubjectDetailID
		) 
			<
				(
				SELECT Capacity 
				FROM Academic.Classroom C
					JOIN Academic.SubjectClassroom SC ON SC.ClassroomID = C.ClassroomID
				WHERE SubjectDetailID = @SubjectDetailID
			)
				RETURN 1
		RETURN 0
	END
GO

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
		SELECT 1
		FROM Person.PersonPassword ppp
		WHERE ppp.PersonID = @PersonID AND
			ppp.PasswordHash = @PasswordHash
	)
		RETURN 1
	RETURN 0
END
GO

CREATE OR ALTER FUNCTION Academic.F_SubjectScheduleValidation(
	@StudentID int,
	@WeekdayID int,
	@StartTime int,
	@EndTime int
)
RETURNS NVARCHAR(9)
AS
BEGIN
	DECLARE @subject nvarchar(9) = (
		SELECT CONCAT(Asub.SubjectCode, '-', Asdet.Section)
		FROM Academic.SubjectSchedule Asch
			INNER JOIN Academic.SubjectDetail Asdet ON Asdet.SubjectDetailID = Asch.SubjectDetailID
			INNER JOIN Academic.Subject Asub ON Asub.SubjectID = Asdet.SubjectID
			INNER JOIN Academic.StudentSubject AstuSub ON AstuSub.SubjectDetailID = Asdet.SubjectDetailID
		WHERE Asch.WeekdayID = @WeekdayID AND
			(Asch.EndTime > @StartTime AND Asch.StartTime < @EndTime) AND
			AstuSub.StudentID = @StudentID
	)
    IF @subject IS NULL
		RETURN NULL
	RETURN @subject
END
GO

CREATE OR ALTER FUNCTION Academic.F_StudentSubjectValidation(
	@SubjectDetailID int,
	@StudentID int
)
RETURNS BIT
AS
BEGIN
	IF EXISTS(
		SELECT 1
		FROM Academic.StudentSubject
		WHERE SubjectDetailID = @SubjectDetailID AND
		StudentID = @StudentID
	)
		RETURN 1
	RETURN 0
END
GO

CREATE OR ALTER FUNCTION Academic.F_GetSubjectSchedule(
	@SubjectID int
)
RETURNS TABLE
AS
	RETURN
		SELECT *
		FROM Academic.vSubjectSchedule AvsubSche
		WHERE AvsubSche.SubjectDetailID = @SubjectID
GO

CREATE OR ALTER FUNCTION Academic.F_GetStudentsSchedule(
	@StudentID int,
	@Year int,
	@Trimester int
)
RETURNS TABLE
AS
	RETURN
		SELECT *
		FROM Academic.vStudentSubjects vSSD
		WHERE vSSD.StudentID = @StudentID		
				AND vSSD.Year = @Year AND
				vSSD.Trimester = @Trimester
GO

--Grade publication
CREATE OR ALTER FUNCTION Academic.F_GetSubjectsOfProfessor(
	@ProfessorId int
)
RETURNS TABLE
AS
	RETURN
		SELECT *
		FROM Academic.vSubjectSectionDetails Avsub
		WHERE Avsub.ProfessorID = @ProfessorId
GO

CREATE OR ALTER FUNCTION Academic.F_GetStudentsOfSubject(
	@SubjectID int
)
RETURNS TABLE
AS
	RETURN
		SELECT SS.StudentID, CONCAT(P.FirstName, ' ', MiddleName, ' ', FirstSurname, ' ', SecondSurname) [Nombres], SS.GradeID
		FROM Academic.StudentSubject SS
		JOIN Person.Person P ON SS.StudentID = P.PersonID

		WHERE SS.SubjectDetailID = @SubjectID
GO

-- Login
CREATE OR ALTER FUNCTION Academic.F_StudentLogin(
	@PersonID int,
	@PasswordHash nvarchar(64)
)
RETURNS TABLE
AS
	RETURN
		SELECT *
		FROM Academic.vStudentDetails Ast
		WHERE Ast.PersonID = @PersonID AND
			1 = (SELECT Person.F_PasswordValidation(@PersonID, @PasswordHash))
GO

CREATE OR ALTER FUNCTION Academic.F_AdminLogin(
	@PersonID int,
	@PasswordHash nvarchar(64)
)
RETURNS TABLE
AS
	RETURN
		SELECT *
		FROM Academic.vAdministratorDetails Adt
		WHERE Adt.PersonID = @PersonID AND
			1 = (SELECT Person.F_PasswordValidation(@PersonID, @PasswordHash))
GO

CREATE OR ALTER FUNCTION Academic.F_GetUnsolvedRevisions()
RETURNS TABLE
AS
	RETURN
		SELECT *
		FROM Academic.vGradeRevision 
		WHERE ModifiedGradeID is null
GO

CREATE OR ALTER FUNCTION Academic.F_ProfessorLogin(
	@PersonID int,
	@PasswordHash nvarchar(64)
)
RETURNS TABLE
AS
	RETURN
		SELECT *
		FROM Academic.vProfessorDetails Ap
		WHERE Ap.PersonID = @PersonID
GO

CREATE OR ALTER FUNCTION Person.F_GetPasswordSalt(
	@PersonID int
)
RETURNS nvarchar(5)
AS
BEGIN
	DECLARE @passwordSalt nvarchar(5)
	SELECT @passwordSalt = Ppp.PasswordSalt
	FROM Person.PersonPassword Ppp
	WHERE Ppp.PersonID = @PersonID
	RETURN @passwordSalt
END
GO

CREATE OR ALTER FUNCTION Academic.F_GetStudentCurrentSchedule(
	@StudentID int
)
RETURNS @schedule table(
	StudentID int,
	SubjectDetailID int,
	SubjectCode nvarchar(8),
	Section int,
	Subject nvarchar(100),
	Professor nvarchar(75),
	Credits tinyint,
	ClassroomCode nvarchar(7),
	Trimester int,
	Year int,
	Monday nvarchar(25),
	Tuesday nvarchar(25),
	Wednesday nvarchar(25),
	Thursday nvarchar(25),
	Friday nvarchar(25),
	Saturday nvarchar(25),
	Grade nvarchar(2),
	Points decimal(8, 4)
)
AS
BEGIN
		DECLARE @Year int,
			@Trimester int

		Set @Year = (
			SELECT  MAX(Year)
			FROM Academic.vSubjectSectionDetails
		) 
		Set @Trimester = (
			SELECT MAX(Trimester)
			FROM Academic.vSubjectSectionDetails
					WHERE Year = @Year
		)

		INSERT INTO @schedule SELECT * FROM Academic.F_GetStudentsSchedule(@StudentID, @Year, @Trimester)

		RETURN
END;
GO

CREATE OR ALTER FUNCTION F_GetSubjectStudent(
@SubjectDetailID int)
RETURNS TABLE
AS
RETURN
SELECT SS.StudentID, CONCAT(P.FirstName, ' ', P.MiddleName, ' ', P.FirstSurname, ' ', P.SecondSurname) [Nombre], C.Code [CareerCode], S.Trimester
FROM Academic.StudentSubject SS
JOIN Person.Person P ON P.PersonID = SS.StudentID
JOIN Academic.Student S ON S.PersonID = SS.StudentID
JOIN Academic.Career C ON C.CareerID = S.CareerID
WHERE SS.SubjectDetailID = @SubjectDetailID
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

 CREATE TRIGGER ChangedGrade
    ON Academic.StudentSubject
    AFTER UPDATE
AS
BEGIN
    UPDATE Academic.StudentSubject
    SET ModifiedDate = GETDATE()
    WHERE SubjectDetailID = (Select SubjectDetailID from Inserted) 
		AND StudentID =  (Select StudentID from Inserted) 
END
GO

-- Application User
CREATE USER projectIndiaCharlie
FOR LOGIN projectIndiaCharlieAPI
WITH DEFAULT_SCHEMA = Academic
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
ALTER ROLE API
	ADD MEMBER projectIndiaCharlie
GO

-- Views
-- View Definition
GRANT VIEW DEFINITION ON Academic.vStudentSubjects
	TO API
GO
GRANT VIEW DEFINITION ON Academic.vStudentDetails
	TO API
GO
GRANT VIEW DEFINITION ON Academic.vSubjectSchedule
	TO API
GO
GRANT VIEW DEFINITION ON Academic.vProfessorDetails
	TO API
GO
GRANT VIEW DEFINITION ON Academic.vSubjectSectionDetails
	TO API
GO
GRANT VIEW DEFINITION ON Academic.vAdministratorDetails
	TO API
GO
GRANT VIEW DEFINITION ON Academic.vGradeRevision
	TO API
GO
GRANT VIEW DEFINITION ON Academic.vAvailableCareers
	TO API
GO
GRANT VIEW DEFINITION ON Person.vPeopleDetails
	TO API
GO
GRANT VIEW DEFINITION ON Academic.vGrades
	TO API
GO
-- Select
GRANT SELECT ON Academic.vStudentSubjects
	TO API
GO
GRANT SELECT ON Academic.vStudentDetails
	TO API
GO
GRANT SELECT ON Academic.vSubjectSchedule
	TO API
GO
GRANT SELECT ON Academic.vProfessorDetails
	TO API
GO
GRANT SELECT ON Academic.vSubjectSectionDetails
	TO API
GO
GRANT SELECT ON Academic.vAdministratorDetails
	TO API
GO
GRANT SELECT ON Academic.vGradeRevision
	TO API
GO
GRANT SELECT ON Academic.vAvailableCareers
	TO API
GO
GRANT SELECT ON Person.vPeopleDetails
	TO API
GO
GRANT SELECT ON Academic.vGrades
	TO API
GO

-- Functions
GRANT SELECT ON Academic.F_GetSubjectSchedule
	TO API
GO
GRANT SELECT ON Academic.F_StudentLogin
	TO API
GO
GRANT SELECT ON Academic.F_AdminLogin
	TO API
GO
GRANT SELECT ON Academic.F_GetSubjectsOfProfessor
	TO API
GO
GRANT EXECUTE ON Academic.F_StudentValidation
	TO API
GO
GRANT SELECT ON Academic.F_GetStudentsSchedule
	TO API
GO
GRANT EXECUTE ON Person.F_ClassAvalibilityValidation
	TO API
GO
GRANT EXECUTE ON Academic.F_SubjectScheduleValidation
	TO API
GO
GRANT EXECUTE ON Academic.F_ProfessorValidation
	TO API
GO
GRANT EXECUTE ON Person.F_PasswordValidation
	TO API
GO
GRANT EXECUTE ON Person.F_DocNoValidation
	TO API
GO
GRANT EXECUTE ON Person.F_GetPasswordSalt
	TO API
GO
GRANT SELECT ON Academic.F_GetUnsolvedRevisions
	TO API
GO
GRANT SELECT ON Academic.F_GetStudentCurrentSchedule
	TO API
GO
GRANT SELECT ON Academic.F_ProfessorLogin
	TO API
GO
GRANT SELECT ON Academic.F_GetStudentsOfSubject
	TO API
GO
GRANT EXECUTE ON Academic.F_StudentSubjectValidation
	TO API
GO
-- SPs
GRANT EXECUTE ON Academic.SP_SubjectScheduleAssignment
	TO API
GO
GRANT EXECUTE ON Academic.SP_SubjectSelection
	TO API
GO
GRANT EXECUTE ON Academic.SP_CareerRegistration
	TO API
GO
GRANT EXECUTE ON Academic.SP_StudentRegistration
	TO API
GO
GRANT EXECUTE ON Academic.SP_SubjectElimination
	TO API
GO
GRANT EXECUTE ON Academic.SP_ProfessorRegistration
	TO API
GO
GRANT EXECUTE ON Academic.SP_SubjectRetirement
	TO API
GO
GRANT EXECUTE ON Person.SP_PersonRegistration
	TO API
GO
GRANT EXECUTE ON Academic.SP_SubjectClassroomAssignment
	TO API
GO
GRANT EXECUTE ON Academic.SP_ProcessGradeRevision
	TO API
GO
GRANT EXECUTE ON Academic.SP_PublishGrade
	TO API
GO
GRANT EXECUTE ON Person.SP_PasswordUpsert
	TO API
GO
GRANT EXECUTE ON Academic.SP_RequestGradeRevision
	TO API
GO


---- Login
--DROP LOGIN ProjectIndiaCharlieAPI
--GO
--CREATE LOGIN ProjectIndiaCharlieAPI
--WITH Password = 'picAPI',
--DEFAULT_DATABASE = ProjectIndiaCharlie
--GO

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

--INSERT INTO Person.Person(DocNo, FirstName, FirstSurname, Gender, BirthDate, Email)
--VALUES('123', 'Juan', 'Cito', 'M', '2002-02-01', 'juancito@mail.com')

--INSERT INTO Academic.Student(PersonID, CareerID)
--VALUES(1110201, 1)

--INSERT INTO Academic.Career(Name, Code, Subjects, Credits, IsActive, Year)
--VALUES('Software', 'IDS', 40, 240, 1, 2020)

--INSERT INTO Person.PersonPassword(PersonID, PasswordHash, PasswordSalt)
--VALUES(1110201, '123', '123')

--SELECT * FROM Person.PersonPassword
--SELECT * FROM Person.F_StudentLogin('1110201', '96cae35ce8a9b0244178bf28e4966c2ce1b8385723a96a6b838858cdd6ca0a1e')