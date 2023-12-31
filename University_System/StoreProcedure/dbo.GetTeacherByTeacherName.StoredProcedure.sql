USE [UniversitySystem-ba1be054-e24d-4118-92db-465948ebef30]
GO
/****** Object:  StoredProcedure [dbo].[GetTeacherByTeacherName]    Script Date: 12/26/2023 10:31:56 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetTeacherByTeacherName]
@teacherName [nvarchar](50)

AS

BEGIN
	SELECT
		teacherId,
		teacherName,
		gender,
		email,
		IsDeleted
	FROM dbo.Teachers WITH (NOLOCK)
	where teacherName Like '%' + @teacherName + '%' 
	AND IsDeleted = 'false'
END
GO
