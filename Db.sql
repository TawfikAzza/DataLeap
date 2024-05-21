-- Create the database
CREATE DATABASE IF NOT EXISTS EZMoney_test;
USE EZMoney_test;

-- Create User table without constraints
CREATE TABLE User (
      id BINARY(16) NOT NULL PRIMARY KEY,
      name VARCHAR(255) NOT NULL,
      phone_number VARCHAR(20)
);

-- Create Group table without constraints
CREATE TABLE `Group` (
     id BINARY(16) NOT NULL PRIMARY KEY,
     name VARCHAR(255) NOT NULL,
     token VARCHAR(255) NOT NULL
);

-- Create Expense table without constraints
CREATE TABLE Expense (
     id BINARY(16) NOT NULL PRIMARY KEY,
     ownerID BINARY(16) NOT NULL,
     groupID BINARY(16) NOT NULL,
     title VARCHAR(255) NOT NULL,
     amount DECIMAL(10, 2) NOT NULL,
     date DATE NOT NULL
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

-- Insert 20 users with specific UUIDs
INSERT INTO User (id, name, phone_number) VALUES
      (UUID_TO_BIN('a3bb189e-8bf9-4a3c-9912-ace4e6543001'), 'Alice', '123-456-7890'),
      (UUID_TO_BIN('b3bb189e-8bf9-4a3c-9912-ace4e6543002'), 'Bob', '234-567-8901'),
      (UUID_TO_BIN('c3bb189e-8bf9-4a3c-9912-ace4e6543003'), 'Charlie', '345-678-9012'),
      (UUID_TO_BIN('d3bb189e-8bf9-4a3c-9912-ace4e6543004'), 'David', '456-789-0123'),
      (UUID_TO_BIN('e3bb189e-8bf9-4a3c-9912-ace4e6543005'), 'Eve', '567-890-1234'),
      (UUID_TO_BIN('f3bb189e-8bf9-4a3c-9912-ace4e6543006'), 'Frank', '678-901-2345'),
      (UUID_TO_BIN('a3bb189e-8bf9-4a3c-9912-ace4e6543007'), 'Grace', '789-012-3456'),
      (UUID_TO_BIN('b3bb189e-8bf9-4a3c-9912-ace4e6543008'), 'Hank', '890-123-4567'),
      (UUID_TO_BIN('c3bb189e-8bf9-4a3c-9912-ace4e6543009'), 'Ivy', '901-234-5678'),
      (UUID_TO_BIN('d3bb189e-8bf9-4a3c-9912-ace4e6543010'), 'Jack', '012-345-6789'),
      (UUID_TO_BIN('e3bb189e-8bf9-4a3c-9912-ace4e6543011'), 'Kathy', '123-456-7890'),
      (UUID_TO_BIN('f3bb189e-8bf9-4a3c-9912-ace4e6543012'), 'Leo', '234-567-8901'),
      (UUID_TO_BIN('a3bb189e-8bf9-4a3c-9912-ace4e6543013'), 'Mona', '345-678-9012'),
      (UUID_TO_BIN('b3bb189e-8bf9-4a3c-9912-ace4e6543014'), 'Nina', '456-789-0123'),
      (UUID_TO_BIN('c3bb189e-8bf9-4a3c-9912-ace4e6543015'), 'Oscar', '567-890-1234'),
      (UUID_TO_BIN('d3bb189e-8bf9-4a3c-9912-ace4e6543016'), 'Paul', '678-901-2345'),
      (UUID_TO_BIN('e3bb189e-8bf9-4a3c-9912-ace4e6543017'), 'Quinn', '789-012-3456'),
      (UUID_TO_BIN('f3bb189e-8bf9-4a3c-9912-ace4e6543018'), 'Rita', '890-123-4567'),
      (UUID_TO_BIN('a3bb189e-8bf9-4a3c-9912-ace4e6543019'), 'Sam', '901-234-5678'),
      (UUID_TO_BIN('b3bb189e-8bf9-4a3c-9912-ace4e6543020'), 'Tina', '012-345-6789');

-- Insert 4 groups with specific UUIDs
INSERT INTO `Group` (id, name, token) VALUES
      (UUID_TO_BIN('a1bb189e-8bf9-4a3c-9912-ace4e6544001'), 'Group A', 'tokenA'),
      (UUID_TO_BIN('b1bb189e-8bf9-4a3c-9912-ace4e6544002'), 'Group B', 'tokenB'),
      (UUID_TO_BIN('c1bb189e-8bf9-4a3c-9912-ace4e6544003'), 'Group C', 'tokenC'),
      (UUID_TO_BIN('d1bb189e-8bf9-4a3c-9912-ace4e6544004'), 'Group D', 'tokenD');

-- Insert 10 expenses with specific UUIDs
INSERT INTO Expense (id, ownerID, groupID, title, amount, date) VALUES
    (UUID_TO_BIN('e1bb189e-8bf9-4a3c-9912-ace4e6545001'), UUID_TO_BIN('a3bb189e-8bf9-4a3c-9912-ace4e6543001'), UUID_TO_BIN('a1bb189e-8bf9-4a3c-9912-ace4e6544001'), 'Expense 1', 100.00, '2023-01-01'),
    (UUID_TO_BIN('e2bb189e-8bf9-4a3c-9912-ace4e6545002'), UUID_TO_BIN('b3bb189e-8bf9-4a3c-9912-ace4e6543002'), UUID_TO_BIN('b1bb189e-8bf9-4a3c-9912-ace4e6544002'), 'Expense 2', 200.00, '2023-01-02'),
    (UUID_TO_BIN('e3bb189e-8bf9-4a3c-9912-ace4e6545003'), UUID_TO_BIN('c3bb189e-8bf9-4a3c-9912-ace4e6543003'), UUID_TO_BIN('c1bb189e-8bf9-4a3c-9912-ace4e6544003'), 'Expense 3', 150.00, '2023-01-03'),
    (UUID_TO_BIN('e4bb189e-8bf9-4a3c-9912-ace4e6545004'), UUID_TO_BIN('d3bb189e-8bf9-4a3c-9912-ace4e6543004'), UUID_TO_BIN('d1bb189e-8bf9-4a3c-9912-ace4e6544004'), 'Expense 4', 50.00, '2023-01-04'),
    (UUID_TO_BIN('e5bb189e-8bf9-4a3c-9912-ace4e6545005'), UUID_TO_BIN('e3bb189e-8bf9-4a3c-9912-ace4e6543005'), UUID_TO_BIN('a1bb189e-8bf9-4a3c-9912-ace4e6544001'), 'Expense 5', 300.00, '2023-01-05'),
    (UUID_TO_BIN('e6bb189e-8bf9-4a3c-9912-ace4e6545006'), UUID_TO_BIN('f3bb189e-8bf9-4a3c-9912-ace4e6543006'), UUID_TO_BIN('b1bb189e-8bf9-4a3c-9912-ace4e6544002'), 'Expense 6', 75.00, '2023-01-06'),
    (UUID_TO_BIN('e7bb189e-8bf9-4a3c-9912-ace4e6545007'), UUID_TO_BIN('a3bb189e-8bf9-4a3c-9912-ace4e6543007'), UUID_TO_BIN('c1bb189e-8bf9-4a3c-9912-ace4e6544003'), 'Expense 7', 400.00, '2023-01-07'),
    (UUID_TO_BIN('e8bb189e-8bf9-4a3c-9912-ace4e6545008'), UUID_TO_BIN('b3bb189e-8bf9-4a3c-9912-ace4e6543008'), UUID_TO_BIN('d1bb189e-8bf9-4a3c-9912-ace4e6544004'), 'Expense 8', 250.00, '2023-01-08'),
    (UUID_TO_BIN('e9bb189e-8bf9-4a3c-9912-ace4e6545009'), UUID_TO_BIN('c3bb189e-8bf9-4a3c-9912-ace4e6543009'), UUID_TO_BIN('a1bb189e-8bf9-4a3c-9912-ace4e6544001'), 'Expense 9', 500.00, '2023-01-09'),
    (UUID_TO_BIN('eabb189e-8bf9-4a3c-9912-ace4e6545010'), UUID_TO_BIN('d3bb189e-8bf9-4a3c-9912-ace4e6543010'), UUID_TO_BIN('b1bb189e-8bf9-4a3c-9912-ace4e6544002'), 'Expense 10', 125.00, '2023-01-10');

-- Populate Rel_User_Expense table
INSERT INTO Rel_User_Expense (ID_User, ID_Expense) VALUES
   (UUID_TO_BIN('a3bb189e-8bf9-4a3c-9912-ace4e6543001'), UUID_TO_BIN('e1bb189e-8bf9-4a3c-9912-ace4e6545001')),
   (UUID_TO_BIN('b3bb189e-8bf9-4a3c-9912-ace4e6543002'), UUID_TO_BIN('e2bb189e-8bf9-4a3c-9912-ace4e6545002')),
   (UUID_TO_BIN('c3bb189e-8bf9-4a3c-9912-ace4e6543003'), UUID_TO_BIN('e3bb189e-8bf9-4a3c-9912-ace4e6545003')),
   (UUID_TO_BIN('d3bb189e-8bf9-4a3c-9912-ace4e6543004'), UUID_TO_BIN('e4bb189e-8bf9-4a3c-9912-ace4e6545004')),
   (UUID_TO_BIN('e3bb189e-8bf9-4a3c-9912-ace4e6543005'), UUID_TO_BIN('e5bb189e-8bf9-4a3c-9912-ace4e6545005'));

-- Populate Rel_Expense_Group table
INSERT INTO Rel_Expense_Group (ID_Expense, ID_Group) VALUES
 (UUID_TO_BIN('e1bb189e-8bf9-4a3c-9912-ace4e6545001'), UUID_TO_BIN('a1bb189e-8bf9-4a3c-9912-ace4e6544001')),
 (UUID_TO_BIN('e2bb189e-8bf9-4a3c-9912-ace4e6545002'), UUID_TO_BIN('b1bb189e-8bf9-4a3c-9912-ace4e6544002')),
 (UUID_TO_BIN('e3bb189e-8bf9-4a3c-9912-ace4e6545003'), UUID_TO_BIN('c1bb189e-8bf9-4a3c-9912-ace4e6544003')),
 (UUID_TO_BIN('e4bb189e-8bf9-4a3c-9912-ace4e6545004'), UUID_TO_BIN('d1bb189e-8bf9-4a3c-9912-ace4e6544004')),
 (UUID_TO_BIN('e5bb189e-8bf9-4a3c-9912-ace4e6545005'), UUID_TO_BIN('a1bb189e-8bf9-4a3c-9912-ace4e6544001'));

-- Populate Rel_User_Group table
INSERT INTO Rel_User_Group (ID_User, ID_Group) VALUES
   (UUID_TO_BIN('a3bb189e-8bf9-4a3c-9912-ace4e6543001'), UUID_TO_BIN('a1bb189e-8bf9-4a3c-9912-ace4e6544001')),
   (UUID_TO_BIN('b3bb189e-8bf9-4a3c-9912-ace4e6543002'), UUID_TO_BIN('a1bb189e-8bf9-4a3c-9912-ace4e6544001')),
   (UUID_TO_BIN('c3bb189e-8bf9-4a3c-9912-ace4e6543003'), UUID_TO_BIN('b1bb189e-8bf9-4a3c-9912-ace4e6544002')),
   (UUID_TO_BIN('d3bb189e-8bf9-4a3c-9912-ace4e6543004'), UUID_TO_BIN('b1bb189e-8bf9-4a3c-9912-ace4e6544002')),
   (UUID_TO_BIN('e3bb189e-8bf9-4a3c-9912-ace4e6543005'), UUID_TO_BIN('c1bb189e-8bf9-4a3c-9912-ace4e6544003'));

-- Add foreign key constraints to Rel_User_Expense table
ALTER TABLE Rel_User_Expense
    ADD CONSTRAINT fk_user_expense_user FOREIGN KEY (ID_User) REFERENCES User(id),
ADD CONSTRAINT fk_user_expense_expense FOREIGN KEY (ID_Expense) REFERENCES Expense(id);

-- Add foreign key constraints to Rel_Expense_Group table
ALTER TABLE Rel_Expense_Group
    ADD CONSTRAINT fk_expense_group_expense FOREIGN KEY (ID_Expense) REFERENCES Expense(id),
ADD CONSTRAINT fk_expense_group_group FOREIGN KEY (ID_Group) REFERENCES `Group`(id);

-- Add foreign key constraints to Rel_User_Group table
ALTER TABLE Rel_User_Group
    ADD CONSTRAINT fk_user_group_user FOREIGN KEY (ID_User) REFERENCES User(id),
ADD CONSTRAINT fk_user_group_group FOREIGN KEY (ID_Group) REFERENCES `Group`(id);

-- Add foreign key constraints to Expense table
ALTER TABLE Expense
    ADD CONSTRAINT fk_expense_owner FOREIGN KEY (ownerID) REFERENCES User(id),
ADD CONSTRAINT fk_expense_group FOREIGN KEY (groupID) REFERENCES `Group`(id);
