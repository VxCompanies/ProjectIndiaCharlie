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
	('Saturday');

----Register cordinadores y  careras
--Medicina
EXEC [Person].[SP_RegisterPerson]
	@DocNo = N'38564444658',
	@FirstName = N'María',
	@MiddleName = N'Nicole',
	@FirstSurname = N'Tejada',
	@SecondSurname = N'Diaz',
	@Gender = N'f',
	@BirthDate = '1978-04-02',
	@Email = N'Miguelrodrig@mail.net',
	@RolId = 2,
	@PasswordHash = N'e14ceeffc3107c5956645fe09232515ed7d2af3048eea37e2571bd340c7ef05a',
	@PasswordSalt = N'jj331'
GO
EXEC Academic.SP_AddCareer
	@CoordinatorID	= 1110201,
	@Name			= 'Medicina',
	@Code			= 'MED',
	@Subjects		= 204,
	@Credits		= 425,
	@Year			= 2020,
	@IsActive		= 1;  
GO

--Software
EXEC [Person].[SP_RegisterPerson]
	@DocNo = N'12345678901',
	@FirstName = N'Roger',
	@FirstSurname = N'Ramiro',
	@Gender = N'm',
	@BirthDate = '1988-02-10',
	@Email = N'ramiro@mail.com',
	@RolId = 2,
	@PasswordHash = N'fdbfd43cca1aed1691d4bd7821d700449d53fd84f419462de98c6504da136687',
	@PasswordSalt = N'8ui37'
GO

EXEC Academic.SP_AddCareer
	@CoordinatorID	= 1110202,
	@Name			= 'Ingeniería de Software',
	@Code			= 'IDS',
	@Subjects		= 111,
	@Credits		= 279,
	@Year			= 2020,
	@IsActive		= 1;  
GO

--Ing mecanica
EXEC [Person].[SP_RegisterPerson]
	@DocNo = N'12987308901',
	@FirstName = N'Oliver ',
	@FirstSurname = N'Ledesma',
	@Gender = N'm',
	@BirthDate = '1979-11-9',
	@Email = N'OliverLedesma@mail.com',
	@RolId = 2,
	@PasswordHash = N'fdbfd43cca1aed1691d4bd7821d700449d53fd84f419462de98c6504da136687',
	@PasswordSalt = N'8ui37'
GO

EXEC Academic.SP_AddCareer
	@CoordinatorID	= 1110203,
	@Name			= 'Ingeniería Mecánica',
	@Code			= 'MEC',
	@Subjects		= 112,
	@Credits		= 280,
	@Year			= 2020,
	@IsActive		= 1;  
GO

----Registrar estudiantes

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
--Agregar rol de profe
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



SELECT * FROM Person.Person;
SELECT * FROM Person.PersonPassword;

SELECT * FROM Academic.Career;
SELECT * FROM Academic.Classroom;
SELECT * FROM Academic.Grade;
SELECT * FROM Academic.Professor;
SELECT * FROM Academic.Schedule;
SELECT * FROM Academic.Section;
SELECT * FROM Academic.Student;
SELECT * FROM Academic.Subject;
SELECT * FROM Academic.SubjectClassroom;
SELECT * FROM Academic.SubjectStudent;
SELECT * FROM Academic.Weekday;

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
