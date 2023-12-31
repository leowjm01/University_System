USE [UniversitySystem-ba1be054-e24d-4118-92db-465948ebef30]
GO
/****** Object:  StoredProcedure [dbo].[GetAllScoreResultsByStudentId]    Script Date: 12/26/2023 10:31:56 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetAllScoreResultsByStudentId]
@studentId int

AS

BEGIN
	SELECT
		scoreResultId,
		mark,
		grade,
		studentId,
		courseId,
		IsDeleted
	FROM dbo.ScoreResults WITH (NOLOCK)
	where studentId = @studentId
END
GO
