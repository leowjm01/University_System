USE [UniversitySystem-ba1be054-e24d-4118-92db-465948ebef30]
GO
/****** Object:  StoredProcedure [dbo].[GetScoreResultByStudentName]    Script Date: 12/26/2023 10:31:56 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetScoreResultByStudentName]
@studentName [nvarchar] (50)

AS

BEGIN
	SELECT
		sr.scoreResultId,
		sr.mark,
		sr.grade,
		c.courseId,
		c.courseName,
		c.IsDeleted,
		s.studentId,
		s.studentName,
		s.gender,
		s.email
	FROM dbo.ScoreResults sr WITH (NOLOCK)
	INNER JOIN dbo.Courses c on sr.courseId = c.courseId
	INNER JOIN dbo.Students s on sr.studentId = s.studentId
	where s.studentName Like '%' + @studentName + '%' 
	AND sr.IsDeleted = 'False'
END
GO
