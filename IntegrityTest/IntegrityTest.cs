using IntegrityTest.Repositories.Factories;
using IntegrityTest.Repositories.Interfaces;

namespace IntegrityTest;

public class IntegrityTest
{
    private IRepository sql;
    private IRepository mongo;

    public IntegrityTest()
    {
        sql = SqlRepositoryFactory.Create();
        mongo = MongoRepositoryFactory.Create();
    }

    // Connection tests
    [Fact]
    public void TestDatabaseConnections()
    {
        Assert.True(sql.isConnected());
        Assert.True(mongo.isConnected());
    }
    
    // Record count tests
    [Fact]
    public void TestUserCounts()
    {
        int sqlUserCount = sql.GetUserCount();
        int mongoUserCount = mongo.GetUserCount();
        Assert.Equal(sqlUserCount, mongoUserCount);
    }
    
    [Fact]
    public void TestGroupCounts()
    {
        int sqlGroupCount = sql.GetGroupCount();
        int mongoGroupCount = mongo.GetGroupCount();
        Assert.Equal(sqlGroupCount, mongoGroupCount);
    }

    [Fact]
    public void TestExpenseCounts()
    {
        int sqlExpenseCount = sql.GetExpenseCount();
        int mongoExpenseCount = mongo.GetExpenseCount();
        
        Assert.Equal(sqlExpenseCount, mongoExpenseCount);
    }

    // Table name tests
    [Fact]
    public void TestTableNames()
    {
        //Arrange
        List<string> expectedSqlTableNames = new List<string>() { "User", "Group", "Expense", "Rel_User_Group", "Rel_User_Expense", "Rel_Expense_Group" };
        List<string> expectedMongoTableNames = new List<string>() { "User", "Group", "Expense" };
        
        //Act
        List<string> sqlTableNames = sql.getTableNames();
        List<string> mongoTableNames = mongo.getTableNames();
        
        //Assert
        
        //Two should not be equal
        Assert.NotEqual(sqlTableNames, mongoTableNames);
        
        //SQL
        //Expected table names and actual table names should be equal length
        Assert.Equal(expectedSqlTableNames.Count, sqlTableNames.Count);
        //All expected table names should be in actual table names
        Assert.True(expectedSqlTableNames.All(sqlTableNames.Contains));
        
        //Mongo
        //Expected table names and actual table names should be equal length
        Assert.Equal(expectedMongoTableNames.Count, mongoTableNames.Count);
        //All expected table names should be in actual table names
        Assert.True(expectedMongoTableNames.All(mongoTableNames.Contains));
    }

    // Field tests TODO: Continue from here
    [Fact]
    public void TestUserFields()
    {
        //Arrange
        Dictionary<string, string> expectedSqlUserFields = new Dictionary<string, string>()
        {
            { "id", "binary" }, //GUID
            { "name", "varchar" },
            { "phone_number", "varchar" },
        };
        Dictionary<string, string> expectedMongoUserFields = new Dictionary<string, string>()
        {
            { "_id", "String" },
            { "name", "String" },
            { "phone_number", "String" },
        };
        
        //Act
        Dictionary<string, string> sqlUserFields = sql.getFields("User");
        Dictionary<string, string> mongoUserFields = mongo.getFields("User");
        
        //Assert
        Assert.NotEqual(sqlUserFields, mongoUserFields);
        Assert.Equal(expectedMongoUserFields.Count, mongoUserFields.Count);
        Assert.Equal(expectedMongoUserFields, mongoUserFields);
        Assert.Equal(expectedSqlUserFields.Count, sqlUserFields.Count);
        Assert.Equal(expectedSqlUserFields, sqlUserFields);
    }

    [Fact]
    public void TestGroupFields()
    {
        //Arrange
        Dictionary<string, string> expectedSqlGroupFields = new Dictionary<string, string>()
        {
            { "id", "binary" }, //GUID
            { "name", "varchar" },
            { "token", "varchar" },
        };
        Dictionary<string, string> expectedMongoGroupFields = new Dictionary<string, string>()
        {
            { "_id", "String" },
            { "name", "String" },
            { "token", "String" },
            { "users", "Array" },
            { "expenses", "Array" },
        };
        
        //Act
        Dictionary<string, string> sqlGroupFields = sql.getFields("Group");
        Dictionary<string, string> mongoGroupFields = mongo.getFields("Group");
        
        //Assert
        Assert.NotEqual(sqlGroupFields, mongoGroupFields);
        Assert.Equal(expectedMongoGroupFields.Count, mongoGroupFields.Count);
        Assert.Equal(expectedMongoGroupFields, mongoGroupFields);
        Assert.Equal(expectedSqlGroupFields.Count, sqlGroupFields.Count);
        Assert.Equal(expectedSqlGroupFields, sqlGroupFields);
    }
    
    [Fact]
    public void TestExpenseFields()
    {
        //Arrange
        Dictionary<string, string> expectedSqlExpenseFields = new Dictionary<string, string>()
        {
            { "id", "binary" }, //GUID
            { "ownerID", "binary" }, //GUID
            { "groupID", "binary" }, //GUID
            { "title", "varchar" },
            { "amount", "decimal" },
            { "date", "date" },
        };
        Dictionary<string, string> expectedMongoExpenseFields = new Dictionary<string, string>()
        {
            { "_id", "String" },
            { "owner", "Document" },
            { "group", "Document" },
            { "title", "String" },
            { "amount", "Decimal128" },
            { "date", "DateTime" },
        };
        
        //Act
        Dictionary<string, string> sqlExpenseFields = sql.getFields("Expense");
        Dictionary<string, string> mongoExpenseFields = mongo.getFields("Expense");
        
        //Assert
        Assert.NotEqual(sqlExpenseFields, mongoExpenseFields);
        Assert.Equal(expectedMongoExpenseFields.Count, mongoExpenseFields.Count);
        Assert.Equal(expectedMongoExpenseFields, mongoExpenseFields);
        Assert.Equal(expectedSqlExpenseFields.Count, sqlExpenseFields.Count);
        Assert.Equal(expectedSqlExpenseFields, sqlExpenseFields);
    }
}