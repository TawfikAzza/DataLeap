using MongoDB.Bson;
using MySql.Data.MySqlClient;

namespace EzMigrationTool.Services;

public class MysqlExtractionService {
    // Singleton class
    private static MysqlExtractionService _instance = null!;
    private readonly string _mySqlConnectionString;

    private MysqlExtractionService(string mysqlConnStr) {
        _mySqlConnectionString = mysqlConnStr;
    }

    public List<BsonDocument> ExtractUsers() {
        using var mysqlConn = new MySqlConnection(_mySqlConnectionString);
        mysqlConn.Open();
        const string query = "SELECT * FROM User";
        using var cmd = new MySqlCommand(query, mysqlConn);
        using var reader = cmd.ExecuteReader();
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
        Console.WriteLine("User data extracted.");
        return users;
    }


    public List<BsonDocument> ExtractGroups() {
        using var mysqlConn = new MySqlConnection(_mySqlConnectionString);
        mysqlConn.Open();
        const string query = "SELECT BIN_TO_UUID(id) as id, name, token FROM `Group`;";

        using var cmd = new MySqlCommand(query, mysqlConn);
        using var reader = cmd.ExecuteReader();
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
            group.Add("users", GetUsersByGroup(_mySqlConnectionString, groupId));
            group.Add("expenses", GetExpensesByGroup(_mySqlConnectionString, groupId));
        }

        Console.WriteLine("Groups data extracted.");
        return groups;
    }

    public List<BsonDocument> ExtractExpenses() {
        using var mysqlConn = new MySqlConnection(_mySqlConnectionString);
        mysqlConn.Open();
        const string query =
            "SELECT BIN_TO_UUID(id) as id, BIN_TO_UUID(ownerID) as ownerID, BIN_TO_UUID(groupID) as groupID, title, amount, date FROM Expense;";

        using var cmd = new MySqlCommand(query, mysqlConn);
        using var reader = cmd.ExecuteReader();
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
                { "owner", GetUserById(_mySqlConnectionString, ownerId!) },
                { "group", GetGroupById(_mySqlConnectionString, groupId!) }
            };

            expenses.Add(expense);
        }
        Console.WriteLine("Expenses data extracted.");
        return expenses;
    }

    private static BsonArray GetUsersByGroup(string mysqlConnStr, string groupId) {
        var users = new BsonArray();

        using var mysqlConn = new MySqlConnection(mysqlConnStr);
        mysqlConn.Open();
        const string query = """
                                         SELECT BIN_TO_UUID(u.id) as id, u.name, u.phone_number
                                         FROM User u
                                         JOIN Rel_User_Group rug ON u.id = rug.ID_User
                                         WHERE rug.ID_Group = UUID_TO_BIN(@groupId);
                             """;

        using var cmd = new MySqlCommand(query, mysqlConn);
        cmd.Parameters.AddWithValue("@groupId", groupId);
        using var reader = cmd.ExecuteReader();
        while (reader.Read()) {
            var user = new BsonDocument {
                { "user_id", reader["id"].ToString() },
                { "name", reader["name"].ToString() },
                { "phone_number", reader["phone_number"].ToString() }
            };
            users.Add(user);
        }
        return users;
    }

    private static BsonArray GetExpensesByGroup(string mysqlConnStr, string groupId) {
        var expenses = new BsonArray();

        using var mysqlConn = new MySqlConnection(mysqlConnStr);
        mysqlConn.Open();
        const string query = """
                                         SELECT BIN_TO_UUID(e.id) as id, BIN_TO_UUID(e.ownerID) as ownerID, BIN_TO_UUID(e.groupID) as groupID, e.title, e.amount, e.date
                                         FROM Expense e
                                         JOIN Rel_Expense_Group reg ON e.id = reg.ID_Expense
                                         WHERE reg.ID_Group = UUID_TO_BIN(@groupId);
                             """;

        using var cmd = new MySqlCommand(query, mysqlConn);
        cmd.Parameters.AddWithValue("@groupId", groupId);
        using var reader = cmd.ExecuteReader();
        while (reader.Read()) {
            var expense = new BsonDocument {
                { "_id", reader["id"].ToString() }
            };
            expenses.Add(expense);
        }
        return expenses;
    }

    private static BsonDocument GetUserById(string mysqlConnStr, string userId) {
        using var mysqlConn = new MySqlConnection(mysqlConnStr);
        mysqlConn.Open();
        const string query = """
                                         SELECT BIN_TO_UUID(id) as id, name, phone_number
                                         FROM User
                                         WHERE id = UUID_TO_BIN(@userId);
                             """;

        using var cmd = new MySqlCommand(query, mysqlConn);
        cmd.Parameters.AddWithValue("@userId", userId);
        using var reader = cmd.ExecuteReader();
        if (reader.Read()) {
            return new BsonDocument {
                { "user_id", reader["id"].ToString() },
                { "name", reader["name"].ToString() },
                { "phone_number", reader["phone_number"].ToString() }
            };
        }
        return null!;
    }

    private static BsonDocument GetGroupById(string mysqlConnStr, string groupId) {
        using var mysqlConn = new MySqlConnection(mysqlConnStr);
        mysqlConn.Open();
        const string query = """
                                         SELECT BIN_TO_UUID(id) as id, name, token
                                         FROM `Group`
                                         WHERE id = UUID_TO_BIN(@groupId);
                             """;

        using var cmd = new MySqlCommand(query, mysqlConn);
        cmd.Parameters.AddWithValue("@groupId", groupId);
        using var reader = cmd.ExecuteReader();
        if (reader.Read()) {
            return new BsonDocument {
                { "_id", reader["id"].ToString() },
                { "name", reader["name"].ToString() },
                { "token", reader["token"].ToString() }
            };
        }
        return null!;
    }

    #region Singleton

    public static MysqlExtractionService GetInstance(string mysqlConnStr) {
        if (_instance == null!) {
            _instance = new MysqlExtractionService(mysqlConnStr);
        }
        return _instance;
    }

    #endregion

}
