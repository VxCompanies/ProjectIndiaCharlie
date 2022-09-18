USE ProjectIndiaCharlie;

DECLARE @Path NVARCHAR(MAX);
DECLARE @FileLoc NVARCHAR(MAX);
DECLARE @SQL_BULK VARCHAR(MAX);

--SET @Path = 'E:\Desarrollo\ProjectIndiaCharlie\SQL\';--Path to folder of your pc for bulk insert script
SET @Path = 'C:\Users\omars\source\repos\VxGameX\IDS325-01\ProjectIndiaCharlie\SQL\';--Path to folder of your pc for bulk insert script

SET @FileLoc = @Path + 'Asignaturas.csv';

SET @SQL_BULK = 'BULK INSERT  Academic.Subject
FROM  ''' + @FileLoc + '''
WITH (
  FIELDTERMINATOR = '';'',
  ROWTERMINATOR = ''\n'',
  FIRSTROW = 2,
  CODEPAGE = ''ACP''
);' --  
EXEC(@SQL_BULK);

SET @FileLoc = @Path + 'Persons.csv';

SET @SQL_BULK = 'BULK INSERT  Person.Person
FROM  ''' + @FileLoc + '''
WITH (
  FIELDTERMINATOR = '';'',
  ROWTERMINATOR = ''\n'',
  FIRSTROW = 2,
  CODEPAGE = ''ACP''
);' --  
EXEC(@SQL_BULK);

SET @FileLoc = @Path + 'Profesores.csv';

SET @SQL_BULK = 'BULK INSERT  Academic.Professor
FROM  ''' + @FileLoc + '''
WITH (
  FIELDTERMINATOR = '';'',
  ROWTERMINATOR = ''\n'',
  FIRSTROW = 2,
  CODEPAGE = ''ACP''
);' --  
EXEC(@SQL_BULK);

SET @FileLoc = @Path + 'SubjectDetail.csv';

SET @SQL_BULK = 'BULK INSERT  Academic.SubjectDetail
FROM  ''' + @FileLoc + '''
WITH (
  FIELDTERMINATOR = '';'',
  ROWTERMINATOR = ''\n'',
  FIRSTROW = 2,
  CODEPAGE = ''ACP''
);' --  
EXEC(@SQL_BULK);

SET @FileLoc = @Path + 'Classroom.csv';

SET @SQL_BULK = 'BULK INSERT  Academic.Classroom
FROM  ''' + @FileLoc + '''
WITH (
  FIELDTERMINATOR = '';'',
  ROWTERMINATOR = ''\n'',
  FIRSTROW = 2,
  CODEPAGE = ''ACP''
);' --  
EXEC(@SQL_BULK);

SET @FileLoc = @Path + 'SubjectClassroom.csv';

SET @SQL_BULK = 'BULK INSERT  Academic.SubjectClassroom
FROM  ''' + @FileLoc + '''
WITH (
  FIELDTERMINATOR = '';'',
  ROWTERMINATOR = ''\n'',
  FIRSTROW = 2,
  CODEPAGE = ''ACP''
);' --  
EXEC(@SQL_BULK);

SET @FileLoc = @Path + 'SubjectSchedule.csv';

SET @SQL_BULK = 'BULK INSERT  Academic.SubjectSchedule
FROM  ''' + @FileLoc + '''
WITH (
  FIELDTERMINATOR = '';'',
  ROWTERMINATOR = ''\n'',
  FIRSTROW = 1,
  CODEPAGE = ''ACP''
);' --  
EXEC(@SQL_BULK);

SET @FileLoc = @Path + 'Student.csv';

SET @SQL_BULK = 'BULK INSERT  Academic.Student
FROM  ''' + @FileLoc + '''
WITH (
  FIELDTERMINATOR = '';'',
  ROWTERMINATOR = ''\n'',
  FIRSTROW = 2,
  CODEPAGE = ''ACP''
);' --  
EXEC(@SQL_BULK);

SET @FileLoc = @Path + 'PersonPassword.csv';

