USE [UniversitySystem-ba1be054-e24d-4118-92db-465948ebef30]
GO
/****** Object:  StoredProcedure [dbo].[DeleteScoreresult]    Script Date: 12/26/2023 10:31:56 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteScoreresult]
@scoreResultId int

AS

BEGIN
	UPDATE dbo.ScoreResults WITH (ROWLOCK)
    SET
		IsDeleted = 1
	WHERE scoreResultId = @scoreResultId
END
GO
