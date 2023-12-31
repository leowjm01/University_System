USE [UniversitySystem-ba1be054-e24d-4118-92db-465948ebef30]
GO
/****** Object:  StoredProcedure [dbo].[GetCourseByCourseName]    Script Date: 12/26/2023 10:31:56 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetCourseByCourseName]
@courseName [nvarchar] (50)

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
	where c.courseName Like '%' + @courseName + '%' 
	AND c.IsDeleted = 'false'
END
GO
