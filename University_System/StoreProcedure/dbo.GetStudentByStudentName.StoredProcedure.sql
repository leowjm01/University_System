USE [UniversitySystem-ba1be054-e24d-4118-92db-465948ebef30]
GO
/****** Object:  StoredProcedure [dbo].[GetStudentByStudentName]    Script Date: 12/26/2023 10:31:56 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetStudentByStudentName]
@studentName [nvarchar](50)

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
	where studentName Like '%' + @studentName + '%' 
	AND IsDeleted = 'false'
END
GO
