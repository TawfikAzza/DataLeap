// MySQL connection string

using MongoDB.Bson;
using MongoDB.Driver;
using MySql.Data.MySqlClient;

string mysqlConnStr = "server=localhost;user=root;database=EZMoney_test;password=rootpassword;";

// MongoDB connection string
string mongoConnStr = "mongodb://localhost:27017";

// MongoDB database and collection names
string mongoDatabase = "EZMoney_test";

// Connect to MySQL
using (var mysqlConn = new MySqlConnection(mysqlConnStr)) {
    mysqlConn.Open();

    // Migrate User table
    MigrateTable(mysqlConn, mongoConnStr, mongoDatabase, "User", new string[] { "id", "name", "phone_number" });

    // Migrate Group table
    MigrateTable(mysqlConn, mongoConnStr, mongoDatabase, "`Group`", new string[] { "id", "name", "token" });

    // Migrate Expense table
    MigrateTable(mysqlConn, mongoConnStr, mongoDatabase, "Expense",
        new string[] { "id", "ownerID", "groupID", "title", "amount", "date" });

    // Migrate Rel_User_Expense table
    MigrateTable(mysqlConn, mongoConnStr, mongoDatabase, "Rel_User_Expense", new string[] { "ID_User", "ID_Expense" });

    // Migrate Rel_Expense_Group table
    MigrateTable(mysqlConn, mongoConnStr, mongoDatabase, "Rel_Expense_Group", new string[] { "ID_Expense", "ID_Group" });

    // Migrate Rel_User_Group table
    MigrateTable(mysqlConn, mongoConnStr, mongoDatabase, "Rel_User_Group", new string[] { "ID_User", "ID_Group" });

    Console.WriteLine("Data migration completed successfully.");
}


static void MigrateTable(MySqlConnection mysqlConn, string mongoConnStr, string mongoDatabase, string tableName, string[] columns) {
    string query = $"SELECT * FROM {tableName}";
    using (var cmd = new MySqlCommand(query, mysqlConn))
    using (var reader = cmd.ExecuteReader()) {
        var documents = new List<BsonDocument>();
        while (reader.Read()) {
            var document = new BsonDocument();
            foreach (var column in columns) {
                document[column] = BsonValue.Create(reader[column]);
            }
            documents.Add(document);
        }

        // Connect to MongoDB
        var mongoClient = new MongoClient(mongoConnStr);
        var mongoDb = mongoClient.GetDatabase(mongoDatabase);
        var mongoCol = mongoDb.GetCollection<BsonDocument>(tableName);

        // Insert documents into MongoDB
        mongoCol.InsertMany(documents);
    }
}
