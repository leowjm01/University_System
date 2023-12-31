USE [UniversitySystem-ba1be054-e24d-4118-92db-465948ebef30]
GO
/****** Object:  StoredProcedure [dbo].[GetPaginatedTeachers]    Script Date: 12/26/2023 10:31:56 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetPaginatedTeachers]
@TeacherName nvarchar(50) = NULL,
@PageNum int,
@PageSize int

AS

BEGIN
    SET NOCOUNT ON;

    DECLARE @Offset INT = (@PageNum - 1) * @PageSize;

    SELECT teacherId,teacherName,email,gender,IsDeleted
    FROM Teachers WITH (NOLOCK)
    WHERE (@TeacherName IS NULL OR teacherName  LIKE '%' + @TeacherName + '%') 
	AND IsDeleted = 'false'
    ORDER BY teacherID
    
	OFFSET @Offset ROWS
    FETCH NEXT @PageSize ROWS ONLY;
END
GO
