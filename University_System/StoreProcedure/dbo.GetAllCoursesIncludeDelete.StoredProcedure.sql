USE [UniversitySystem-ba1be054-e24d-4118-92db-465948ebef30]
GO
/****** Object:  StoredProcedure [dbo].[GetAllCoursesIncludeDelete]    Script Date: 12/26/2023 10:31:56 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetAllCoursesIncludeDelete]

AS

BEGIN
	SELECT
		c.courseId,
		c.courseName,
		t.teacherId,
		t.teacherName,
		t.gender,
		t.email,
		t.IsDeleted
	FROM dbo.Courses c WITH (NOLOCK) 
	INNER JOIN dbo.Teachers t on c.teacherId = t.teacherId
END
GO
