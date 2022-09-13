USE ProjectIndiaCharlie;

DECLARE @Path NVARCHAR(MAX);
DECLARE @FileLoc NVARCHAR(MAX);
DECLARE @SQL_BULK VARCHAR(MAX);
--SET @Path = 'C:\Users\Nikita\Desktop\Projects\ProjectIndiaCharlie\SQL\';--Path to folder of your pc for bulk insert script
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
  FIRSTROW = 2,
  CODEPAGE = ''ACP''
);' --  
EXEC(@SQL_BULK);

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
	('Saturday');
GO

INSERT INTO Academic.Career(Name, Code, Subjects, Credits, Year, IsActive)
VALUES ('Medicina', 'MED', 204, 425, 2020, 1),
('Ingeniería de Software', 'IDS', 111, 279, 2020, 1),
('Ingeniería Mecánica', 'MEC', 112, 280, 2020, 1);
GO


SELECT * FROM Person.Person;
SELECT * FROM Person.PersonPassword;

SELECT * FROM Academic.Career;
SELECT * FROM Academic.Classroom;
SELECT * FROM Academic.Grade;
SELECT * FROM Academic.Professor;
SELECT * FROM Academic.Student;
SELECT * FROM Academic.StudentSubject;


SELECT * FROM Academic.Subject;
SELECT * FROM Academic.SubjectClassroom;
SELECT * FROM Academic.SubjectDetail;
SELECT * FROM Academic.SubjectSchedule;
SELECT * FROM Academic.Weekday;



SELECT *
FROM Academic.SubjectSchedule SS
JOIN Academic.SubjectDetail SD on SD.SubjectID = SS.SubjectDetailID
JOIN ACADEMIC.Subject S ON SD.SubjectId = S.SubjectId
JOIN Person.Person P ON SD.ProfessorID = P.PersonID
WHERE S.Name LIKE '%vector%';

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
