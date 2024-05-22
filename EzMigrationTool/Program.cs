// MySQL connection string

using System.Data;
using MongoDB.Bson;
using MongoDB.Driver;
using MySql.Data.MySqlClient;

// MySQL connection string
string mysqlConnStr = "server=localhost;user=root;database=EZMoney_test;password=rootpassword;";

// MongoDB connection string
string mongoConnStr = "mongodb://localhost:27017";

// MongoDB database name
string mongoDatabase = "EZMoney_test";

// Connect to MySQL
using (var mysqlConn = new MySqlConnection(mysqlConnStr))
{
    mysqlConn.Open();

    // Migrate User table
    MigrateUsers(mysqlConnStr, mongoConnStr, mongoDatabase);

    // Migrate Group table
    MigrateGroups(mysqlConnStr, mongoConnStr, mongoDatabase);

    // Migrate Expense table
    //MigrateExpenses(mysqlConnStr, mongoConnStr, mongoDatabase);

    Console.WriteLine("Data migration completed successfully.");
}

static void MigrateUsers(string mysqlConnStr, string mongoConnStr, string mongoDatabase)
    {
        var mongoClient = new MongoClient(mongoConnStr);
        var mongoDb = mongoClient.GetDatabase(mongoDatabase);
        var userCollection = mongoDb.GetCollection<BsonDocument>("User");

        using (var mysqlConn = new MySqlConnection(mysqlConnStr))
        {
            mysqlConn.Open();
            string query = "SELECT * FROM User";
            using (var cmd = new MySqlCommand(query, mysqlConn))
            using (var reader = cmd.ExecuteReader())
            {
                var users = new List<BsonDocument>();
                while (reader.Read())
                {
                    var userId = new Guid((byte[])reader["id"]).ToString();
                    var user = new BsonDocument
                    {
                        { "_id", userId },
                        { "name", reader["name"].ToString() },
                        { "phone_number", reader["phone_number"].ToString() }
                    };

                    // Add expenses and groups later
                    users.Add(user);
                }
                userCollection.InsertMany(users);
                Console.WriteLine("Users migrated successfully.");
            }
        }
    }


static void MigrateGroups(string mysqlConnStr, string mongoConnStr, string mongoDatabase)
{
    var mongoClient = new MongoClient(mongoConnStr);
    var mongoDb = mongoClient.GetDatabase(mongoDatabase);
    var groupCollection = mongoDb.GetCollection<BsonDocument>("Group");

    using (var mysqlConn = new MySqlConnection(mysqlConnStr))
    {
        mysqlConn.Open();
        string query = "SELECT BIN_TO_UUID(id) as id, name, token FROM `Group`;";
        using (var cmd = new MySqlCommand(query, mysqlConn))
        using (var reader = cmd.ExecuteReader())
        {
            var groups = new List<BsonDocument>();
            while (reader.Read())
            {
                var groupId = reader["id"].ToString();
                var group = new BsonDocument
                {
                    { "_id", groupId },
                    { "name", reader["name"].ToString() },
                    { "token", reader["token"].ToString() }
                };

                groups.Add(group);
            }

            foreach (var group in groups)
            {
                var groupId = group["_id"].ToString()!;
                //group.Add("users", GetUsersByGroup(mysqlConnStr, groupId));
                group.Add("expenses", GetExpensesByGroup(mysqlConnStr, groupId));
            }

            groupCollection.InsertMany(groups);
        }
    }
}

    static BsonArray GetUsersByGroup(string mysqlConnStr, string groupId)
    {
        var users = new BsonArray();
        string query = $"SELECT u.id, u.name, u.phone_number FROM User u JOIN Rel_User_Group rg ON u.id = rg.ID_User WHERE rg.ID_Group = UNHEX(REPLACE('{groupId}', '-', ''))";

        using (var mysqlConn = new MySqlConnection(mysqlConnStr))
        {
            mysqlConn.Open();
            using (var cmd = new MySqlCommand(query, mysqlConn))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var user = new BsonDocument
                    {
                        { "user_id", new Guid((byte[])reader["id"]).ToString() },
                        { "name", reader["name"].ToString() },
                        { "phone_number", reader["phone_number"].ToString() }
                    };
                    users.Add(user);
                }
            }
        }
        return users;
    }

