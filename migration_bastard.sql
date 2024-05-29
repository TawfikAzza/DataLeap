-- Step 1: Create the Database and Tables Without Constraints

-- Create the database GROUP_DB
CREATE DATABASE IF NOT EXISTS Group_DB;

-- Create Group table without constraints
CREATE TABLE Group_DB.`Group` (
  id BINARY(16) NOT NULL PRIMARY KEY,
  name VARCHAR(255) NOT NULL
);

-- Create UserGroup table without constraints
CREATE TABLE Group_DB.UserGroup (
    user_id BINARY(16) NOT NULL,
    group_id BINARY(16) NOT NULL
);

-- Step 2: Populate the Tables with Data

-- Populate Group table with data from EZMoney_test.Group
INSERT INTO Group_DB.`Group` (id, name)
SELECT id, name FROM EZMoney_test.`Group`;

-- Populate UserGroup table with data from EZMoney_test.Rel_User_Group
INSERT INTO Group_DB.UserGroup (user_id, group_id)
SELECT ID_User, ID_Group FROM EZMoney_test.Rel_User_Group;

-- Step 3: Add Constraints After Populating Data

-- Add foreign key constraints to UserGroup table
ALTER TABLE Group_DB.UserGroup
ADD CONSTRAINT fk_usergroup_group FOREIGN KEY (group_id) REFERENCES Group_DB.`Group`(id);

-- Step 1: Create the Database and Tables Without Constraints

-- Create the database EXPENSE_DB
CREATE DATABASE IF NOT EXISTS Expense_DB;

-- Create Expense table without constraints
CREATE TABLE Expense_DB.`Expense` (
      id BINARY(16) NOT NULL PRIMARY KEY,
      ownerId BINARY(16) NOT NULL,
      groupId BINARY(16) NOT NULL,
      title VARCHAR(255) NOT NULL,
      amount DECIMAL(10, 2) NOT NULL,
      date DATE NOT NULL
);

-- Create UserExpense table without constraints
CREATE TABLE Expense_DB.UserExpense (
    expenseId BINARY(16) NOT NULL,
    userId BINARY(16) NOT NULL
);

-- Step 2: Populate the Tables with Data

-- Populate Expense table with data from EZMoney_test.Expense
INSERT INTO Expense_DB.`Expense` (id, ownerId,groupId,title,amount,date)
SELECT id, ownerId,groupId,title,amount,date FROM EZMoney_test.`Expense`;

-- Populate UserExpense table with data from EZMoney_test.Rel_User_Expense
INSERT INTO Expense_DB.UserExpense (expenseId, userId)
SELECT ID_Expense, ID_User FROM EZMoney_test.Rel_User_Expense;

-- Step 3: Add Constraints After Populating Data

-- Add foreign key constraints to UserExpense table
ALTER TABLE Expense_DB.UserExpense
    ADD CONSTRAINT fk_userexpense_expense FOREIGN KEY (expenseId) REFERENCES Expense_DB.Expense(id)


-- Create the database USER_DB
CREATE DATABASE IF NOT EXISTS User_DB;

-- Create User table without constraints
CREATE TABLE User_DB.User (
      id BINARY(16) NOT NULL PRIMARY KEY,
      name VARCHAR(255) NOT NULL,
      phone_number VARCHAR(20)
);

-- Step 2: Populate the Tables with Data

-- Populate User table with data from EZMoney_test.User
INSERT INTO User_DB.User (id, name, phone_number)
SELECT id, name, phone_number FROM EZMoney_test.User;


