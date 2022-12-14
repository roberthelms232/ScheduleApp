USE scheduleappDB
GO

--
--Structure for Roles Table
--

CREATE TABLE Roles (
RoleID int IDENTITY(1,1) PRIMARY KEY
,Name nvarchar(50) NOT NULL --Name of the role
,IsAdmin bit NOT NULL
,CanEditProfile bit NOT NULL
,Users tinyint NOT NULL
,Roles tinyint NOT NULL
,Rooms tinyint NOT NULL
,Classes tinyint NOT NULL
)
GO

--
--Structure for Users Table
--

CREATE TABLE Users(
UserID int IDENTITY(1,1) PRIMARY KEY
,FirstName nvarchar(50) NOT NULL
,LastName nvarchar(50) NOT NULL
,Email nvarchar(191) NOT NULL
,Password nvarchar(191) NOT NULL
,RoleID int NOT NULL CONSTRAINT User_Role FOREIGN KEY REFERENCES Roles(RoleID)
)
GO

--
--Structure for Rooms Table
--

CREATE TABLE Rooms (
RoomNum int IDENTITY(1,1) PRIMARY KEY
,BuildingNum int NOT NULL
)
GO

--
--Structure for Classes Table
--

CREATE TABLE Classes (
ClassID int IDENTITY(1,1) PRIMARY KEY
,Name nvarchar(50) NOT NULL
,RoomNum int NOT NULL CONSTRAINT Class_Room FOREIGN KEY REFERENCES Rooms(RoomNum)
,BuildingNum int NOT NULL
,StartTime datetime NOT NULL
,EndTime datetime NOT NULL
,ProfName nvarchar(50) NOT NULL
)
GO

--
-- Necessary data import for Roles and Users tables
--

SET IDENTITY_INSERT Roles ON

INSERT INTO Roles (RoleID, [Name],
	IsAdmin,CanEditProfile, Users, Roles, Rooms,Classes) VALUES
	(1, 'Admin', 1,1,15,15,15,15)
SET IDENTITY_INSERT Roles OFF

SET IDENTITY_INSERT Users ON

INSERT INTO Users (UserID, FirstName,LastName,Email,Password,RoleID) VALUES
(1,'Robert','Helms','roberthelms@isu.edu','tester',1)
SET IDENTITY_INSERT Users OFF