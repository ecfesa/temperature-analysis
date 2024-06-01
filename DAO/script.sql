
USE [sqldb]
GO
/****** Object:  Table [dbo].[Employees]    Script Date: 31/05/2024 19:27:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Employees](
	[EmployeeID] [int] IDENTITY(1,1) NOT NULL,
	[IsAdmin] [bit] NOT NULL,
	[PersonID] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[EmployeeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Persons]    Script Date: 31/05/2024 19:27:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Persons](
	[PersonID] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [varchar](50) NOT NULL,
	[LastName] [varchar](50) NOT NULL,
	[Email] [varchar](100) NOT NULL,
	[Username] [varchar](100) NOT NULL,
	[PasswordHash] [varchar](max) NOT NULL,
	[PhoneNumber] [varchar](20) NULL,
	[ThemeId] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[PersonID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Themes]    Script Date: 31/05/2024 19:27:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Themes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Description] [varchar](max) NULL,
	[PrimaryHex] [varchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[Employees] ADD  DEFAULT ((0)) FOR [IsAdmin]
GO
ALTER TABLE [dbo].[Persons] ADD  DEFAULT ((1)) FOR [ThemeId]
GO
ALTER TABLE [dbo].[Employees]  WITH CHECK ADD FOREIGN KEY([PersonID])
REFERENCES [dbo].[Persons] ([PersonID])
GO
ALTER TABLE [dbo].[Persons]  WITH CHECK ADD FOREIGN KEY([ThemeId])
REFERENCES [dbo].[Themes] ([Id])
GO
/****** Object:  StoredProcedure [dbo].[spDelete_Employees]    Script Date: 31/05/2024 19:27:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Delete from Employees
CREATE PROCEDURE [dbo].[spDelete_Employees]
    @EmployeeID INT
AS
BEGIN
    DELETE FROM Employees WHERE EmployeeID = @EmployeeID;
END
GO
/****** Object:  StoredProcedure [dbo].[spDelete_Persons]    Script Date: 31/05/2024 19:27:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Delete from Persons
CREATE PROCEDURE [dbo].[spDelete_Persons]
    @PersonID INT
AS
BEGIN
    DELETE FROM Persons WHERE PersonID = @PersonID;
END
GO
/****** Object:  StoredProcedure [dbo].[spGet_Employees]    Script Date: 31/05/2024 19:27:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Get specific Employee
CREATE PROCEDURE [dbo].[spGet_Employees]
    @EmployeeID INT
AS
BEGIN
    SELECT * FROM Employees WHERE EmployeeID = @EmployeeID;
END
GO
/****** Object:  StoredProcedure [dbo].[spGet_Persons]    Script Date: 31/05/2024 19:27:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spGet_Persons]
    @PersonID INT
AS
BEGIN
    SELECT * FROM Persons WHERE PersonID = @PersonID;
END
GO
/****** Object:  StoredProcedure [dbo].[spGet_Themes]    Script Date: 31/05/2024 19:27:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Get specific Employee
create PROCEDURE [dbo].[spGet_Themes]
    @ThemeId INT
AS
BEGIN
    SELECT * FROM Themes WHERE ID = @ThemeId;
END
GO
/****** Object:  StoredProcedure [dbo].[spGetAll_Employees]    Script Date: 31/05/2024 19:27:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Get all Employees
CREATE PROCEDURE [dbo].[spGetAll_Employees]
AS
BEGIN
    SELECT * FROM Employees;
END
GO
/****** Object:  StoredProcedure [dbo].[spGetAll_Persons]    Script Date: 31/05/2024 19:27:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Get all Persons
CREATE PROCEDURE [dbo].[spGetAll_Persons]
AS
BEGIN
    SELECT a.*, b.Description as ThemeDescription, b.PrimaryHex as ThemeHex FROM Persons a LEFT JOIN Themes b on a.ThemeId = b.Id;
END
GO
/****** Object:  StoredProcedure [dbo].[spGetAll_Themes]    Script Date: 31/05/2024 19:27:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[spGetAll_Themes]
AS
BEGIN
	SELECT * FROM Themes
END
GO
/****** Object:  StoredProcedure [dbo].[spInsert_Employees]    Script Date: 31/05/2024 19:27:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


----------------------------------------------------------------------

-- Insert into Employees
CREATE PROCEDURE [dbo].[spInsert_Employees]
    @IsAdmin BIT,
    @PersonID INT
AS
BEGIN
    INSERT INTO Employees (IsAdmin, PersonID)
    VALUES (@IsAdmin, @PersonID);
END
GO
/****** Object:  StoredProcedure [dbo].[spInsert_Persons]    Script Date: 31/05/2024 19:27:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Insert into Persons
CREATE PROCEDURE [dbo].[spInsert_Persons]
    @FirstName VARCHAR(50),
    @LastName VARCHAR(50),
    @Username VARCHAR(50),
    @PasswordHash VARCHAR(MAX),
    @Email VARCHAR(100),
    @PhoneNumber VARCHAR(20),
	@ThemeId INT
AS
BEGIN
    INSERT INTO Persons (FirstName, LastName, Email, Username, PasswordHash, PhoneNumber, ThemeId)
    VALUES (@FirstName, @LastName,@Email, @Username, @PasswordHash, @PhoneNumber, @ThemeId);
END
GO
/****** Object:  StoredProcedure [dbo].[spUpdate_Employees]    Script Date: 31/05/2024 19:27:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Update Employees
CREATE PROCEDURE [dbo].[spUpdate_Employees]
    @EmployeeID INT,
    @IsAdmin BIT,
    @PersonID INT
AS
BEGIN
    UPDATE Employees
    SET IsAdmin = @IsAdmin, PersonID = @PersonID
    WHERE EmployeeID = @EmployeeID;
END
GO
/****** Object:  StoredProcedure [dbo].[spUpdate_Persons]    Script Date: 31/05/2024 19:27:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Update Persons
CREATE PROCEDURE [dbo].[spUpdate_Persons]
    @PersonID INT,
    @FirstName VARCHAR(50),
    @LastName VARCHAR(50),
    @Username VARCHAR(50),
    @PasswordHash VARCHAR(50),
    @Email VARCHAR(100),
    @PhoneNumber VARCHAR(20),
	@ThemeId INT
AS
BEGIN
    UPDATE Persons
    SET FirstName = @FirstName, LastName = @LastName, Email = @Email, Username = @Username, PasswordHash = @PasswordHash, PhoneNumber = @PhoneNumber, ThemeId = @ThemeId
    WHERE PersonID = @PersonID;
END
GO

INSERT INTO Persons (FirstName, LastName, Email, Username, PasswordHash, PhoneNumber, ThemeId)
VALUES
('John', 'Doe', 'john.doe@example.com', 'johndoe', 'ac9689e2272427085e35b9d3e3e8bed88cb3434828b43b86fc0596cad4c6e270', '123456789', 1),
('Jane', 'Smith', 'jane.smith@example.com', 'janesmith', 'ac9689e2272427085e35b9d3e3e8bed88cb3434828b43b86fc0596cad4c6e270', '987654321', 2),
('Michael', 'Johnson', 'michael.johnson@example.com', 'michaelj', 'ac9689e2272427085e35b9d3e3e8bed88cb3434828b43b86fc0596cad4c6e270', '555666777', 3),
('Emily', 'Brown', 'emily.brown@example.com', 'emilyb', 'ac9689e2272427085e35b9d3e3e8bed88cb3434828b43b86fc0596cad4c6e270', NULL, 1),
('David', 'Davis', 'david.davis@example.com', 'davidd', 'ac9689e2272427085e35b9d3e3e8bed88cb3434828b43b86fc0596cad4c6e270', '333222111', 3);

EXEC spInsert_Employees 1,1
