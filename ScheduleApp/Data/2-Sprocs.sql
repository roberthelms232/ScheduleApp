USE scheduleappDB
GO

--
--
--

--
--SPROCS for Users
--

CREATE PROCEDURE dbo.sproc_UserAdd(
@UserID int OUTPUT
,@FirstName nvarchar(50)
,@LastName nvarchar(50)
,@Email nvarchar(191)
,@Password nvarchar(191)
,@RoleID int
)
AS
BEGIN
	INSERT INTO Users(FirstName,LastName,Email,Password,RoleID)
		VALUES(@FirstName,@LastName,@Email,@Password,@RoleID)
	SET @UserID = @@IDENTITY
END
GO

GRANT EXECUTE ON dbo.sproc_UserAdd TO db_writer
GO

CREATE PROCEDURE dbo.sproc_UserUpdate
@UserID int OUTPUT
,@FirstName nvarchar(50)
,@LastName nvarchar(50)
,@Email nvarchar(191)
,@Password nvarchar(191)
,@RoleID int
AS
BEGIN
	UPDATE Users
		SET
			FirstName = @FirstName
			,LastName = @LastName
			,Email = @Email
			,[Password] = @Password
			,RoleID = @RoleID
		WHERE UserID = @UserID
END
GO

GRANT EXECUTE ON dbo.sproc_UserUpdate TO db_writer
GO

CREATE PROCEDURE dbo.sproc_UserGet
@UserID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * FROM Users
		WHERE UserID = @UserID
END
GO

GRANT EXECUTE ON dbo.sproc_UserGet TO db_reader
GO

CREATE PROCEDURE DBO.sproc_UsersGetAll
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * FROM Users
END
GO

GRANT EXECUTE ON dbo.sproc_UserGetAll TO db_reader
GO

--
--SPROCS for Roles
--

CREATE PROCEDURE dbo.sproc_RoleAdd
@RoleID int OUTPUT
,@Name nvarchar(50)
,@IsAdmin bit
,@CanEditProfile bit
,@Users tinyint
,@Roles tinyint
,@Rooms tinyint
,@Classes tinyint
AS
BEGIN
	INSERT INTO Roles([Name],IsAdmin,CanEditProfile,Roles,Users,Rooms,Classes)
		VALUES(@Name,@IsAdmin,@CanEditProfile,@Roles,@Users, @Rooms,@Classes)
	SET @RoleID = @@IDENTITY
END
GO

GRANT EXECUTE ON dbo.sproc_RoleAdd TO db_writer
GO

CREATE PROCEDURE dbo.sproc_RoleUpdate
@RoleID int OUTPUT
,@Name nvarchar(50)
,@IsAdmin bit
,@CanEditProfile bit
,@Users tinyint
,@Roles tinyint
,@Rooms tinyint
,@Classes tinyint
AS
BEGIN
	UPDATE Roles
		SET
			[Name] = @Name
			,IsAdmin = @IsAdmin
			,CanEditProfile = @CanEditProfile
			,Users = @Users
			,Roles = @Roles
			,Classes = @Classes
		WHERE RoleID = @RoleID
END
GO

GRANT EXECUTE ON dbo.sproc_RoleUpdate TO db_Writer
GO

CREATE PROCEDURE dbo.sproc_RoleGet
@RoleID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT* FROM Roles
		WHERE RoleID = @RoleID
END
GO

GRANT EXECUTE ON dbo.sproc_RoleGet TO db_reader
GO

CREATE PROCEDURE dbo.sproc_RolesGetAll
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * FROM Roles
END
GO

GRANT EXECUTE ON dbo.sproc_RolesGetAll TO db_reader
GO

--
--Sprocs for Rooms
--

CREATE PROCEDURE dbo.sproc_RoomAdd
@RoomNum int OUTPUT
,@BuildingNum int
AS
BEGIN
	INSERT INTO Rooms(BuildingNum) VALUES(@BuildingNum)
	SET @RoomNum = @@IDENTITY
END
GO

GRANT EXECUTE ON dbo.sproc_RoomAdd TO db_writer
GO


CREATE PROCEDURE dbo.sproc_RoomUpdate
@RoomNum int OUTPUT
,@BuildingNum int
AS
BEGIN
	UPDATE Rooms
		SET
			BuildingNum = @BuildingNum
		WHERE RoomNum = @RoomNum
END
GO

GRANT EXECUTE ON dbo.sproc_RoomUpdate TO db_writer
GO

CREATE PROCEDURE dbo.sproc_RoomGet
@RommNum int OUTPUT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * FROM Rooms
		WHERE RoomNum = @RoomNum
END
GO

GRANT EXECUTE ON dbo.sproc_RoomGet TO db_reader
GO


CREATE PROCEDURE dbo.sproc_RoomGetAll
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * FROM Rooms
END
GO

GRANT EXECUTE ON dbo.sproc_RoomGetAll TO db_reader
GO

--
--Sprocs for Classes
--

CREATE PROCEDURE dbo.sproc_ClassAdd
@ClassID int OUTPUT
,@Name nvarchar(50)
,@RoomNum int
,@BuildingNum int
,@StartTime datetime
,@EndTime datetime
,@ProfName nvarchar(50)
AS
BEGIN
	INSERT INTO Classes([Name],RoomNum,BuildingNum,StartTime,Endtime,ProfName)
		VALUES(@Name,@RoomNum,@BuildingNum,@StartTime,@EndTime,@ProfName)
	SET @ClassID = @@IDENTITY
END
GO

GRANT EXECUTE ON dbo.sproc_ClassAdd TO db_writer
GO

CREATE PROCEDURE dbo.sproc_ClassUpdate
@ClassID int OUTPUT
,@Name nvarchar(50)
,@RoomNum int
,@BuildingNum int
,@StartTime datetime
,@EndTime datetime
,@ProfName nvarchar(50)
AS
BEGIN
	UPDATE Classes
		SET
			[Name] = @Name
			,RoomNum = @RoomNum
			,BuildingNum = @BuildingNum
			,StartTime = @StartTime
			,EndTime = @EndTime
			,ProfName = @ProfName
		WHERE ClassID = @ClassID
END
GO

GRANT EXECUTE ON dbo.sproc_ClassUpdate TO db_writer
GO

CREATE PROCEDURE dbo.sproc_ClassGet
@ClassID int OUTPUT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * FROM Classes
		WHERE ClassID = @ClassID
END
GO

GRANT EXECUTE ON dbo.sproc_ClassUpdate TO db_reader
GO

CREATE PROCEDURE dbo.sproc_ClassGetAll
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * FROM Classes
END
GO

GRANT EXECUTE ON dbo.sproc_ClassGetAll TO db_reader
GO