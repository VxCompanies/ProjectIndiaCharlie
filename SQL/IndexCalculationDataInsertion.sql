USE ProjectIndiaCharlie;
BEGIN TRAN;

SELECT * FROM Academic.Student
WHERE PersonID = 1111666;

SELECT * FROM Academic.Subject
WHERE Name like '%s%';

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


EXEC Academic.SP_SubjectSelection
	@SubjectDetailId = 2,
	@StudentId	= 1110408;
EXEC Academic.SP_SubjectSelection
	@SubjectDetailId = 1,
	@StudentId	= 1110408;
EXEC Academic.SP_SubjectSelection
	@SubjectDetailId = 2,
	@StudentId	= 1110409;

EXEC Academic.SP_PublishGrade
@SubjectDetailID = 2,
@StudentID = 1110408,
@GradeId = 2;
EXEC Academic.SP_PublishGrade
@SubjectDetailID = 1,
@StudentID = 1110408,
@GradeId = 8;
EXEC Academic.SP_PublishGrade
@SubjectDetailID = 2,
@StudentID = 1110409,
@GradeId = 8;


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
GO


CREATE OR ALTER FUNCTION dbo.IsZero(
@number int)
RETURNS INT
AS
BEGIN
	IF (@number = 0)
		return 1
	return @number
END;
GO

Create or alter PROCEDURE Academic.SP_UpdateStudentIndex
@StudentID int,
@GeneralIndex decimal (3,2),
@TrimestralIndex decimal (3,2)
AS
BEGIN
	UPDATE Academic.Student
	SET GeneralIndex = @GeneralIndex, TrimestralIndex = @TrimestralIndex
	WHERE PersonID = @StudentID
	RETURN 1
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
where YEAR = @Year AND Trimester = @Trimester

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

	IF EXISTS (SELECT * FROM Academic.IndexHistory WHERE PersonID = @StudentID AND Year = @Year and Trimester = @Trimester)
		BEGIN
		UPDATE 	Academic.IndexHistory
		SET CreditsTrimester = @TrimesterCredits, CreditsSumm = @AccumulatedCredits, PointsTrimester = @TrimesterPoints,PontsSumm =  @AccumulatedPoints, 
				TrimesteralIndex = @TrimestralIndex, GeneralIndex = @TrimestralIndex
		WHERE PersonID = @StudentID AND Year = @Year and Trimester = @Trimester
		END
	ELSE
		BEGIN
		INSERT INTO Academic.IndexHistory(	PersonID, CareerID, Year, Trimester, 
				CreditsTrimester, CreditsSumm, PointsTrimester,PontsSumm , 
				TrimesteralIndex, GeneralIndex)
		VALUES(	@StudentID, @StudentCareer, @Year, @Trimester,
				@TrimesterCredits, @AccumulatedCredits, @TrimesterPoints, @AccumulatedPoints, 
				@TrimestralIndex, @GeneralIndex)
		END
	EXEC Academic.SP_UpdateStudentIndex
		@StudentID = @StudentID,
		@GeneralIndex = @GeneralIndex,
		@TrimestralIndex = @TrimestralIndex;

	DELETE TOP(1)  FROM #studentTemp;
	SET @StudentQuantity = @StudentQuantity - 1;
END
DROP TABLE #studentTemp;
GO


SELECT * FROM Academic.F_GetStudentsSchedule (1111666, 2022, 3) GSS;
SELECT * FROM Academic.F_GetStudentsSchedule (1110408, 2022, 3) GSS;

EXEC Academic.SP_CalculateIndexByTrimester
@Year = 2022,
@Trimester = 3;


SELECT * FROM Academic.IndexHistory
ORDER BY PersonID;
SELECT * FROM Academic.StudentSubject

SELECT * FROM Academic.Student
WHERE GeneralIndex != 0



