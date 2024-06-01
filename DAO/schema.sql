-- Create Themes table
CREATE TABLE Themes (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Description VARCHAR(MAX) NULL,
    PrimaryHex VARCHAR(100) NULL
);

-- Create Persons table
CREATE TABLE Persons (
    PersonID INT IDENTITY(1,1) PRIMARY KEY,
    FirstName VARCHAR(50) NOT NULL,
    LastName VARCHAR(50) NOT NULL,
    Email VARCHAR(100) NOT NULL,
    Username VARCHAR(100) NOT NULL,
    PasswordHash VARCHAR(MAX) NOT NULL,
    PhoneNumber VARCHAR(20) NULL,
    ThemeId INT DEFAULT 1 NULL,
    FOREIGN KEY (ThemeId) REFERENCES Themes(Id)
);

-- Create Employees table
CREATE TABLE Employees (
    EmployeeID INT IDENTITY(1,1) PRIMARY KEY,
    IsAdmin BIT DEFAULT 0 NOT NULL,
    PersonID INT NULL,
    FOREIGN KEY (PersonID) REFERENCES Persons(PersonID)
);

-- Stored procedure to delete from Employees
CREATE PROCEDURE spDelete_Employees
    @EmployeeID INT
AS
BEGIN
    DELETE FROM Employees WHERE EmployeeID = @EmployeeID;
END;

-- Stored procedure to delete from Persons
CREATE PROCEDURE spDelete_Persons
    @PersonID INT
AS
BEGIN
    DELETE FROM Persons WHERE PersonID = @PersonID;
END;

-- Stored procedure to get specific Employee
CREATE PROCEDURE spGet_Employees
    @EmployeeID INT
AS
BEGIN
    SELECT * FROM Employees WHERE EmployeeID = @EmployeeID;
END;

-- Stored procedure to get specific Person
CREATE PROCEDURE spGet_Persons
    @PersonID INT
AS
BEGIN
    SELECT * FROM Persons WHERE PersonID = @PersonID;
END;

-- Stored procedure to get specific Theme
CREATE PROCEDURE spGet_Themes
    @ThemeId INT
AS
BEGIN
    SELECT * FROM Themes WHERE Id = @ThemeId;
END;

-- Stored procedure to get all Employees
CREATE PROCEDURE spGetAll_Employees
AS
BEGIN
    SELECT * FROM Employees;
END;

-- Stored procedure to get all Persons
CREATE PROCEDURE spGetAll_Persons
AS
BEGIN
    SELECT a.*, b.Description AS ThemeDescription, b.PrimaryHex AS ThemeHex 
    FROM Persons a 
    LEFT JOIN Themes b ON a.ThemeId = b.Id;
END;

-- Stored procedure to get all Themes
CREATE PROCEDURE spGetAll_Themes
AS
BEGIN
    SELECT * FROM Themes;
END;

-- Stored procedure to insert into Employees
CREATE PROCEDURE spInsert_Employees
    @IsAdmin BIT,
    @PersonID INT
AS
BEGIN
    INSERT INTO Employees (IsAdmin, PersonID)
    VALUES (@IsAdmin, @PersonID);
END;

-- Stored procedure to insert into Persons
CREATE PROCEDURE spInsert_Persons
    @FirstName VARCHAR(50),
    @LastName VARCHAR(50),
    @Username VARCHAR(100),
    @PasswordHash VARCHAR(MAX),
    @Email VARCHAR(100),
    @PhoneNumber VARCHAR(20),
    @ThemeId INT
AS
BEGIN
    INSERT INTO Persons (FirstName, LastName, Email, Username, PasswordHash, PhoneNumber, ThemeId)
    VALUES (@FirstName, @LastName, @Email, @Username, @PasswordHash, @PhoneNumber, @ThemeId);
END;

-- Stored procedure to update Employees
CREATE PROCEDURE spUpdate_Employees
    @EmployeeID INT,
    @IsAdmin BIT,
    @PersonID INT
AS
BEGIN
    UPDATE Employees
    SET IsAdmin = @IsAdmin, PersonID = @PersonID
    WHERE EmployeeID = @EmployeeID;
END;

-- Stored procedure to update Persons
CREATE PROCEDURE spUpdate_Persons
    @PersonID INT,
    @FirstName VARCHAR(50),
    @LastName VARCHAR(50),
    @Username VARCHAR(100),
    @PasswordHash VARCHAR(MAX),
    @Email VARCHAR(100),
    @PhoneNumber VARCHAR(20),
    @ThemeId INT
AS
BEGIN
    UPDATE Persons
    SET FirstName = @FirstName, LastName = @LastName, Email = @Email, Username = @Username, PasswordHash = @PasswordHash, PhoneNumber = @PhoneNumber, ThemeId = @ThemeId
    WHERE PersonID = @PersonID;
END;

-- Insert sample data into Persons and Employees
INSERT INTO Themes (Description, PrimaryHex)
VALUES
('Default Theme', '#000000'),
('Blue Theme', '#0000FF'),
('Green Theme', '#00FF00');

INSERT INTO Persons (FirstName, LastName, Email, Username, PasswordHash, PhoneNumber, ThemeId)
VALUES
('John', 'Doe', 'john.doe@example.com', 'johndoe', 'ac9689e2272427085e35b9d3e3e8bed88cb3434828b43b86fc0596cad4c6e270', '123456789', 1),
('Jane', 'Smith', 'jane.smith@example.com', 'janesmith', 'ac9689e2272427085e35b9d3e3e8bed88cb3434828b43b86fc0596cad4c6e270', '987654321', 2),
('Michael', 'Johnson', 'michael.johnson@example.com', 'michaelj', 'ac9689e2272427085e35b9d3e3e8bed88cb3434828b43b86fc0596cad4c6e270', '555666777', 3),
('Emily', 'Brown', 'emily.brown@example.com', 'emilyb', 'ac9689e2272427085e35b9d3e3e8bed88cb3434828b43b86fc0596cad4c6e270', NULL, 1),
('David', 'Davis', 'david.davis@example.com', 'davidd', 'ac9689e2272427085e35b9d3e3e8bed88cb3434828b43b86fc0596cad4c6e270', '333222111', 3);

EXEC spInsert_Employees 1, 1;