// done it works
    static BsonArray GetExpensesByGroup(string mysqlConnStr, string groupId)
    {
        var expenses = new BsonArray();

        using (var mysqlConn = new MySqlConnection(mysqlConnStr))
        {
            mysqlConn.Open();
            string query = @"
            SELECT BIN_TO_UUID(e.id) as id, BIN_TO_UUID(e.ownerID) as ownerID, BIN_TO_UUID(e.groupID) as groupID, e.title, e.amount, e.date 
            FROM Expense e 
            JOIN Rel_Expense_Group reg ON e.id = reg.ID_Expense 
            WHERE reg.ID_Group = UUID_TO_BIN(@groupId);";
        
            using (var cmd = new MySqlCommand(query, mysqlConn))
            {
                cmd.Parameters.AddWithValue("@groupId", groupId);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var expense = new BsonDocument
                        {
                            { "id", reader["id"].ToString() }
                        };
                        expenses.Add(expense);
                    }
                }
            }
        }

        return expenses;
    }
/*
    static void MigrateExpenses(string mysqlConnStr, string mongoConnStr, string mongoDatabase)
    {
        var mongoClient = new MongoClient(mongoConnStr);
        var mongoDb = mongoClient.GetDatabase(mongoDatabase);
        var expenseCollection = mongoDb.GetCollection<BsonDocument>("Expense");

        using (var mysqlConn = new MySqlConnection(mysqlConnStr))
        {
            mysqlConn.Open();
            string query = "SELECT * FROM Expense";
            using (var cmd = new MySqlCommand(query, mysqlConn))
            using (var reader = cmd.ExecuteReader())
            {
                var expenses = new List<BsonDocument>();
                while (reader.Read())
                {
                    var expenseId = new Guid((byte[])reader["id"]).ToString();
                    var ownerId = new Guid((byte[])reader["ownerID"]).ToString();
                    var groupId = new Guid((byte[])reader["groupID"]).ToString();

                    var expense = new BsonDocument
                    {
                        { "_id", expenseId },
                        { "title", reader["title"].ToString() },
                        { "amount", Convert.ToDecimal(reader["amount"]) },
                        { "date", Convert.ToDateTime(reader["date"]) },
                        { "owner", GetUserById(mysqlConnStr, ownerId) },
                        { "group", GetGroupById(mysqlConnStr, groupId) }
                    };

                    expenses.Add(expense);
                }
                expenseCollection.InsertMany(expenses);
            }
        }
    }

    static BsonDocument GetUserById(string mysqlConnStr, string userId)
    {
        string query = $"SELECT * FROM User WHERE id = UNHEX(REPLACE('{userId}', '-', ''))";
        using (var mysqlConn = new MySqlConnection(mysqlConnStr))
        {
            mysqlConn.Open();
            using (var cmd = new MySqlCommand(query, mysqlConn))
            using (var reader = cmd.ExecuteReader(CommandBehavior.SingleRow))
            {
                if (reader.Read())
                {
                    return new BsonDocument
                    {
                        { "user_id", userId },
                        { "name", reader["name"].ToString() },
                        { "phone_number", reader["phone_number"].ToString() }
                    };
                }
            }
        }
        return null;
    }

    static BsonDocument GetGroupById(string mysqlConnStr, string groupId)
    {
        string query = $"SELECT * FROM `Group` WHERE id = UNHEX(REPLACE('{groupId}', '-', ''))";
        using (var mysqlConn = new MySqlConnection(mysqlConnStr))
        {
            mysqlConn.Open();
            using (var cmd = new MySqlCommand(query, mysqlConn))
            using (var reader = cmd.ExecuteReader(CommandBehavior.SingleRow))
            {
                if (reader.Read())
                {
                    return new BsonDocument
                    {
                        { "group_id", groupId },
                        { "name", reader["name"].ToString() },
                        { "token", reader["token"].ToString() }
                    };
                }
            }
        }
        return null;
    }
*/