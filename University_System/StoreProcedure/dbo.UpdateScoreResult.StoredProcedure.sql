USE [UniversitySystem-ba1be054-e24d-4118-92db-465948ebef30]
GO
/****** Object:  StoredProcedure [dbo].[UpdateScoreResult]    Script Date: 12/26/2023 10:31:56 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UpdateScoreResult]
@scoreResultId int,
@mark decimal(6,2),
@grade [nvarchar] (50),
@coursId int,
@studentId int
AS
BEGIN
	UPDATE dbo.ScoreResults WITH (ROWLOCK)
    SET
		mark = @mark,
		grade = @grade,
		courseId = @coursId,
		studentId = @studentId
	WHERE scoreResultId = @scoreResultId
END
GO
