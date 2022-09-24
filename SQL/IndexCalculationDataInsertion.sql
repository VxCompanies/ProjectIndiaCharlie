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

--
EXEC Academic.SP_RequestGradeRevision
@SubjectDetailID = 529,
@StudentID = 1111666;

SELECT * FROM Academic.GradeRevision

EXEC Academic.SP_ProcessGradeRevision
@SubjectDetailID = 529,
@ModifiedGradeID = 3,
@AdminID = 1112200,
@StudentID = 1111666;

BEGIN TRAN 
UPDATE Academic.GradeRevision
		SET ModifiedGradeID = 3, AdminID = 1122, ProfessorID = @ProfessorID, DateModified = GETDATE()
		WHERE SubjectDetailID = @SubjectDetailID and PersonId = @StudentID
		PRINT('updated grade revision')
ROLLBACK

CREATE OR ALTER FUNCTION dbo.IsZero(
@number int)
RETURNS INT
AS
BEGIN
	IF (@number = 0)
		return 1
	return @number
END
GO



CREATE OR ALTER PROCEDURE Academic.SP_CalculateIndexByTrimester
@Year int,
@Trimester int
AS
DECLARE @StudentQuantity int,
@StudentID int,
@StudentCareer int,
@AccumulatedCredits float,
@AccumulatedPoints float,
@TrimesterCredits float,
@TrimesterPoints float,
@TrimestralIndex decimal(3, 2),
@GeneralIndex decimal(3, 2)

--Select Estudiantes que cursaban materias en el trimestre
SELECT DISTINCT(SS.StudentID)
INTO #studentTemp
from Academic.StudentSubject SS
JOIN Academic.SubjectDetail SD ON SD.SubjectDetailID = SS.SubjectDetailID
where YEAR = 2022 AND Trimester = 3

SET @StudentQuantity = (SELECT COUNT(StudentID) FROM #studentTemp);
PRINT @StudentQuantity;

WHILE @StudentQuantity > 0
BEGIN
	SET @StudentID = (SELECT TOP(1) StudentID FROM #studentTemp);
	SET @StudentCareer = (SELECT TOP(1) CareerID FROM Academic.Student WHERE PersonID = @StudentID);

	SET @TrimesterCredits = (SELECT ISNULL(SUM(GSS.Credits),0) FROM Academic.F_GetStudentsSchedule (@StudentID, @Year, @Trimester) GSS WHERE GSS.Grade != 'R');
	SET @TrimesterPoints = (SELECT ISNULL(SUM(GSS.Points), 0) FROM Academic.F_GetStudentsSchedule (@StudentID, @Year, @Trimester) GSS);

	SET @AccumulatedCredits = (	SELECT ISNULL(SUM(CreditsTrimester), 0)
								FROM Academic.IndexHistory 
								WHERE PersonID = @StudentID AND CareerID = @StudentCareer) + @TrimesterCredits;
	SET @AccumulatedPoints  = (	SELECT ISNULL(SUM(PointsTrimester), 0)
								FROM Academic.IndexHistory 
								WHERE PersonID = @StudentID AND CareerID = @StudentCareer) + @TrimesterPoints;
	
	SET @TrimestralIndex = (@TrimesterPoints/  dbo.IsZero(@TrimesterCredits));
	SET @GeneralIndex = (@AccumulatedPoints/ dbo.IsZero(@AccumulatedCredits));

	INSERT INTO Academic.IndexHistory(	PersonID, CareerID, Year, Trimester, 
			CreditsTrimester, CreditsSumm, PointsTrimester,PontsSumm , 
			TrimesteralIndex, GeneralIndex)
	VALUES(	@StudentID, @StudentCareer, @Year, @Trimester,
			@TrimesterCredits, @AccumulatedCredits, @TrimesterPoints, @AccumulatedPoints, 
			@TrimestralIndex, @GeneralIndex)

	DELETE TOP(1)  FROM #studentTemp;
	SET @StudentQuantity = @StudentQuantity - 1;
END
DROP TABLE #studentTemp;
GO



BEGIN TRAN
EXEC Academic.SP_CalculateIndexByTrimester
@Year = 2022,
@Trimester = 3;

SELECT * FROM Academic.F_GetStudentsSchedule (1111666, 2022, 3) GSS;
SELECT * FROM Academic.F_GetStudentsSchedule (1110408, 2022, 3) GSS;


SELECT * FROM Academic.IndexHistory;

ROLLBACK

SELECT * FROM Academic.StudentSubject
