using IntegrityTest.Repositories.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace IntegrityTest.Repositories;

public class MongoRepository : IRepository
{
    private readonly string _connectionString;
    private readonly string _databaseName;
    
    public MongoRepository(string connectionString, string databaseName)
    {
        _connectionString = connectionString;
        _databaseName = databaseName;
    }
    
    public bool isConnected()
    {
        try
        {
            var client = new MongoClient(_connectionString);
            var database = client.GetDatabase("admin");
            database.RunCommandAsync((Command<BsonDocument>)"{ping:1}").Wait();
            return true;
        }
        catch
        {
            return false;
        }
    }
    
    public int GetUserCount()
    {
        return GetCount("User");
    }
    
    public int GetGroupCount()
    {
        return GetCount("Group");
    }
    
    public int GetExpenseCount()
    {
        return GetCount("Expense");
    }
    
    private int GetCount(string collectionName)
    {
        var client = new MongoClient(_connectionString);
        var database = client.GetDatabase(_databaseName);
        var collection = database.GetCollection<BsonDocument>(collectionName);
        return (int)collection.CountDocuments(new BsonDocument());
    }
    
    public List<string> getTableNames()
    {
        var client = new MongoClient(_connectionString);
        var database = client.GetDatabase(_databaseName);
        var collectionNames = database.ListCollectionNames().ToList();
        return collectionNames;
    }

    public Dictionary<string, string> getFields(string collectionName)
    {
        var client = new MongoClient(_connectionString);
        var database = client.GetDatabase(_databaseName);
        var collection = database.GetCollection<BsonDocument>(collectionName);
        var fields = new Dictionary<string, string>();

        // Get the first document in the collection to extract its structure
        var document = collection.Find(new BsonDocument()).FirstOrDefault();
        if (document != null)
        {
            foreach (var element in document.Elements)
            {
                fields.Add(element.Name, element.Value.BsonType.ToString());
            }
        }

        return fields;
    }
}
