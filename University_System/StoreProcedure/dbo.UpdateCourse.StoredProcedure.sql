USE [UniversitySystem-ba1be054-e24d-4118-92db-465948ebef30]
GO
/****** Object:  StoredProcedure [dbo].[UpdateCourse]    Script Date: 12/26/2023 10:31:56 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UpdateCourse]
@courseId int,
@courseName [nvarchar](50),
@teacherId int

AS

BEGIN
	UPDATE dbo.Courses WITH (ROWLOCK)
    SET
		courseName = @courseName,
		teacherId = @teacherId
	WHERE courseId = @courseId
END
GO
