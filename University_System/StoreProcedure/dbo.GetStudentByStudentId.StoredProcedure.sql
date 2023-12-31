USE [UniversitySystem-ba1be054-e24d-4118-92db-465948ebef30]
GO
/****** Object:  StoredProcedure [dbo].[GetStudentByStudentId]    Script Date: 12/26/2023 10:31:56 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetStudentByStudentId]
@studentId int

AS

BEGIN
	SELECT
		studentId,
		studentName,
		gender,
		email,
		examSelected,
		IsDeleted
	FROM dbo.Students WITH (NOLOCK)
	where studentId = @studentId
END
GO
