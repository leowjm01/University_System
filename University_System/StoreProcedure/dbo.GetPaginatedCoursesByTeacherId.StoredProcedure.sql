USE [UniversitySystem-ba1be054-e24d-4118-92db-465948ebef30]
GO
/****** Object:  StoredProcedure [dbo].[GetPaginatedCoursesByTeacherId]    Script Date: 12/26/2023 10:31:56 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetPaginatedCoursesByTeacherId]
@TeacherId int,
@PageNum int,
@PageSize int

AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Offset INT = (@PageNum - 1) * @PageSize;

    SELECT c.courseId, c.courseName,
        t.teacherId, t.teacherName, t.gender, t.email, t.IsDeleted
    FROM Courses c WITH (NOLOCK)
    INNER JOIN dbo.Teachers t ON c.teacherId = t.teacherId
    WHERE t.teacherId = @TeacherId AND c.IsDeleted = 'false'
    ORDER BY c.courseId

    OFFSET @Offset ROWS
    FETCH NEXT @PageSize ROWS ONLY;
END
GO
