SELECT
    e.id AS ExpenseID,
    e.title AS ExpenseTitle,
    e.amount AS ExpenseAmount,
    e.date AS ExpenseDate,
    u.id AS UserID,
    u.name AS UserName,
    u.phone_number AS UserPhoneNumber,
    g.id AS GroupID,
    g.name AS GroupName,
    g.token AS GroupToken
FROM
    Expense e
        JOIN
    User u ON e.ownerID = u.id
        JOIN
    `Group` g ON e.groupID = g.id
WHERE
    e.id = UUID_TO_BIN('e1bb189e-8bf9-4a3c-9912-ace4e6545001');