SET @SQL_BULK = 'BULK INSERT  Person.PersonPassword
FROM  ''' + @FileLoc + '''
WITH (
  FIELDTERMINATOR = '';'',
  ROWTERMINATOR = ''\n'',
  FIRSTROW = 2,
  CODEPAGE = ''ACP''
);' --  
EXEC(@SQL_BULK);

INSERT INTO Academic.Weekday(Name)
VALUES	('Monday'),
	('Tuesday'),
	('Wednesday'),
	('Thursday'),
	('Friday'),
	('Saturday');
GO

INSERT INTO Academic.Grade(Grade, Points)
VALUES ('A', 4),
	('B+', 3.5),
	('B', 3),
	('C+', 2.5),
	('C', 2),
	('D', 1),
	('F', 0);

INSERT INTO Academic.Career(Name, Code, Subjects, Credits, Year, IsActive)
VALUES ('Medicina', 'MED', 204, 425, 2020, 1),
('Ingeniería de Software', 'IDS', 111, 279, 2020, 1),
('Ingeniería Mecánica', 'MEC', 112, 280, 2020, 1);
GO

--Admins
INSERT INTO Person.Person(DocNo, FirstName, FirstSurname, Gender, BirthDate, Email)
VALUES		('66666666666', 'Nikita', 'Kravchenko', 'M', '1998-10-12', 'nikita1998@mail.com'),
			('11111111111', 'Omar', 'Núñez', 'M', '2002-5-18', 'omarnun2002@mail.com');
GO

INSERT INTO Academic.Administrator(PersonID)
VALUES		(1112200),(1112201);
GO

INSERT INTO Person.PersonPassword(PersonID ,PasswordHash, PasswordSalt)
VALUES		(1112200, '06d6a394462a1f19abf14fa321174311d642dfc55f11d050b011c79de7321df3', 'mmmmm'),
			(1112201, '06d6a394462a1f19abf14fa321174311d642dfc55f11d050b011c79de7321df3', 'mmmmm');
GO

--Datos de prueba de secciones
INSERT INTO Academic.StudentSubject(SubjectDetailID, StudentID)
VALUES (1, 1110408), (2, 1110409), (2, 1110408);
GO

EXEC Academic.SP_SubjectSelection
	@SubjectDetailId = 2,
	@StudentId	= 1110412;
EXEC Academic.SP_SubjectSelection
	@SubjectDetailId = 2,
	@StudentId	= 1110413;
EXEC Academic.SP_SubjectSelection
	@SubjectDetailId = 2,
	@StudentId	= 1110414;
EXEC Academic.SP_SubjectSelection
	@SubjectDetailId = 2,
	@StudentId	= 1110415;
EXEC Academic.SP_SubjectSelection
	@SubjectDetailId = 2,
	@StudentId	= 1110416;
EXEC Academic.SP_SubjectSelection
	@SubjectDetailId = 2,
	@StudentId	= 1110417;
EXEC Academic.SP_SubjectSelection
	@SubjectDetailId = 2,
	@StudentId	= 1110418;
EXEC Academic.SP_SubjectSelection
	@SubjectDetailId = 2,
	@StudentId	= 1110419;

SELECT * FROM Academic.vSubjectSectionDetails
WHERE SubjectDetailID = 2;

--UPDATE Academic.SubjectDetail
--SET Year = 2022
--WHERE SubjectDetailID = 2;

EXEC Academic.SP_GetLastTrimesterStudentsSchedule @StudentID = 1110408;

SELECT * FROM Academic.F_GetStudentsSchedule(1110408, 2022, 3);

EXEC Academic.SP_SubjectSelection
	@SubjectDetailId = 2,
	@StudentId	= 1110422;
EXEC Academic.SP_SubjectSelection
	@SubjectDetailId = 2,
	@StudentId	= 1110423;
	
EXEC Academic.SP_SubjectElimination
	@SubjectDetailId = 2,
	@StudentId	= 1110422;

--EXEC Academic.SP_SubjectSelection
--	@SubjectDetailId = 2,
--	@StudentId	= 1110424;

