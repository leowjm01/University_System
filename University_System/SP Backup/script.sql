USE [UniversitySystem-ba1be054-e24d-4118-92db-465948ebef30]
GO
/****** Object:  StoredProcedure [dbo].[AddNewCourse]    Script Date: 12/26/2023 10:29:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[AddNewCourse]
@courseName [nvarchar](50),
@teacherId int

AS

BEGIN
	INSERT INTO dbo.Courses
		(
			courseName,
			teacherId,
			IsDeleted
		)
    VALUES
		(
			@courseName,
			@teacherId,
			0
		)
END
GO
/****** Object:  StoredProcedure [dbo].[AddNewScoreResult]    Script Date: 12/26/2023 10:29:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[AddNewScoreResult]
@mark decimal(6,2),
@grade [nvarchar] (50),
@courseId int,
@studentId int

AS

BEGIN
	INSERT INTO dbo.ScoreResults
		(
			mark,
			grade,
			courseId,
			studentId,
			IsDeleted
		)
    VALUES
		(
			@mark,
			@grade,
			@courseId,
			@studentId,
			0
		)
END
GO
/****** Object:  StoredProcedure [dbo].[AddNewStudent]    Script Date: 12/26/2023 10:29:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[AddNewStudent]
@studentName [nvarchar](50),
@email [nvarchar] (50),
@gender int

AS
BEGIN
	INSERT INTO dbo.Students
		(
			studentName,
			email,
			gender,
			examSelected,
			IsDeleted
		)

    VALUES
		(
			@studentName,
			@email,
			@gender,
			0,
			0
		)
END
GO
/****** Object:  StoredProcedure [dbo].[AddNewTeacher]    Script Date: 12/26/2023 10:29:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[AddNewTeacher]
@teacherName [nvarchar](50),
@email [nvarchar] (50),
@gender int

AS

BEGIN
	INSERT INTO dbo.Teachers
		(
			teacherName,
			email,
			gender,
			IsDeleted
		)

    VALUES
		(
			@teacherName,
			@email,
			@gender,
			0
		)
END
GO
/****** Object:  StoredProcedure [dbo].[CheckExamSelected]    Script Date: 12/26/2023 10:29:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[CheckExamSelected]
@studentId int

AS

BEGIN
	SELECT
		sr.scoreResultId,
		sr.mark,
		sr.grade,
		c.courseId,
		c.courseName,
		c.IsDeleted,
		s.studentId,
		s.studentName,
		s.gender,
		s.email
	FROM dbo.ScoreResults sr WITH (NOLOCK)
	INNER JOIN dbo.Courses c on sr.courseId = c.courseId
	INNER JOIN dbo.Students s on sr.studentId = s.studentId
	where sr.studentId = @studentId AND sr.IsDeleted = 'False' 
	AND (mark < 50 OR mark IS NULL) AND c.IsDeleted = 'False'
END
GO
/****** Object:  StoredProcedure [dbo].[DeleteCourse]    Script Date: 12/26/2023 10:29:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteCourse]
@courseId int

AS

BEGIN
	UPDATE dbo.Courses WITH (ROWLOCK)
    SET
		IsDeleted = 1
	WHERE courseId = @courseId
END
GO
/****** Object:  StoredProcedure [dbo].[DeleteScoreresult]    Script Date: 12/26/2023 10:29:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteScoreresult]
@scoreResultId int

AS

BEGIN
	UPDATE dbo.ScoreResults WITH (ROWLOCK)
    SET
		IsDeleted = 1
	WHERE scoreResultId = @scoreResultId
END
GO
/****** Object:  StoredProcedure [dbo].[DeleteScoreResultByStudentId]    Script Date: 12/26/2023 10:29:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteScoreResultByStudentId]
@studentId int

AS

BEGIN
	UPDATE dbo.ScoreResults WITH (ROWLOCK)
    SET
		IsDeleted = 1
	WHERE studentId = @studentId
END
GO
/****** Object:  StoredProcedure [dbo].[DeleteStudent]    Script Date: 12/26/2023 10:29:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteStudent]
@studentId int

AS

BEGIN
	UPDATE dbo.Students WITH (ROWLOCK)
    SET
		IsDeleted = 1
	WHERE studentId = @studentId
END
GO
/****** Object:  StoredProcedure [dbo].[DeleteTeacher]    Script Date: 12/26/2023 10:29:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteTeacher]
@teacherId int

AS

BEGIN
	UPDATE dbo.Teachers WITH (ROWLOCK)
    SET
		IsDeleted = 1
	WHERE teacherId = @teacherId
END
GO
/****** Object:  StoredProcedure [dbo].[GetAllCourses]    Script Date: 12/26/2023 10:29:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetAllCourses]

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
	where c.IsDeleted = 'False'
END
GO
/****** Object:  StoredProcedure [dbo].[GetAllCoursesByTeacherId]    Script Date: 12/26/2023 10:29:14 AM ******/
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
/****** Object:  StoredProcedure [dbo].[GetAllCoursesIncludeDelete]    Script Date: 12/26/2023 10:29:14 AM ******/
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
/****** Object:  StoredProcedure [dbo].[GetAllResults]    Script Date: 12/26/2023 10:29:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetAllResults]

AS

BEGIN
	SELECT
		sr.scoreResultId,
		sr.mark,
		sr.grade,
		c.courseId,
		c.courseName,
		c.IsDeleted,
		s.studentId,
		s.studentName,
		s.gender,
		s.email
	FROM dbo.ScoreResults sr WITH (NOLOCK)
	INNER JOIN dbo.Courses c on sr.courseId = c.courseId
	INNER JOIN dbo.Students s on sr.studentId = s.studentId
	where sr.IsDeleted = 'False'
END
GO
/****** Object:  StoredProcedure [dbo].[GetAllScoreResultsByStudentId]    Script Date: 12/26/2023 10:29:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetAllScoreResultsByStudentId]
@studentId int

AS

BEGIN
	SELECT
		scoreResultId,
		mark,
		grade,
		studentId,
		courseId,
		IsDeleted
	FROM dbo.ScoreResults WITH (NOLOCK)
	where studentId = @studentId
END
GO
/****** Object:  StoredProcedure [dbo].[GetAllStudents]    Script Date: 12/26/2023 10:29:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetAllStudents]
AS

BEGIN
	SELECT 		
		studentId,
		studentName,
		gender,
		email,
		examSelected,
		IsDeleted
	FROM dbo.Students WITH (NOLOCK) 
	Where IsDeleted = 'False';
END
GO
/****** Object:  StoredProcedure [dbo].[GetAllStudentsIncludeDelete]    Script Date: 12/26/2023 10:29:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetAllStudentsIncludeDelete]
AS

BEGIN
	SELECT 		
		studentId,
		studentName,
		gender,
		email,
		examSelected,
		IsDeleted
	FROM dbo.Students WITH (NOLOCK) 
END
GO
/****** Object:  StoredProcedure [dbo].[GetAllTeachers]    Script Date: 12/26/2023 10:29:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetAllTeachers]
AS

BEGIN

	SELECT 		
		teacherId,
		teacherName,
		gender,
		email,
		IsDeleted
	FROM dbo.Teachers WITH (NOLOCK)
	Where IsDeleted = 'False';

END
GO
/****** Object:  StoredProcedure [dbo].[GetCourseByCourseId]    Script Date: 12/26/2023 10:29:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetCourseByCourseId]
@courseId int

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
	where c.courseId = @courseId
END
GO
/****** Object:  StoredProcedure [dbo].[GetCourseByCourseName]    Script Date: 12/26/2023 10:29:14 AM ******/
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
/****** Object:  StoredProcedure [dbo].[GetCourseByStudentId]    Script Date: 12/26/2023 10:29:14 AM ******/
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
/****** Object:  StoredProcedure [dbo].[GetPaginatedCourses]    Script Date: 12/26/2023 10:29:14 AM ******/
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
/****** Object:  StoredProcedure [dbo].[GetPaginatedCoursesByTeacherId]    Script Date: 12/26/2023 10:29:14 AM ******/
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
/****** Object:  StoredProcedure [dbo].[GetPaginatedScoreResult]    Script Date: 12/26/2023 10:29:14 AM ******/
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
/****** Object:  StoredProcedure [dbo].[GetPaginatedScoreResultByStudentId]    Script Date: 12/26/2023 10:29:14 AM ******/
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
/****** Object:  StoredProcedure [dbo].[GetPaginatedStudents]    Script Date: 12/26/2023 10:29:14 AM ******/
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
/****** Object:  StoredProcedure [dbo].[GetPaginatedTeachers]    Script Date: 12/26/2023 10:29:14 AM ******/
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
/****** Object:  StoredProcedure [dbo].[GetScoreResultByScoreResultID]    Script Date: 12/26/2023 10:29:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetScoreResultByScoreResultID]
@scoreResultId int

AS

BEGIN
	SELECT
		sr.scoreResultId,
		sr.mark,
		sr.grade,
		s.studentId,
		s.studentName,
		s.gender,
		s.email,
		c.courseId,
		c.courseName,
		c.IsDeleted
	FROM dbo.ScoreResults sr WITH (NOLOCK)
	INNER JOIN dbo.Students s on sr.studentId = s.studentId 
	INNER JOIN dbo.Courses c on sr.courseId = c.courseId
	where sr.scoreResultId = @scoreResultId
END
GO
/****** Object:  StoredProcedure [dbo].[GetScoreResultByStudentId]    Script Date: 12/26/2023 10:29:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetScoreResultByStudentId]
@studentId int
AS
BEGIN
	SELECT
		sr.scoreResultId,
		sr.mark,
		sr.grade,
		c.courseId,
		c.courseName,
		c.IsDeleted,
		s.studentId,
		s.studentName,
		s.gender,
		s.email
	FROM dbo.ScoreResults sr
	INNER JOIN dbo.Courses c on sr.courseId = c.courseId
	INNER JOIN dbo.Students s on sr.studentId = s.studentId
	where s.studentId = @studentId AND sr.IsDeleted = 'False'
END
GO
/****** Object:  StoredProcedure [dbo].[GetScoreResultByStudentName]    Script Date: 12/26/2023 10:29:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetScoreResultByStudentName]
@studentName [nvarchar] (50)

AS

BEGIN
	SELECT
		sr.scoreResultId,
		sr.mark,
		sr.grade,
		c.courseId,
		c.courseName,
		c.IsDeleted,
		s.studentId,
		s.studentName,
		s.gender,
		s.email
	FROM dbo.ScoreResults sr WITH (NOLOCK)
	INNER JOIN dbo.Courses c on sr.courseId = c.courseId
	INNER JOIN dbo.Students s on sr.studentId = s.studentId
	where s.studentName Like '%' + @studentName + '%' 
	AND sr.IsDeleted = 'False'
END
GO
/****** Object:  StoredProcedure [dbo].[GetStudentByStudentId]    Script Date: 12/26/2023 10:29:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetStudentByStudentId]
@studentId int

AS

BEGIN
	SELECT
		studentId,
		studentName,
		gender,
		email,
		examSelected,
		IsDeleted
	FROM dbo.Students WITH (NOLOCK)
	where studentId = @studentId
END
GO
/****** Object:  StoredProcedure [dbo].[GetStudentByStudentName]    Script Date: 12/26/2023 10:29:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetStudentByStudentName]
@studentName [nvarchar](50)

AS

BEGIN
	SELECT
		studentId,
		studentName,
		gender,
		email,
		examSelected,
		IsDeleted
	FROM dbo.Students WITH (NOLOCK)
	where studentName Like '%' + @studentName + '%' 
	AND IsDeleted = 'false'
END
GO
/****** Object:  StoredProcedure [dbo].[GetTeacherByTeacherID]    Script Date: 12/26/2023 10:29:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetTeacherByTeacherID]
@teacherId int

AS

BEGIN
	SELECT
		teacherId,
		teacherName,
		gender,
		email,
		IsDeleted
	FROM dbo.Teachers WITH (NOLOCK)
	where teacherId = @teacherId
END
GO
/****** Object:  StoredProcedure [dbo].[GetTeacherByTeacherName]    Script Date: 12/26/2023 10:29:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetTeacherByTeacherName]
@teacherName [nvarchar](50)

AS

BEGIN
	SELECT
		teacherId,
		teacherName,
		gender,
		email,
		IsDeleted
	FROM dbo.Teachers WITH (NOLOCK)
	where teacherName Like '%' + @teacherName + '%' 
	AND IsDeleted = 'false'
END
GO
/****** Object:  StoredProcedure [dbo].[UpdateCourse]    Script Date: 12/26/2023 10:29:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UpdateCourse]
@courseId int,
@courseName [nvarchar](50),
@teacherId int

AS

BEGIN
	UPDATE dbo.Courses WITH (ROWLOCK)
    SET
		courseName = @courseName,
		teacherId = @teacherId
	WHERE courseId = @courseId
END
GO
/****** Object:  StoredProcedure [dbo].[UpdateScoreResult]    Script Date: 12/26/2023 10:29:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UpdateScoreResult]
@scoreResultId int,
@mark decimal(6,2),
@grade [nvarchar] (50),
@coursId int,
@studentId int
AS
BEGIN
	UPDATE dbo.ScoreResults WITH (ROWLOCK)
    SET
		mark = @mark,
		grade = @grade,
		courseId = @coursId,
		studentId = @studentId
	WHERE scoreResultId = @scoreResultId
END
GO
/****** Object:  StoredProcedure [dbo].[UpdateSelectedExamByStudentId]    Script Date: 12/26/2023 10:29:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UpdateSelectedExamByStudentId]
@studentId int,
@examSelected int

AS
BEGIN
	UPDATE dbo.Students WITH (ROWLOCK)
    SET
		examSelected = @examSelected
	WHERE studentId = @studentId
END
GO
/****** Object:  StoredProcedure [dbo].[UpdateStudent]    Script Date: 12/26/2023 10:29:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UpdateStudent]
@studentId int,
@studentName [nvarchar](50),
@email [nvarchar](50),
@gender int

AS

BEGIN
	UPDATE dbo.Students WITH (ROWLOCK)
    SET
		studentName = @studentName,
		email = @email,
		gender = @gender
	WHERE studentId = @studentId
END
GO
/****** Object:  StoredProcedure [dbo].[UpdateTeacher]    Script Date: 12/26/2023 10:29:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UpdateTeacher]
@teacherId int,
@teacherName [nvarchar](50),
@email [nvarchar](50),
@gender int

AS

BEGIN
	UPDATE dbo.Teachers WITH (ROWLOCK)
    SET
		teacherName = @teacherName,
		email = @email,
		gender = @gender
	WHERE teacherId = @teacherId
END
GO
