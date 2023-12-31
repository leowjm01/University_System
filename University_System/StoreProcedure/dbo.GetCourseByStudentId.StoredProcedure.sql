USE [UniversitySystem-ba1be054-e24d-4118-92db-465948ebef30]
GO
/****** Object:  StoredProcedure [dbo].[GetCourseByStudentId]    Script Date: 12/26/2023 10:31:56 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetCourseByStudentId]
@studentId int

AS

BEGIN
SELECT
    c.courseId,
    c.courseName,
    c.teacherId,
    c.IsDeleted
FROM dbo.Courses c WITH (NOLOCK)
LEFT JOIN dbo.ScoreResults sr ON c.courseId = sr.courseId 
AND sr.studentId = @studentId
WHERE sr.scoreResultId IS NULL
AND c.IsDeleted = 'false';
END
GO
