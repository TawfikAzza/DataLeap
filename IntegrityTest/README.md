# IntegrityTest

This is a test of the integrity of the migration, from SQL to NoSQL. Features a unit test class in xUnit, which really is an integration test as it connects and verifies the contents of the SQL and NoSQL databases.

Tests are carried out to verify that the migration did not compromise the integrity of the data.

The SQL database is also verified, to ensure that the data input was correct.

## What is tested?
### 1. Connection
```
TestDatabaseConnections
```
### 2. Row counts
```
TestUserCounts
TestGroupCounts
TestExpenseCounts
```
### 3. Table names
In the NoSQL version, the relation tables of course should not be not present.
```
TestTableNames
```
### 4. Field names and types
```
TestUserFields
TestGroupFields
TestExpenseFields
```

Of course, the tests are not exhaustive, but they are a good start. 
For a complete test, a more comprehensive test suite would be needed, for which there are suggestions below.

## What else could be tested?
- Data Correlation

Creating sample queries to verify that the data in the NoSQL database is consistent with the data in the SQL database.


- Constraints

Testing that the constraints are enforced in the NoSQL database.

- Referential Integrity

Testing that the referential integrity is enforced in the NoSQL database, meaning that eg. foreign keys correspond to embedded documents.

