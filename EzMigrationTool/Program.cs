using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Driver;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using JsonConvert = Newtonsoft.Json.JsonConvert;


// MySQL connection string
string mysqlConnStr = "server=localhost;user=root;database=EZMoney_test;password=rootpassword;";

// MongoDB connection string
string mongoConnStr = "mongodb://localhost:27017";

// MongoDB database name
string mongoDatabase = "EZMoney_test";

// Connect to MySQL
using (var mysqlConn = new MySqlConnection(mysqlConnStr)) {
    mysqlConn.Open();
    
    ExtractUsers(mysqlConnStr);
    ExtractGroups(mysqlConnStr);
    ExtractExpenses(mysqlConnStr);

    Console.WriteLine("Data extraction completed successfully.");
    Console.WriteLine("Would you like to migrate the data to MongoDB? (y/n)");
}

var step2 = Console.ReadLine();

if (step2 == "n") {
    return;
}

using (var mysqlConn = new MySqlConnection(mysqlConnStr)) {
    mysqlConn.Open();
    
    InjectUsersToMongo(mongoConnStr, mongoDatabase);
    InjectGroupsToMongo(mongoConnStr, mongoDatabase);
    InjectExpensesToMongo(mongoConnStr, mongoDatabase);

    Console.WriteLine("Data migration completed successfully.");
}

static void ExtractUsers(string mysqlConnStr) {
    using (var mysqlConn = new MySqlConnection(mysqlConnStr)) {
        mysqlConn.Open();
        string query = "SELECT * FROM User";
        using (var cmd = new MySqlCommand(query, mysqlConn))
        using (var reader = cmd.ExecuteReader()) {
            var users = new List<BsonDocument>();
            while (reader.Read()) {
                var userId = new Guid((byte[])reader["id"]).ToString();
                var user = new BsonDocument {
                    { "user_id", userId },
                    { "name", reader["name"].ToString() },
                    { "phone_number", reader["phone_number"].ToString() }
                };
                // Add the user to the BSON array
                users.Add(user);
            }
            Console.WriteLine("User data extracted successfully.");
            SaveDataToFile(users, "users.json");
        }
    }
}

static void ExtractGroups(string mysqlConnStr) {
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
            
            Console.WriteLine("Groups data extracted successfully.");
            SaveDataToFile(groups, "groups.json");
        }
    }
}

static void ExtractExpenses(string mysqlConnStr) {
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
                    { "amount", reader["amount"].ToString() },
                    { "date", reader["date"].ToString() },
                    { "owner", GetUserById(mysqlConnStr, ownerId) },
                    { "group", GetGroupById(mysqlConnStr, groupId) }
                };

                expenses.Add(expense);
            }
            Console.WriteLine("Expenses data extracted successfully.");
            SaveDataToFile(expenses, "expenses.json");
        }
    }
}

static void SaveDataToFile(List<BsonDocument> document, string fileName) {
    try {
        // Convert the list of BsonDocuments to a BsonArray and then to JSON
        var usersJson = new BsonArray(document).ToJson(new JsonWriterSettings { Indent = true });

        // Get the current working directory
        string currentDirectory = Environment.CurrentDirectory;

        // Navigate to the desired target directory relative to the current directory
        string parentDirectory = Directory.GetParent(currentDirectory).Parent.Parent.FullName;
        string targetDirectory = Path.Combine(parentDirectory, "jsonData");

        // Ensure the directory exists
        if (!Directory.Exists(targetDirectory)) {
            Directory.CreateDirectory(targetDirectory);
        }

        // Define the file path
        string filePath = Path.Combine(targetDirectory, fileName);

        // Open a StreamWriter to write the JSON string to a file
        using (var writer = new StreamWriter(filePath)) {
            writer.Write(usersJson);
        }

        Console.WriteLine("Data saved to file successfully.");
    } catch (Exception ex) {
        Console.WriteLine("An error occurred while saving data to file: " + ex.Message);
    }
}

static void InjectUsersToMongo(string mongoConnStr, string mongoDatabase) {
    // Create a MongoClient with the connection string
    var mongoClient = new MongoClient(mongoConnStr);
    
    // Get the specified database
    var mongoDb = mongoClient.GetDatabase(mongoDatabase);
    
    // Get the "User" collection (if it doesn't exist, it will be created)
    var userCollection = mongoDb.GetCollection<BsonDocument>("User");
    
    // Navigate to the desired target directory relative to the current directory
    string parentDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
    string targetDirectory = Path.Combine(parentDirectory, "jsonData");
    
    // Read the JSON file
    var jsonFilePath = Path.Combine(targetDirectory, "users.json");
    var jsonData = File.ReadAllText(jsonFilePath);

    // Deserialize JSON data into a list of User objects
    var users = JsonConvert.DeserializeObject<List<MongoUser>>(jsonData);
    
    // Convert the list of User objects to a list of BsonDocument
    var bsonDocuments = new List<BsonDocument>();
    foreach (var user in users)
    {
        var bsonDocument = new BsonDocument
        {
            { "user_id", user.user_id },
            { "name", user.name },
            { "phone_number", user.phone_number }
        };
        bsonDocuments.Add(bsonDocument);
    }
    
    // Insert the BsonDocuments into the "User" collection
    userCollection.InsertMany(bsonDocuments);
    Console.WriteLine("Users migrated successfully.");
}

