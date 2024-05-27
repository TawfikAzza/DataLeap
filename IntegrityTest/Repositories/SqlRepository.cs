using System.Data;
using IntegrityTest.Repositories.Interfaces;
using MySql.Data.MySqlClient;

namespace IntegrityTest.Repositories;

public class SqlRepository  : IRepository
{
    private readonly string _connectionString;
    
    public SqlRepository(string connectionString)
    {
        _connectionString = connectionString;
    }
    
    public bool isConnected()
    {
        try
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                return connection.State == System.Data.ConnectionState.Open;
            }
        }
        catch
        {
            return false;
        }
    }
    
    public int GetUserCount()
    {
        return GetCount("SELECT COUNT(*) FROM User");
    }

    public int GetGroupCount()
    {
        return GetCount("SELECT COUNT(*) FROM `Group`");
    }

    public int GetExpenseCount()
    {
        return GetCount("SELECT COUNT(*) FROM Expense");
    }
    
    public List<string> getTableNames()
    {
        List<string> tableNames = new List<string>();
        using (var connection = new MySqlConnection(_connectionString))
        {
            connection.Open();
            DataTable schema = connection.GetSchema("Tables");
            foreach (DataRow row in schema.Rows)
            {
                string tableName = row["TABLE_NAME"].ToString();
                tableNames.Add(tableName);
            }
        }
        return tableNames;
    }

    public Dictionary<string, string> getFields(string tableName)
    {
        Dictionary<string, string> fields = new Dictionary<string, string>();
        using (var connection = new MySqlConnection(_connectionString))
        {
            connection.Open();
            DataTable schema = connection.GetSchema("Columns", new string[] { null, null, tableName });
            foreach (DataRow row in schema.Rows)
            {
                string fieldName = row["COLUMN_NAME"].ToString();
                string fieldType = row["DATA_TYPE"].ToString();
                fields.Add(fieldName, fieldType);
            }
        }
        return fields;
    }

    private int GetCount(string query)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            connection.Open();
            using (var command = new MySqlCommand(query, connection))
            {
                return Convert.ToInt32(command.ExecuteScalar());
            }
        }
    }
}