USE master;
GO
DROP DATABASE IF EXISTS ProjectIndiaCharlie;
GO
CREATE DATABASE ProjectIndiaCharlie
COLLATE SQL_Latin1_General_CP1_CI_AS ;
GO
USE ProjectIndiaCharlie;
GO

CREATE SCHEMA Person;
GO
CREATE SCHEMA Academic;
GO

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
  ProfessorID int NOT NULL,
  SubjectID int NOT NULL,
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
INSERT INTO Person.Role(RoleName)
VALUES	('Student'),
		('Profesor'),
		('Cordinator');
		
INSERT INTO Academic.Classroom(Code, IsLab, Capacity)
VALUES ('FD402', 0, 20)

INSERT INTO Academic.Grade(Grade, Points)
VALUES ('A', 4),
	('B+', 3.5),
	('B', 3),
	('C+', 2.5),
	('C', 2),
	('D', 1),
	('F', 0);
	
INSERT INTO Academic.Weekday(Name)
VALUES	('Monday'),
	('Tuesday'),
	('Wednesday'),
	('Thursday'),
	('Friday'),
	('Saturday')

--EXEC Person.SP_RegisterPerson 
--	@DocNo			= '40231024361', 
--	@FirstName		= 'Nikita', 
--	@MiddleName		= 'Alekseevich', 
--	@FirstSurname	= 'Kravchenko', 
--	@Gender			= 'M', 
--	@BirthDate		= '1998-10-12', 
--	@Email			= 'android.oct7@gmail.com';
--GO

--EXEC Person.SP_RegisterPerson 
--	@DocNo			= '98765432109', 
--	@FirstName		= 'Ramón', 
--	@FirstSurname	= 'Ramírez', 
--	@Gender			= 'M', 
--	@BirthDate		= '2000-01-02', 
--	@Email			= 'ran.32@gmail.com';
--GO

EXEC [Person].[SP_RegisterPerson]
	@DocNo = N'12345678901',
	@FirstName = N'Roger',
	@FirstSurname = N'Ramiro',
	@Gender = N'm',
	@BirthDate = '1988-02-10',
	@Email = N'ramiro@mail.com',
	@RolId = 3,
	@PasswordHash = N'fdbfd43cca1aed1691d4bd7821d700449d53fd84f419462de98c6504da136687',
	@PasswordSalt = N'8ui37'
GO

EXEC Academic.SP_AddCareer
	@CoordinatorID	= 1110201,
	@Name			= 'Ingeniería de Software',
	@Code			= 'IDS',
	@Subjects		= 111,
	@Credits		= 279,
	@Year			= 2020,
	@IsActive		= 1;  
GO

EXEC [Person].[SP_RegisterPerson]
	@DocNo = N'38565826658',
	@FirstName = N'Raquel',
	@FirstSurname = N'Rijo',
	@Gender = N'f',
	@BirthDate = '2000-01-01',
	@Email = N'raquelita@mail.net',
	@RolId = 1,
	@CareerId = 1,
	@PasswordHash = N'e14ceeffc3107c5956645fe09232515ed7d2af3048eea37e2571bd340c7ef05a',
	@PasswordSalt = N'jj331'
GO

EXEC [Person].[SP_RegisterPerson]
	@DocNo = N'38565826658',
	@FirstName = N'Raquel',
	@FirstSurname = N'Rijo',
	@Gender = N'f',
	@BirthDate = '2000-01-01',
	@Email = N'raquelita@mail.net',
	@RolId = 2,
	@PasswordHash = N'e14ceeffc3107c5956645fe09232515ed7d2af3048eea37e2571bd340c7ef05a',
	@PasswordSalt = N'jj331'
GO


--DECLARE @Path NVARCHAR(MAX);
--DECLARE @FileLoc NVARCHAR(MAX);
--DECLARE @SQL_BULK VARCHAR(MAX);
--SET @Path = 'C:\Users\Nikita\Desktop\Projects\';

--SET @FileLoc = @Path + 'Asignaturas1.csv';

--SET @SQL_BULK = 'BULK INSERT  Academic.Subject
--FROM  ''' + @FileLoc + '''
--WITH (
--  FIELDTERMINATOR = '';'',
--  ROWTERMINATOR = ''\n'',
--  FIRSTROW = 2,
--  CODEPAGE = ''ACP''
--);' --  
--EXEC (@SQL_BULK);
BULK INSERT  Academic.Subject
FROM  'C:\Users\Nikita\Desktop\Projects\Asignaturas.csv'
WITH (
  FIELDTERMINATOR = ';',
  ROWTERMINATOR = '\n',
  FIRSTROW = 2,
  CODEPAGE = 'ACP'
);
--CREATE TABLE Academic.Section(
--  SubjectDetailID int identity,
--  ProfessorID int NOT NULL,
--  SubjectID int NOT NULL,
--  Section int NOT NULL,
--  Trimester int NOT NULL,
--  Year int NOT NULL,
--  CreatedDate datetime NOT NULL DEFAULT GETDATE(), 
--  ModifiedDate datetime NOT NULL DEFAULT GETDATE(),
--);

SELECT * FROM Person.F_Login('1110202', 'e14ceeffc3107c5956645fe09232515ed7d2af3048eea37e2571bd340c7ef05a')
GO

--UPDATE Person.Person 
--SET 
--    --DocNo = '40243310243',
--	Email = 'androidd@mail.com'
--WHERE
--    PersonID = 1;

--EXEC Person.SP_AssignRole
--	@PersonID	= 1110202,
--	@RoleId		= 2;

--EXEC Person.SP_AssignRole
--	@PersonID	= 1110202,
--	@RoleId		= 3;

--EXEC Person.SP_AssignRole
--	@PersonID	= 1110201,
--	@RoleId		= 1,
--	@CareerID = 1;

--EXEC Person.SP_UpsertPassword 
--@PersonID	= 1110201,
--@PasswordHash = '7aa29861c7da79138967fcdf217112320bb79afbab5cb5d470a040a1f473f96d',
--@PasswordSalt = 'jbchk';

--EXEC Person.SP_UpsertPassword 
--@PersonID	= 1110202,
--@PasswordHash = 'd87c4581fc345b94449702d0ed8e954adaaa07ee86417decdf0a07fad4b9d4dd',
--@PasswordSalt = '12345';

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
