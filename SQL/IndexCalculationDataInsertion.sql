USE ProjectIndiaCharlie;

SELECT * FROM Academic.Student
WHERE PersonID = 1111666;

SELECT * FROM Academic.Subject
WHERE Name like '%anal%';

SELECT * FROM Academic.SubjectDetail
WHERE SubjectID = 1195;
--326 vectorial | 919 Fisica  2 | 850 Dise;o de software | 529 big data

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

SELECT * FROM Academic.F_GetStudentsSchedule (1111666);