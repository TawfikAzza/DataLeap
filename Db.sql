-- Create the database
CREATE DATABASE IF NOT EXISTS ExpenseTracker;
USE ExpenseTracker;

-- Create User table
CREATE TABLE User (
                      Id BINARY(16) NOT NULL PRIMARY KEY,
                      Name VARCHAR(255) NOT NULL,
                      PhoneNumber VARCHAR(20)
);

-- Create Group table
CREATE TABLE `Group` (
                         Id BINARY(16) NOT NULL PRIMARY KEY,
                         Name VARCHAR(255) NOT NULL
);

-- Create Expense table
CREATE TABLE Expense (
                         Id BINARY(16) NOT NULL PRIMARY KEY,
                         Title VARCHAR(255) NOT NULL,
                         Amount DECIMAL(10, 2) NOT NULL,
                         Date DATE NOT NULL
);

-- Create Rel_User_Expense table without constraints
CREATE TABLE Rel_User_Expense (
                                  ID_User BINARY(16) NOT NULL,
                                  ID_Expense BINARY(16) NOT NULL
);

-- Create Rel_Expense_Group table without constraints
CREATE TABLE Rel_Expense_Group (
                                   ID_Expense BINARY(16) NOT NULL,
                                   ID_Group BINARY(16) NOT NULL
);

-- Create Rel_User_Group table without constraints
CREATE TABLE Rel_User_Group (
                                ID_User BINARY(16) NOT NULL,
                                ID_Group BINARY(16) NOT NULL
);


-- Insert Users with specific UUIDs
INSERT INTO User (Id, Name, PhoneNumber) VALUES
                                             (UUID_TO_BIN('a3bb189e-8bf9-3888-9912-ace4e6543001'), 'Alice', '123-456-7890'),
                                             (UUID_TO_BIN('d2a4dd28-9bb4-11ea-bb37-0242ac130001'), 'Bob', '234-567-8901'),
                                             (UUID_TO_BIN('d2a4df90-9bb4-11ea-bb37-0242ac130002'), 'Charlie', '345-678-9012'),
                                             (UUID_TO_BIN('d2a4e1de-9bb4-11ea-bb37-0242ac130003'), 'David', '456-789-0123'),
                                             (UUID_TO_BIN('d2a4e3ec-9bb4-11ea-bb37-0242ac130004'), 'Eve', '567-890-1234'),
                                             (UUID_TO_BIN('d2a4e5a2-9bb4-11ea-bb37-0242ac130005'), 'Frank', '678-901-2345'),
                                             (UUID_TO_BIN('d2a4e75a-9bb4-11ea-bb37-0242ac130006'), 'Grace', '789-012-3456'),
                                             (UUID_TO_BIN('d2a4e928-9bb4-11ea-bb37-0242ac130007'), 'Hank', '890-123-4567'),
                                             (UUID_TO_BIN('d2a4eae2-9bb4-11ea-bb37-0242ac130008'), 'Ivy', '901-234-5678'),
                                             (UUID_TO_BIN('d2a4ec98-9bb4-11ea-bb37-0242ac130009'), 'Jack', '012-345-6789');

-- Insert Groups with specific UUIDs
INSERT INTO `Group` (Id, Name) VALUES
                                   (UUID_TO_BIN('f47ac10b-58cc-4372-a567-0e02b2c3d479'), 'Group A'),
                                   (UUID_TO_BIN('f47ac10b-58cc-4372-a567-0e02b2c3d480'), 'Group B'),
                                   (UUID_TO_BIN('f47ac10b-58cc-4372-a567-0e02b2c3d481'), 'Group C'),
                                   (UUID_TO_BIN('f47ac10b-58cc-4372-a567-0e02b2c3d482'), 'Group D');
-- Insert Expenses with specific UUIDs
INSERT INTO Expense (Id, Title, Amount, Date) VALUES
                                                  (UUID_TO_BIN('8c7daa2e-9bb4-11ea-bb37-0242ac130002'), 'Expense 1', 100.00, '2023-01-01'),
                                                  (UUID_TO_BIN('8c7dabd2-9bb4-11ea-bb37-0242ac130003'), 'Expense 2', 200.00, '2023-01-02'),
                                                  (UUID_TO_BIN('8c7dacec-9bb4-11ea-bb37-0242ac130004'), 'Expense 3', 150.00, '2023-01-03'),
                                                  (UUID_TO_BIN('8c7dae22-9bb4-11ea-bb37-0242ac130005'), 'Expense 4', 50.00, '2023-01-04'),
                                                  (UUID_TO_BIN('8c7daee6-9bb4-11ea-bb37-0242ac130006'), 'Expense 5', 300.00, '2023-01-05'),
                                                  (UUID_TO_BIN('8c7dafc0-9bb4-11ea-bb37-0242ac130007'), 'Expense 6', 75.00, '2023-01-06'),
                                                  (UUID_TO_BIN('8c7db0ba-9bb4-11ea-bb37-0242ac130008'), 'Expense 7', 400.00, '2023-01-07'),
                                                  (UUID_TO_BIN('8c7db1e4-9bb4-11ea-bb37-0242ac130009'), 'Expense 8', 250.00, '2023-01-08'),
                                                  (UUID_TO_BIN('8c7db28a-9bb4-11ea-bb37-0242ac130010'), 'Expense 9', 500.00, '2023-01-09'),
                                                  (UUID_TO_BIN('8c7db322-9bb4-11ea-bb37-0242ac130011'), 'Expense 10', 125.00, '2023-01-10');