static void InjectGroupsToMongo(string mongoConnStr, string mongoDatabase) {
    // Create a MongoClient with the connection string
    var mongoClient = new MongoClient(mongoConnStr);
    
    // Get the specified database
    var mongoDb = mongoClient.GetDatabase(mongoDatabase);
    
    // Get the "Group" collection (if it doesn't exist, it will be created)
    var groupCollection = mongoDb.GetCollection<BsonDocument>("Group");
    
    // Navigate to the desired target directory relative to the current directory
    string parentDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
    string targetDirectory = Path.Combine(parentDirectory, "jsonData");
    
    // Read the JSON file
    var jsonFilePath = Path.Combine(targetDirectory, "groups.json");
    var jsonData = File.ReadAllText(jsonFilePath);

    // Deserialize JSON data into a list of Group objects
    var groups = JsonConvert.DeserializeObject<List<MongoGroup>>(jsonData);
    
    // Convert the list of Group objects to a list of BsonDocument
    var bsonDocuments = new List<BsonDocument>();
    foreach (var group in groups)
    {
        var bsonDocument = new BsonDocument
        {
            { "_id", group._id },
            { "name", group.name },
            { "token", group.token },
            { "users", new BsonArray(group.users.ConvertAll(user => new BsonDocument
                {
                    { "user_id", user.user_id },
                    { "name", user.name },
                    { "phone_number", user.phone_number }
                }))
            },
            { "expenses", new BsonArray(group.expenses.ConvertAll(expense => new BsonDocument
                {
                    { "id", expense._id}
                }))
            }
        };
        bsonDocuments.Add(bsonDocument);
    }
    
    // Insert the BsonDocuments into the "Group" collection
    groupCollection.InsertMany(bsonDocuments);
    Console.WriteLine("Groups migrated successfully.");
}

static void InjectExpensesToMongo(string mongoConnStr, string mongoDatabase)
{
    // Create a MongoClient with the connection string
    var mongoClient = new MongoClient(mongoConnStr);
    
    // Get the specified database
    var mongoDb = mongoClient.GetDatabase(mongoDatabase);
    
    // Get the "Expense" collection (if it doesn't exist, it will be created)
    var expenseCollection = mongoDb.GetCollection<BsonDocument>("Expense");
    
    // Navigate to the desired target directory relative to the current directory
    string parentDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
    string targetDirectory = Path.Combine(parentDirectory, "jsonData");
    
    // Read the JSON file
    var jsonFilePath = Path.Combine(targetDirectory, "expenses.json");
    var jsonData = File.ReadAllText(jsonFilePath);

    // Deserialize JSON data into a list of Expense objects
    var expenses = JsonConvert.DeserializeObject<List<MongoExpense>>(jsonData);
    
    // Convert the list of Expense objects to a list of BsonDocument
    var bsonDocuments = new List<BsonDocument>();
    foreach (var expense in expenses)
    {
        var bsonDocument = new BsonDocument
        {
            { "_id", expense._id ?? throw new ArgumentNullException(nameof(expense._id)) },
            { "title", expense.title ?? throw new ArgumentNullException(nameof(expense.title)) },
            { "amount", expense.amount != null ? Convert.ToDecimal(expense.amount) : throw new ArgumentNullException(nameof(expense.amount)) },
            { "date", DateTime.Parse(expense.date) },
            { "owner", new BsonDocument
                {
                    { "id", expense.owner?.user_id ?? throw new ArgumentNullException(nameof(expense.owner.user_id)) },
                    { "name", expense.owner?.name ?? throw new ArgumentNullException(nameof(expense.owner.name)) },
                    { "phone_number", expense.owner?.phone_number ?? throw new ArgumentNullException(nameof(expense.owner.phone_number)) }
                }
            },
            { "group", new BsonDocument
                {
                    { "id", expense.group?._id ?? throw new ArgumentNullException(nameof(expense.group._id)) },
                    { "name", expense.group?.name ?? throw new ArgumentNullException(nameof(expense.group.name)) },
                    { "token", expense.group?.token ?? throw new ArgumentNullException(nameof(expense.group.token)) }
                }
            }
        };
        bsonDocuments.Add(bsonDocument);
    }
    
    // Insert the BsonDocuments into the "Expense" collection
    expenseCollection.InsertMany(bsonDocuments);
    Console.WriteLine("Expenses migrated successfully.");
}



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

