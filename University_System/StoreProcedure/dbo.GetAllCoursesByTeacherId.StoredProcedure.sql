USE [UniversitySystem-ba1be054-e24d-4118-92db-465948ebef30]
GO
/****** Object:  StoredProcedure [dbo].[GetAllCoursesByTeacherId]    Script Date: 12/26/2023 10:31:56 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetAllCoursesByTeacherId]
@teacherId int

AS

BEGIN
	SELECT
		courseId,
		courseName,
		teacherId,
		IsDeleted
	FROM dbo.Courses WITH (NOLOCK)
	where teacherId = @teacherId
END
GO
