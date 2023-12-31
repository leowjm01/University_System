USE [UniversitySystem-ba1be054-e24d-4118-92db-465948ebef30]
GO
/****** Object:  StoredProcedure [dbo].[GetPaginatedScoreResult]    Script Date: 12/26/2023 10:31:56 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetPaginatedScoreResult]
@StudentName nvarchar(50) = NULL,
@PageNum int,
@PageSize int

AS

BEGIN
    SET NOCOUNT ON;

    DECLARE @Offset INT = (@PageNum - 1) * @PageSize;

    SELECT sr.scoreResultId,sr.mark,sr.grade,
		c.courseId,c.courseName,c.IsDeleted,
		s.studentId,s.studentName,s.gender,s.email
	FROM dbo.ScoreResults sr WITH (NOLOCK)
	INNER JOIN dbo.Courses c on sr.courseId = c.courseId
	INNER JOIN dbo.Students s on sr.studentId = s.studentId
	where (@StudentName IS NULL OR s.studentName Like '%' + @StudentName + '%') 
	AND sr.IsDeleted = 'False'
    ORDER BY scoreResultId
    
	OFFSET @Offset ROWS
    FETCH NEXT @PageSize ROWS ONLY;
END
GO
