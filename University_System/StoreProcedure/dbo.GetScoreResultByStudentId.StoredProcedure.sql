USE [UniversitySystem-ba1be054-e24d-4118-92db-465948ebef30]
GO
/****** Object:  StoredProcedure [dbo].[GetScoreResultByStudentId]    Script Date: 12/26/2023 10:31:56 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetScoreResultByStudentId]
@studentId int
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
	FROM dbo.ScoreResults sr
	INNER JOIN dbo.Courses c on sr.courseId = c.courseId
	INNER JOIN dbo.Students s on sr.studentId = s.studentId
	where s.studentId = @studentId AND sr.IsDeleted = 'False'
END
GO
