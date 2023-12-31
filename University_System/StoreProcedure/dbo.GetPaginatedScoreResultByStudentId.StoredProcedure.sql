USE [UniversitySystem-ba1be054-e24d-4118-92db-465948ebef30]
GO
/****** Object:  StoredProcedure [dbo].[GetPaginatedScoreResultByStudentId]    Script Date: 12/26/2023 10:31:56 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetPaginatedScoreResultByStudentId]
@StudentId int,
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
	where s.studentId = @studentId AND sr.IsDeleted = 'False'
    ORDER BY sr.scoreResultId

    OFFSET @Offset ROWS
    FETCH NEXT @PageSize ROWS ONLY;
END
GO
