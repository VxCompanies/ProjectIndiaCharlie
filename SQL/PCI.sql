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

-- Application Role
CREATE APPLICATION ROLE Academics
	WITH PASSWORD='p1c4P1',
	DEFAULT_SCHEMA = Academic
GO
--ALTER AUTHORIZATION ON SCHEMA::Academic
--TO Academics
--GO
--ALTER AUTHORIZATION ON SCHEMA::Person
--TO Academics
--GO

DECLARE @Path NVARCHAR(MAX);
DECLARE @FileLoc NVARCHAR(MAX);
DECLARE @SQL_BULK VARCHAR(MAX);
SET @Path = 'C:\Users\Nikita\Desktop\Projects\ProjectIndiaCharlie\SQL\';--Path to folder of your pc for bulk insert script


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
);
GO

CREATE TABLE Person.PersonPassword(
  PersonID int,
  PasswordHash nvarchar(64) NOT NULL,
  PasswordSalt nvarchar(5) NOT NULL,
  CreatedDate datetime NOT NULL DEFAULT GETDATE(),
  ModifiedDate datetime NOT NULL DEFAULT GETDATE(),

  PRIMARY KEY(PersonID),
  FOREIGN KEY(PersonID) REFERENCES Person.Person(PersonID)
);
GO

CREATE TABLE Person.Role(
  RoleID int identity,
  RoleName nvarchar(64) NOT NULL,
  CreatedDate datetime NOT NULL DEFAULT GETDATE(), 
  ModifiedDate datetime NOT NULL DEFAULT GETDATE(),

  PRIMARY KEY(RoleID)
);
GO

CREATE TABLE Person.PersonRole(
  PersonID int,
  RoleID int,
  CreatedDate datetime NOT NULL DEFAULT GETDATE(), 
  ModifiedDate datetime NOT NULL DEFAULT GETDATE(),

  PRIMARY KEY(PersonID, RoleID),
  FOREIGN KEY(PersonID) REFERENCES Person.Person(PersonID),
  FOREIGN KEY(RoleID) REFERENCES Person.Role(RoleID)
);
GO

CREATE TABLE Academic.Coordinator(
  PersonID int,
  CreatedDate datetime NOT NULL DEFAULT GETDATE(), 
  ModifiedDate datetime NOT NULL DEFAULT GETDATE(),

  PRIMARY KEY(PersonID),
  FOREIGN KEY(PersonID) REFERENCES Person.Person(PersonID)
);
GO

CREATE TABLE Academic.Professor(
  PersonID int,
  CreatedDate datetime NOT NULL DEFAULT GETDATE(), 
  ModifiedDate datetime NOT NULL DEFAULT GETDATE(),

  PRIMARY KEY(PersonID),
  FOREIGN KEY(PersonID) REFERENCES Person.Person(PersonID)
);
GO

CREATE TABLE Academic.Career(
  CareerID int identity,
  CoordinatorID int NOT NULL,
  Name nvarchar(50) NOT NULL,
  Code nvarchar(3) NOT NULL,
  Subjects int NOT NULL,
  Credits int NOT NULL,
  Year int NOT NULL,
  IsActive bit NOT NULL,
  CreatedDate datetime NOT NULL DEFAULT GETDATE(), 
  ModifiedDate datetime NOT NULL DEFAULT GETDATE(),

  PRIMARY KEY(CareerID),
  FOREIGN KEY(CoordinatorID) REFERENCES Academic.Coordinator(PersonID)
);
GO

CREATE TABLE Academic.Student(
  PersonID int,
  CareerID int NOT NULL,
  GeneralIndex decimal(3, 2) NOT NULL DEFAULT 0,
  TrimestralIndex decimal(3, 2) NOT NULL DEFAULT 0,
  Trimester int NOT NULL DEFAULT 1,
  EnrolementDate date NOT NULL DEFAULT GETDATE(),
  CreatedDate datetime NOT NULL DEFAULT GETDATE(), 
  ModifiedDate datetime NOT NULL DEFAULT GETDATE(),

  PRIMARY KEY(PersonID),
  FOREIGN KEY(PersonID) REFERENCES Person.Person(PersonID), 
  FOREIGN KEY(CareerID) REFERENCES Academic.Career(CareerID)
);
GO

CREATE TABLE Academic.Grade(
  GradeID int identity,
  Grade nvarchar(2) NOT NULL,
  Points float NOT NULL,
  CreatedDate datetime NOT NULL DEFAULT GETDATE(), 
  ModifiedDate datetime NOT NULL DEFAULT GETDATE(),

  PRIMARY KEY(GradeID)
);
GO

CREATE TABLE Academic.Subject(
  SubjectID int identity,
  SubjectCode nvarchar(7) NOT NULL,
  Name nvarchar(100) NOT NULL,
  Credits INT NOT NULL,
  CreatedDate datetime NOT NULL DEFAULT GETDATE(), 
  ModifiedDate datetime NOT NULL DEFAULT GETDATE(),

  PRIMARY KEY(SubjectID),
);
GO

