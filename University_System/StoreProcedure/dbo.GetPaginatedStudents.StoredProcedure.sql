USE [UniversitySystem-ba1be054-e24d-4118-92db-465948ebef30]
GO
/****** Object:  StoredProcedure [dbo].[GetPaginatedStudents]    Script Date: 12/26/2023 10:31:56 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetPaginatedStudents]
@StudentName nvarchar(50) = NULL,
@PageNum int,
@PageSize int

AS

BEGIN
    SET NOCOUNT ON;

    DECLARE @Offset INT = (@PageNum - 1) * @PageSize;

    SELECT studentId,studentName,email,gender,examSelected,IsDeleted
    FROM Students WITH (NOLOCK)
    WHERE (@StudentName IS NULL OR studentName LIKE '%' + @StudentName + '%') 
	AND IsDeleted = 'false'
	ORDER BY studentId

    OFFSET @Offset ROWS
	FETCH NEXT @PageSize ROWS ONLY;
END
GO
