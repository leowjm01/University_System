USE [UniversitySystem-ba1be054-e24d-4118-92db-465948ebef30]
GO
/****** Object:  StoredProcedure [dbo].[GetTeacherByTeacherID]    Script Date: 12/26/2023 10:31:56 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetTeacherByTeacherID]
@teacherId int

AS

BEGIN
	SELECT
		teacherId,
		teacherName,
		gender,
		email,
		IsDeleted
	FROM dbo.Teachers WITH (NOLOCK)
	where teacherId = @teacherId
END
GO
