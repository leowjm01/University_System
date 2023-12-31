USE [UniversitySystem-ba1be054-e24d-4118-92db-465948ebef30]
GO
/****** Object:  StoredProcedure [dbo].[AddNewStudent]    Script Date: 12/26/2023 10:31:56 AM ******/
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