-- Populate Rel_User_Group table
INSERT INTO Rel_User_Group (ID_User, ID_Group) VALUES
                                                   (UUID_TO_BIN('a3bb189e-8bf9-3888-9912-ace4e6543001'), UUID_TO_BIN('f47ac10b-58cc-4372-a567-0e02b2c3d479')),
                                                   (UUID_TO_BIN('d2a4dd28-9bb4-11ea-bb37-0242ac130001'), UUID_TO_BIN('f47ac10b-58cc-4372-a567-0e02b2c3d479')),
                                                   (UUID_TO_BIN('d2a4df90-9bb4-11ea-bb37-0242ac130002'), UUID_TO_BIN('f47ac10b-58cc-4372-a567-0e02b2c3d480')),
                                                   (UUID_TO_BIN('d2a4e1de-9bb4-11ea-bb37-0242ac130003'), UUID_TO_BIN('f47ac10b-58cc-4372-a567-0e02b2c3d480')),
                                                   (UUID_TO_BIN('d2a4e3ec-9bb4-11ea-bb37-0242ac130004'), UUID_TO_BIN('f47ac10b-58cc-4372-a567-0e02b2c3d481')),
                                                   (UUID_TO_BIN('d2a4e5a2-9bb4-11ea-bb37-0242ac130005'), UUID_TO_BIN('f47ac10b-58cc-4372-a567-0e02b2c3d481')),
                                                   (UUID_TO_BIN('d2a4e75a-9bb4-11ea-bb37-0242ac130006'), UUID_TO_BIN('f47ac10b-58cc-4372-a567-0e02b2c3d482')),
                                                   (UUID_TO_BIN('d2a4e928-9bb4-11ea-bb37-0242ac130007'), UUID_TO_BIN('f47ac10b-58cc-4372-a567-0e02b2c3d482')),
                                                   (UUID_TO_BIN('d2a4eae2-9bb4-11ea-bb37-0242ac130008'), UUID_TO_BIN('f47ac10b-58cc-4372-a567-0e02b2c3d482')),
                                                   (UUID_TO_BIN('d2a4ec98-9bb4-11ea-bb37-0242ac130009'), UUID_TO_BIN('f47ac10b-58cc-4372-a567-0e02b2c3d479'));

-- Populate Rel_User_Expense table
INSERT INTO Rel_User_Expense (ID_User, ID_Expense) VALUES
                                                       (UUID_TO_BIN('a3bb189e-8bf9-3888-9912-ace4e6543001'), UUID_TO_BIN('8c7daa2e-9bb4-11ea-bb37-0242ac130002')),
                                                       (UUID_TO_BIN('d2a4dd28-9bb4-11ea-bb37-0242ac130001'), UUID_TO_BIN('8c7daa2e-9bb4-11ea-bb37-0242ac130002')),
                                                       (UUID_TO_BIN('d2a4df90-9bb4-11ea-bb37-0242ac130002'), UUID_TO_BIN('8c7dabd2-9bb4-11ea-bb37-0242ac130003')),
                                                       (UUID_TO_BIN('d2a4e1de-9bb4-11ea-bb37-0242ac130003'), UUID_TO_BIN('8c7dabd2-9bb4-11ea-bb37-0242ac130003')),
                                                       (UUID_TO_BIN('d2a4e3ec-9bb4-11ea-bb37-0242ac130004'), UUID_TO_BIN('8c7dacec-9bb4-11ea-bb37-0242ac130004'));

-- Populate Rel_Expense_Group table
INSERT INTO Rel_Expense_Group (ID_Expense, ID_Group) VALUES
                                                         (UUID_TO_BIN('8c7daa2e-9bb4-11ea-bb37-0242ac130002'), UUID_TO_BIN('f47ac10b-58cc-4372-a567-0e02b2c3d479')),
                                                         (UUID_TO_BIN('8c7dabd2-9bb4-11ea-bb37-0242ac130003'), UUID_TO_BIN('f47ac10b-58cc-4372-a567-0e02b2c3d479')),
                                                         (UUID_TO_BIN('8c7dacec-9bb4-11ea-bb37-0242ac130004'), UUID_TO_BIN('f47ac10b-58cc-4372-a567-0e02b2c3d480')),
                                                         (UUID_TO_BIN('8c7dae22-9bb4-11ea-bb37-0242ac130005'), UUID_TO_BIN('f47ac10b-58cc-4372-a567-0e02b2c3d480')),
                                                         (UUID_TO_BIN('8c7daee6-9bb4-11ea-bb37-0242ac130006'), UUID_TO_BIN('f47ac10b-58cc-4372-a567-0e02b2c3d481'));
-- Add foreign key constraints to Rel_User_Expense table
ALTER TABLE Rel_User_Expense
    ADD CONSTRAINT fk_user_expense_user FOREIGN KEY (ID_User) REFERENCES User(Id),
ADD CONSTRAINT fk_user_expense_expense FOREIGN KEY (ID_Expense) REFERENCES Expense(Id);

-- Add foreign key constraints to Rel_Expense_Group table
ALTER TABLE Rel_Expense_Group
    ADD CONSTRAINT fk_expense_group_expense FOREIGN KEY (ID_Expense) REFERENCES Expense(Id),
ADD CONSTRAINT fk_expense_group_group FOREIGN KEY (ID_Group) REFERENCES `Group`(Id);

-- Add foreign key constraints to Rel_User_Group table
ALTER TABLE Rel_User_Group
    ADD CONSTRAINT fk_user_group_user FOREIGN KEY (ID_User) REFERENCES User(Id),
ADD CONSTRAINT fk_user_group_group FOREIGN KEY (ID_Group) REFERENCES `Group`(Id);

