USE [UniversitySystem-ba1be054-e24d-4118-92db-465948ebef30]
GO
/****** Object:  StoredProcedure [dbo].[GetScoreResultByScoreResultID]    Script Date: 12/26/2023 10:31:56 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetScoreResultByScoreResultID]
@scoreResultId int

AS

BEGIN
	SELECT
		sr.scoreResultId,
		sr.mark,
		sr.grade,
		s.studentId,
		s.studentName,
		s.gender,
		s.email,
		c.courseId,
		c.courseName,
		c.IsDeleted
	FROM dbo.ScoreResults sr WITH (NOLOCK)
	INNER JOIN dbo.Students s on sr.studentId = s.studentId 
	INNER JOIN dbo.Courses c on sr.courseId = c.courseId
	where sr.scoreResultId = @scoreResultId
END
GO
