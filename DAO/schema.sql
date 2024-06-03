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
    Img VARBINARY(MAX) NULL,
    FOREIGN KEY (ThemeId) REFERENCES Themes(Id)
);

-- Create Employees table
CREATE TABLE Employees (
    EmployeeID INT IDENTITY(1,1) PRIMARY KEY,
    IsAdmin BIT DEFAULT 0 NOT NULL,
    PersonID INT NULL,
    FOREIGN KEY (PersonID) REFERENCES Persons(PersonID)
);