static BsonArray GetUsersByGroup(string mysqlConnStr, string groupId) {
    var users = new BsonArray();

    using (var mysqlConn = new MySqlConnection(mysqlConnStr)) {
        mysqlConn.Open();
        string query = @"
            SELECT BIN_TO_UUID(u.id) as id, u.name, u.phone_number 
            FROM User u 
            JOIN Rel_User_Group rug ON u.id = rug.ID_User 
            WHERE rug.ID_Group = UUID_TO_BIN(@groupId);";

        using (var cmd = new MySqlCommand(query, mysqlConn)) {
            cmd.Parameters.AddWithValue("@groupId", groupId);
            using (var reader = cmd.ExecuteReader()) {
                while (reader.Read()) {
                    var user = new BsonDocument {
                        { "user_id", reader["id"].ToString() },
                        { "name", reader["name"].ToString() },
                        { "phone_number", reader["phone_number"].ToString() }
                    };
                    users.Add(user);
                }
            }
        }
    }

    return users;
}

static BsonArray GetExpensesByGroup(string mysqlConnStr, string groupId) {
    var expenses = new BsonArray();

    using (var mysqlConn = new MySqlConnection(mysqlConnStr)) {
        mysqlConn.Open();
        string query = @"
            SELECT BIN_TO_UUID(e.id) as id, BIN_TO_UUID(e.ownerID) as ownerID, BIN_TO_UUID(e.groupID) as groupID, e.title, e.amount, e.date 
            FROM Expense e 
            JOIN Rel_Expense_Group reg ON e.id = reg.ID_Expense 
            WHERE reg.ID_Group = UUID_TO_BIN(@groupId);";

        using (var cmd = new MySqlCommand(query, mysqlConn)) {
            cmd.Parameters.AddWithValue("@groupId", groupId);
            using (var reader = cmd.ExecuteReader()) {
                while (reader.Read()) {
                    var expense = new BsonDocument {
                        { "_id", reader["id"].ToString() }
                    };
                    expenses.Add(expense);
                }
            }
        }
    }

    return expenses;
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

static BsonDocument GetUserById(string mysqlConnStr, string userId) {
    using (var mysqlConn = new MySqlConnection(mysqlConnStr)) {
        mysqlConn.Open();
        string query = @"
            SELECT BIN_TO_UUID(id) as id, name, phone_number 
            FROM User 
            WHERE id = UUID_TO_BIN(@userId);";

        using (var cmd = new MySqlCommand(query, mysqlConn)) {
            cmd.Parameters.AddWithValue("@userId", userId);
            using (var reader = cmd.ExecuteReader()) {
                if (reader.Read()) {
                    return new BsonDocument {
                        { "user_id", reader["id"].ToString() },
                        { "name", reader["name"].ToString() },
                        { "phone_number", reader["phone_number"].ToString() }
                    };
                }
            }
        }
    }
    return null;
}

static BsonDocument GetGroupById(string mysqlConnStr, string groupId) {
    using (var mysqlConn = new MySqlConnection(mysqlConnStr)) {
        mysqlConn.Open();
        string query = @"
            SELECT BIN_TO_UUID(id) as id, name, token 
            FROM `Group` 
            WHERE id = UUID_TO_BIN(@groupId);";

        using (var cmd = new MySqlCommand(query, mysqlConn)) {
            cmd.Parameters.AddWithValue("@groupId", groupId);
            using (var reader = cmd.ExecuteReader()) {
                if (reader.Read()) {
                    return new BsonDocument {
                        { "_id", reader["id"].ToString() },
                        { "name", reader["name"].ToString() },
                        { "token", reader["token"].ToString() }
                    };
                }
            }
        }
    }
    return null;
}

public class MongoUser
{
    public string user_id { get; set; }
    public string name { get; set; }
    public string phone_number { get; set; }
}

public class MongoExpense
{
    public string _id { get; set; }
    public string title { get; set; }
    public string amount { get; set; }
    public string date { get; set; }
    public MongoUser owner { get; set; }
    public MongoGroup group { get; set; }
}

public class MongoGroup
{
    public string _id { get; set; }
    public string name { get; set; }
    public string token { get; set; }
    public List<MongoUser> users { get; set; }
    public List<MongoExpense> expenses { get; set; }
}