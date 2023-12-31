USE [UniversitySystem-ba1be054-e24d-4118-92db-465948ebef30]
GO
/****** Object:  StoredProcedure [dbo].[AddNewScoreResult]    Script Date: 12/26/2023 10:31:56 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[AddNewScoreResult]
@mark decimal(6,2),
@grade [nvarchar] (50),
@courseId int,
@studentId int

AS

BEGIN
	INSERT INTO dbo.ScoreResults
		(
			mark,
			grade,
			courseId,
			studentId,
			IsDeleted
		)
    VALUES
		(
			@mark,
			@grade,
			@courseId,
			@studentId,
			0
		)
END
GO
