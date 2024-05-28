namespace EzMigrationTool.Services;

public class MigrationService { // DEPRECATED
/*
static void MigrateUsers(string mysqlConnStr, string mongoConnStr, string mongoDatabase) {
    var mongoClient = new MongoClient(mongoConnStr);
    var mongoDb = mongoClient.GetDatabase(mongoDatabase);
    var userCollection = mongoDb.GetCollection<BsonDocument>("User");

    using (var mysqlConn = new MySqlConnection(mysqlConnStr)) {
        mysqlConn.Open();
        string query = "SELECT * FROM User";
        using (var cmd = new MySqlCommand(query, mysqlConn))
        using (var reader = cmd.ExecuteReader()) {
            var users = new List<BsonDocument>();
            while (reader.Read()) {
                var userId = new Guid((byte[])reader["id"]).ToString();
                var user = new BsonDocument {
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

static void MigrateGroups(string mysqlConnStr, string mongoConnStr, string mongoDatabase) {
    var mongoClient = new MongoClient(mongoConnStr);
    var mongoDb = mongoClient.GetDatabase(mongoDatabase);
    var groupCollection = mongoDb.GetCollection<BsonDocument>("Group");

    using (var mysqlConn = new MySqlConnection(mysqlConnStr)) {
        mysqlConn.Open();
        string query = "SELECT BIN_TO_UUID(id) as id, name, token FROM `Group`;";
        using (var cmd = new MySqlCommand(query, mysqlConn))
        using (var reader = cmd.ExecuteReader()) {
            var groups = new List<BsonDocument>();
            while (reader.Read()) {
                var groupId = reader["id"].ToString();
                var group = new BsonDocument {
                    { "_id", groupId },
                    { "name", reader["name"].ToString() },
                    { "token", reader["token"].ToString() }
                };

                groups.Add(group);
            }

            foreach (var group in groups) {
                var groupId = group["_id"].ToString()!;
                group.Add("users", GetUsersByGroup(mysqlConnStr, groupId));
                group.Add("expenses", GetExpensesByGroup(mysqlConnStr, groupId));
            }

            groupCollection.InsertMany(groups);
            Console.WriteLine("Groups migrated successfully.");
        }
    }
}



static void MigrateExpenses(string mysqlConnStr, string mongoConnStr, string mongoDatabase) {
    var mongoClient = new MongoClient(mongoConnStr);
    var mongoDb = mongoClient.GetDatabase(mongoDatabase);
    var expenseCollection = mongoDb.GetCollection<BsonDocument>("Expense");

    using (var mysqlConn = new MySqlConnection(mysqlConnStr)) {
        mysqlConn.Open();
        string query =
            "SELECT BIN_TO_UUID(id) as id, BIN_TO_UUID(ownerID) as ownerID, BIN_TO_UUID(groupID) as groupID, title, amount, date FROM Expense;";
        using (var cmd = new MySqlCommand(query, mysqlConn))
        using (var reader = cmd.ExecuteReader()) {
            var expenses = new List<BsonDocument>();
            while (reader.Read()) {
                var expenseId = reader["id"].ToString();
                var ownerId = reader["ownerID"].ToString();
                var groupId = reader["groupID"].ToString();

                var expense = new BsonDocument {
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
            Console.WriteLine("Expenses migrated successfully.");
        }
    }
}
*/
}