CREATE TABLE Academic.Section(
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
  FOREIGN KEY(SubjectID) REFERENCES Academic.Subject(SubjectID),
);
GO

CREATE TABLE Academic.SubjectStudent(
  SubjectDetailID int,
  StudentID int,
  GradeID int NULL,
  CreatedDate datetime NOT NULL DEFAULT GETDATE(), 
  ModifiedDate datetime NOT NULL DEFAULT GETDATE(),

  PRIMARY KEY(SubjectDetailID, StudentID),
  FOREIGN KEY(StudentID) REFERENCES Academic.Student(PersonID),  
  FOREIGN KEY(SubjectDetailID) REFERENCES Academic.Section(SubjectDetailID)
);
GO

CREATE TABLE Academic.Classroom(
  ClassroomID int identity,
  Code nvarchar(5) NOT NULL,
  IsLab bit NOT NULL,
  Capacity int NOT NULL,
  CreatedDate datetime NOT NULL DEFAULT GETDATE(), 
  ModifiedDate datetime NOT NULL DEFAULT GETDATE(),

  PRIMARY KEY(ClassroomID)
 );
GO

CREATE TABLE Academic.SubjectClassroom(
  SubjectDetailID int,
  ClassroomID int,
  CreatedDate datetime NOT NULL DEFAULT GETDATE(), 
  ModifiedDate datetime NOT NULL DEFAULT GETDATE(),

  FOREIGN KEY(SubjectDetailID) REFERENCES Academic.Section(SubjectDetailID),
  FOREIGN KEY(ClassroomID) REFERENCES Academic.Classroom(ClassroomID),
  PRIMARY KEY(SubjectDetailID, ClassroomID)
);
GO

CREATE TABLE Academic.Weekday(
  WeekdayID int identity,
  Name nvarchar(9) NOT NULL,
  CreatedDate datetime NOT NULL DEFAULT GETDATE(), 
  ModifiedDate datetime NOT NULL DEFAULT GETDATE(),

  PRIMARY KEY(WeekdayID)
);
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
  FOREIGN KEY(SubjectDetailID) REFERENCES Academic.Section(SubjectDetailID),
  FOREIGN KEY(WeekdayID) REFERENCES Academic.Weekday(WeekdayID)
);
GO

-- Procedures
CREATE OR ALTER PROCEDURE Person.SP_UpsertPassword
	@PersonID		int,
	@PasswordHash	nvarchar(64),
	@PasswordSalt	nvarchar(5)
AS  
	IF EXISTS(
		SELECT 1
		FROM Person.PersonPassword pp
		WHERE pp.PersonID = @personID)
    BEGIN
        UPDATE Person.PersonPassword
        SET PasswordHash = @passwordHash
        WHERE PersonID = @personID
    END
	ELSE
		BEGIN
		INSERT INTO Person.PersonPassword(PersonID, PasswordHash, PasswordSalt)
		VALUES(@PersonID, @PasswordHash, @PasswordSalt);
		END
GO  

CREATE OR ALTER PROCEDURE Academic.SP_AddCareer
	@CoordinatorID	int,
	@Name			nvarchar(50),
	@Code			nvarchar(3),
	@Subjects		int,
	@Credits		int,
	@Year			int,
	@IsActive		bit 
AS   
	INSERT INTO Academic.Career(CoordinatorID, Name, Code, 
								 Subjects, Credits, Year, IsActive)
	VALUES(@CoordinatorID, @Name, @Code,			
			@Subjects, @Credits, @Year, @IsActive);
GO  

CREATE OR ALTER PROCEDURE Academic.SP_AssignRoleCoordinator
	@PersonID		int
AS   
	INSERT INTO Academic.Coordinator(PersonID)
	VALUES(@PersonID);
GO  

CREATE OR ALTER PROCEDURE Academic.SP_AssignRoleProfessor
	@PersonID		int
AS   
	INSERT INTO Academic.Professor(PersonID)
	VALUES(@PersonID);
GO  

CREATE OR ALTER PROCEDURE Academic.SP_AssignRoleStudent
	@PersonID			int,
	@CareerID			int
AS
	INSERT INTO Academic.Student(PersonID, CareerID)
	VALUES(@PersonID,  @CareerID);
GO  

CREATE OR ALTER PROCEDURE Person.SP_AssignRole   
	@PersonID		int,
	@RoleId			int,
	@CareerID int = 0
AS
	INSERT INTO Person.PersonRole(PersonID, RoleID)
	VALUES(@PersonID, @RoleId)

	IF @RoleId = 1 
		BEGIN
			EXEC Academic.SP_AssignRoleStudent
			@PersonID = @PersonID,
			@CareerID = @CareerID;
		END
	ELSE IF @RoleId = 2
		BEGIN
			EXEC Academic.SP_AssignRoleProfessor
			@PersonID = @PersonID;
		END
	ELSE IF @RoleId = 3
		BEGIN
			EXEC Academic.SP_AssignRoleCoordinator
			@PersonID = @PersonID;
		END
