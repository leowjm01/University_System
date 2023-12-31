USE [UniversitySystem-ba1be054-e24d-4118-92db-465948ebef30]
GO
/****** Object:  StoredProcedure [dbo].[UpdateSelectedExamByStudentId]    Script Date: 12/26/2023 10:31:56 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UpdateSelectedExamByStudentId]
@studentId int,
@examSelected int

AS
BEGIN
	UPDATE dbo.Students WITH (ROWLOCK)
    SET
		examSelected = @examSelected
	WHERE studentId = @studentId
END
GO
