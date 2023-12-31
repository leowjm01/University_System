USE [UniversitySystem-ba1be054-e24d-4118-92db-465948ebef30]
GO
/****** Object:  StoredProcedure [dbo].[GetPaginatedCourses]    Script Date: 12/26/2023 10:31:56 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetPaginatedCourses]
@CourseName nvarchar(50) = NULL,
@PageNum int,
@PageSize int

AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Offset INT = (@PageNum - 1) * @PageSize;

    SELECT c.courseId,c.courseName,
		   t.teacherId,t.teacherName,t.gender,t.email,t.IsDeleted
    FROM Courses c WITH (NOLOCK)
	INNER JOIN dbo.Teachers t on c.teacherId = t.teacherId
    WHERE (@CourseName  IS NULL OR c.courseName  LIKE '%' + @CourseName  + '%') 
	AND c.IsDeleted = 'false'
    ORDER BY courseId
    OFFSET @Offset ROWS
    FETCH NEXT @PageSize ROWS ONLY;
END
GO
