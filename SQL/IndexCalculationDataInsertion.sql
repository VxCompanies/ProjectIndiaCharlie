USE ProjectIndiaCharlie;
BEGIN TRAN;

SELECT * FROM Academic.Student
WHERE PersonID = 1111666;

SELECT * FROM Academic.Subject
WHERE Name like '%anal%';

SELECT * FROM Academic.SubjectDetail
WHERE SubjectID = 1195;
--326 vectorial | 919 Fisica  2 | 850 Dise;o de software | 529 big data

--Student 1111666 Seleccion
EXEC Academic.SP_SubjectSelection
@SubjectDetailID = 326,
@StudentID = 1111666;

EXEC Academic.SP_SubjectSelection
@SubjectDetailID = 919,
@StudentID = 1111666;

EXEC Academic.SP_SubjectSelection
@SubjectDetailID = 850,
@StudentID = 1111666;

EXEC Academic.SP_SubjectSelection
@SubjectDetailID = 529,
@StudentID = 1111666;

SELECT * FROM Academic.F_GetStudentsSchedule (1111666, 2022, 3);

--Student 1111666 Publicacion
EXEC Academic.SP_PublishGrade
@SubjectDetailID = 326,
@StudentID = 1111666,
@GradeId = 8;

EXEC Academic.SP_PublishGrade
@SubjectDetailID = 919,
@StudentID = 1111666,
@GradeId = 2;

EXEC Academic.SP_PublishGrade
@SubjectDetailID = 850,
@StudentID = 1111666,
@GradeId = 1;

EXEC Academic.SP_PublishGrade
@SubjectDetailID = 529,
@StudentID = 1111666,
@GradeId = 6;

SELECT * 
FROM Academic.IndexHistory;

SELECT * FROM Academic.F_GetStudentsSchedule (1111666, 2022, 3) GSS
JOIN Academic.Grade G ON GSS.GradeID = G.GradeID;

SELECT * FROM Academic.Grade;
GO


CREATE OR ALTER PROCEDURE SP_CalculateIndexByTrimester
@Year int,
@Trimester int
AS
DECLARE @StudentQuantity int

--Select Estudiantes que cursaban materias en el trimestre
SELECT DISTINCT(SS.StudentID)
INTO #studentTemp
from Academic.StudentSubject SS
JOIN Academic.SubjectDetail SD ON SD.SubjectDetailID = SS.SubjectDetailID
where YEAR = 2022 AND Trimester = 3

SET @StudentQuantity = (SELECT COUNT(StudentID) FROM #studentTemp);
PRINT @StudentQuantity;
SELECT TOP(1) StudentID FROM #studentTemp;

DELETE TOP(1)  FROM #studentTemp;
SELECT TOP(1) StudentID FROM #studentTemp;

DROP TABLE #studentTemp;

ROLLBACK