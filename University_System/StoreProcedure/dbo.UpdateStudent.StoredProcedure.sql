USE [UniversitySystem-ba1be054-e24d-4118-92db-465948ebef30]
GO
/****** Object:  StoredProcedure [dbo].[UpdateStudent]    Script Date: 12/26/2023 10:31:56 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UpdateStudent]
@studentId int,
@studentName [nvarchar](50),
@email [nvarchar](50),
@gender int

AS

BEGIN
	UPDATE dbo.Students WITH (ROWLOCK)
    SET
		studentName = @studentName,
		email = @email,
		gender = @gender
	WHERE studentId = @studentId
END
GO
