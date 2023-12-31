USE [UniversitySystem-ba1be054-e24d-4118-92db-465948ebef30]
GO
/****** Object:  StoredProcedure [dbo].[UpdateTeacher]    Script Date: 12/26/2023 10:31:56 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UpdateTeacher]
@teacherId int,
@teacherName [nvarchar](50),
@email [nvarchar](50),
@gender int

AS

BEGIN
	UPDATE dbo.Teachers WITH (ROWLOCK)
    SET
		teacherName = @teacherName,
		email = @email,
		gender = @gender
	WHERE teacherId = @teacherId
END
GO
