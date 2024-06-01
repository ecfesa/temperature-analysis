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

INSERT INTO Employees (IsAdmin, PersonID)
VALUES (1, 1);