GO  

CREATE OR ALTER PROCEDURE Person.SP_RegisterPerson
	@DocNo			nvarchar(11),
	@FirstName		nvarchar(50),
	@MiddleName		nvarchar(50) = null,
	@FirstSurname	nvarchar(50),
	@SecondSurname	nvarchar(50) = null,
	@Gender			nvarchar(1),
	@BirthDate		date,
	@Email			nvarchar(255),
	@RolId int,
	@CareerId int = 0,
	@PasswordHash nvarchar(64),
	@PasswordSalt nvarchar(5)
AS
	DECLARE @personId int

	IF NOT EXISTS(
		SELECT 1
		FROM Person.Person pp
		WHERE pp.DocNo = FORMAT(CAST(@DocNo as bigint), '000-0000000-0')
	)
	BEGIN
		INSERT INTO Person.Person(DocNo, FirstName, MiddleName, FirstSurname, SecondSurname,
					Gender, BirthDate, Email)
		VALUES(FORMAT(CAST(@DocNo as bigint), '000-0000000-0'), @FirstName, @MiddleName, @FirstSurname, @SecondSurname, 
				UPPER(@Gender), @BirthDate, @Email)

		SET @personId =(SELECT pp.PersonID FROM Person.Person pp WHERE pp.DocNo = FORMAT(CAST(@DocNo as bigint), '000-0000000-0'))
		EXEC Person.SP_UpsertPassword @personId, @PasswordHash, @PasswordSalt
	END
	
	SET @personId =(SELECT pp.PersonID FROM Person.Person pp WHERE pp.DocNo = FORMAT(CAST(@DocNo as bigint), '000-0000000-0'))
	EXEC Person.SP_AssignRole @personId, @RolId, @CareerId;
GO

-- Functions
CREATE FUNCTION Person.F_PasswordValidation(
	@PersonID int,
	@PasswordHash nvarchar(64)
)
RETURNS BIT AS
BEGIN
    IF EXISTS(
		SELECT * 
		FROM Person.PersonPassword
		WHERE PersonID = @PersonID AND PasswordHash = @PasswordHash
	)
		RETURN 1
	RETURN 0
END
GO

CREATE OR ALTER FUNCTION Person.F_Login(
	@PersonID int,
	@PasswordHash nvarchar(64)
)
RETURNS TABLE
AS
	RETURN
		SELECT p.PersonID,
			p.DocNo,
			p.FirstName,
			p.MiddleName,
			p.FirstSurname,
			p.SecondSurname,
			aca.Name,
			aca.Year,
			ast.GeneralIndex,
			ast.TrimestralIndex
		FROM Person.Person P
		JOIN Person.PersonPassword PPas ON P.PersonID = PPas.PersonID
		LEFT JOIN Academic.Coordinator ac ON P.PersonID = ac.PersonID
		LEFT JOIN Academic.Professor ap ON P.PersonID = ap.PersonID
		LEFT JOIN Academic.Student ast ON P.PersonID = ast.PersonID
		LEFT JOIN Academic.Career aca ON ast.CareerID = aca.CareerID
		WHERE P.PersonID = @PersonID AND PPas.PasswordHash = @PasswordHash
GO

-- Views
CREATE VIEW Person.vRoles
AS
    SELECT pr.RoleId,
		pr.RoleName
	FROM Person.Role pr
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

CREATE TRIGGER RoleModifiedPersonPerson
    ON Person.PersonRole
    AFTER UPDATE
AS
BEGIN
    UPDATE Person.PersonRole 
    SET ModifiedDate = GETDATE()
    WHERE PersonID =(Select PersonID from Inserted) AND RoleID =(Select RoleID from Inserted)
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

-- Views
CREATE OR ALTER VIEW Academic.vStudents
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
	ast.EnrolementDate,
	ac.Name Career,
	ac.Code,
	ac.Year Pensum
FROM Person.Person pp
	INNER JOIN Academic.Student ast ON pp.PersonID = ast.PersonID
	INNER JOIN Academic.Career ac ON ast.CareerID = ac.CareerID
GO

-- Testing



SELECT * FROM Person.F_Login('1110202', 'e14ceeffc3107c5956645fe09232515ed7d2af3048eea37e2571bd340c7ef05a')
GO



SELECT * FROM Person.Person;
SELECT * FROM Person.PersonPassword;
SELECT * FROM Person.PersonRole;
SELECT * FROM Person.Role;

SELECT * FROM Academic.Career;
SELECT * FROM Academic.Classroom;
SELECT * FROM Academic.Coordinator;
SELECT * FROM Academic.Grade;
SELECT * FROM Academic.Professor;
SELECT * FROM Academic.Schedule;
SELECT * FROM Academic.Section;
SELECT * FROM Academic.Student;
SELECT * FROM Academic.Subject;
SELECT * FROM Academic.SubjectClassroom;
SELECT * FROM Academic.SubjectStudent;
SELECT * FROM Academic.Weekday;
