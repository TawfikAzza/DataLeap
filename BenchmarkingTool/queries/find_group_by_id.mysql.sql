SELECT
    g.id AS GroupID,
    g.name AS GroupName,
    g.token AS GroupToken,
    u.id AS UserID,
    u.name AS UserName,
    u.phone_number AS UserPhoneNumber,
    e.id AS ExpenseID,
    e.title AS ExpenseTitle,
    e.amount AS ExpenseAmount,
    e.date AS ExpenseDate
FROM
    `Group` g
        LEFT JOIN
    Rel_User_Group rug ON g.id = rug.ID_Group
        LEFT JOIN
    User u ON rug.ID_User = u.id
        LEFT JOIN
    Rel_Expense_Group reg ON g.id = reg.ID_Group
        LEFT JOIN
    Expense e ON reg.ID_Expense = e.id
WHERE
    g.id = UUID_TO_BIN('a1bb189e-8bf9-4a3c-9912-ace4e6544001');
