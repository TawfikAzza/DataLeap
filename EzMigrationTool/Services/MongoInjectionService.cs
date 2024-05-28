using EzMigrationTool.Entities.Mongo;
using MongoDB.Bson;
using MongoDB.Driver;
using JsonConvert = Newtonsoft.Json.JsonConvert;

namespace EzMigrationTool.Services;

public class MongoInjectionService {
    // Singleton class
    private static MongoInjectionService _instance;
    private string _mongoConnStr;
    private string _mongoDatabase;

    private MongoInjectionService(string mongoConnStr, string mongoDatabase) {
        _mongoConnStr = mongoConnStr;
        _mongoDatabase = mongoDatabase;
    }

    public void InjectUsersToMongo() {
        // Create a MongoClient with the connection string
        var mongoClient = new MongoClient(_mongoConnStr);

        // Get the specified database
        var mongoDb = mongoClient.GetDatabase(_mongoDatabase);

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
        foreach (var user in users) {
            var bsonDocument = new BsonDocument {
                { "user_id", user.user_id },
                { "name", user.name },
                { "phone_number", user.phone_number }
            };
            bsonDocuments.Add(bsonDocument);
        }

        // Insert the BsonDocuments into the "User" collection
        userCollection.InsertMany(bsonDocuments);
        Console.WriteLine("Users transaction complete.");
    }

    public void InjectGroupsToMongo() {
        // Create a MongoClient with the connection string
        var mongoClient = new MongoClient(_mongoConnStr);

        // Get the specified database
        var mongoDb = mongoClient.GetDatabase(_mongoDatabase);

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
        foreach (var group in groups) {
            var bsonDocument = new BsonDocument {
                { "_id", group._id },
                { "name", group.name },
                { "token", group.token }, {
                    "users", new BsonArray(group.users.ConvertAll(user => new BsonDocument {
                        { "user_id", user.user_id },
                        { "name", user.name },
                        { "phone_number", user.phone_number }
                    }))
                }, {
                    "expenses", new BsonArray(group.expenses.ConvertAll(expense => new BsonDocument {
                        { "id", expense._id }
                    }))
                }
            };
            bsonDocuments.Add(bsonDocument);
        }

        // Insert the BsonDocuments into the "Group" collection
        groupCollection.InsertMany(bsonDocuments);
        Console.WriteLine("Groups transaction complete.");
    }

    public void InjectExpensesToMongo() {
        // Create a MongoClient with the connection string
        var mongoClient = new MongoClient(_mongoConnStr);

        // Get the specified database
        var mongoDb = mongoClient.GetDatabase(_mongoDatabase);

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
        foreach (var expense in expenses) {
            var bsonDocument = new BsonDocument {
                { "_id", expense._id ?? throw new ArgumentNullException(nameof(expense._id)) },
                { "title", expense.title ?? throw new ArgumentNullException(nameof(expense.title)) }, {
                    "amount",
                    expense.amount != null ? Convert.ToDecimal(expense.amount) : throw new ArgumentNullException(nameof(expense.amount))
                },
                { "date", DateTime.Parse(expense.date) }, {
                    "owner", new BsonDocument {
                        { "id", expense.owner?.user_id ?? throw new ArgumentNullException(nameof(expense.owner.user_id)) },
                        { "name", expense.owner?.name ?? throw new ArgumentNullException(nameof(expense.owner.name)) }, {
                            "phone_number",
                            expense.owner?.phone_number ?? throw new ArgumentNullException(nameof(expense.owner.phone_number))
                        }
                    }
                }, {
                    "group", new BsonDocument {
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
        Console.WriteLine("Expenses transaction complete.");
    }
    
    #region Singleton

    public static MongoInjectionService GetInstance(string mongoConnStr, string mongoDatabase) {
        if (_instance == null!) {
            _instance = new MongoInjectionService(mongoConnStr, mongoDatabase);
        }
        return _instance;
    }

    #endregion
    
    
}