EXEC Academic.SP_GetLastTrimesterStudentsSchedule
@StudentID = 1110408;

SELECT * 
FROM Academic.SubjectDetail SD
JOIN Academic.Subject S ON SD.SubjectID = S.SubjectID
WHERE SubjectCode = 'MED348';

EXEC Academic.SP_PublishGrade
@StudentID = 1110409,
@SubjectDetailID = 2,
@GradeID = 1;

SELECT * FROM Academic.StudentSubject;
SELECT * FROM Academic.F_GetSubjectsOfProfessor(1110201);
SELECT * FROM Academic.F_GetStudentsOfSubject(2);
GO

SELECT *-- SS.SubjectScheduleID, S.SubjectID, W.Name, SS.StartTime, SS.EndTime, SD.ProfessorID, SD.Section, SD.Trimester,
	--S.SubjectCode, S.Name, S.Credits, P.FirstName, P.MiddleName, P.FirstSurname, P.SecondSurname
FROM Academic.SubjectSchedule SS
JOIN Academic.SubjectDetail SD on SS.SubjectDetailID = SD.SubjectID
JOIN ACADEMIC.Subject S ON SD.SubjectId = S.SubjectId
--JOIN Person.Person P ON SD.ProfessorID = P.PersonID
JOIN Academic.Weekday W ON SS.WeekdayID = W.WeekdayID
ORDER BY SS.SubjectScheduleID
--WHERE S.Name LIKE '%vector%';

SELECT * FROM Person.Person;
SELECT * FROM Person.PersonPassword;

SELECT * FROM Academic.Career;
SELECT * FROM Academic.Classroom;
SELECT * FROM Academic.Grade;
SELECT * FROM Academic.Professor;
SELECT * FROM Academic.Student;
SELECT * FROM Academic.StudentSubject;

SELECT * FROM Academic.SubjectDetail;
SELECT * FROM Academic.SubjectSchedule;

SELECT * FROM Academic.Subject;
SELECT * FROM Academic.SubjectClassroom;
SELECT * FROM Academic.SubjectDetail;
SELECT * FROM Academic.GradeRevision;

SELECT * 
FROM Academic.SubjectSchedule SS
JOIN Academic.SubjectDetail SD ON SS.SubjectDetailID = SD.SubjectDetailID
--WHERE SubjectID = 29
ORDER BY SS.SubjectDetailID;
SELECT * FROM Academic.Weekday;

SELECT  SS.SubjectScheduleID, S.SubjectID, W.Name, SS.StartTime, SS.EndTime, SD.ProfessorID, SD.Section, SD.Trimester,
	S.SubjectCode, S.Name, S.Credits, P.FirstName, P.MiddleName, P.FirstSurname, P.SecondSurname
FROM Academic.SubjectSchedule SS
	JOIN Academic.SubjectDetail SD on SS.SubjectDetailID = SD.SubjectDetailID
	JOIN ACADEMIC.Subject S ON SD.SubjectId = S.SubjectId
	JOIN Person.Person P ON SD.ProfessorID = P.PersonID
	JOIN Academic.Weekday W ON SS.WeekdayID = W.WeekdayID
WHERE S.Name LIKE '%%' OR
	S.SubjectCode LIKE '%INS%'
ORDER BY SS.SubjectScheduleID;

--WHERE P.PersonID = 1110201
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

--EXEC Person.SP_PersonRegistration 
--	@DocNo			= '40231024361', 
--	@FirstName		= 'Nikita', 
--	@MiddleName		= 'Alekseevich', 
--	@FirstSurname	= 'Kravchenko', 
--	@Gender			= 'M', 
--	@BirthDate		= '1998-10-12', 
--	@Email			= 'android.oct7@gmail.com';
--GO

--EXEC Person.SP_PersonRegistration 
--	@DocNo			= '98765432109', 
--	@FirstName		= 'Ramón', 
--	@FirstSurname	= 'Ramírez', 
--	@Gender			= 'M', 
--	@BirthDate		= '2000-01-02', 
--	@Email			= 'ran.32@gmail.com';
--GO
