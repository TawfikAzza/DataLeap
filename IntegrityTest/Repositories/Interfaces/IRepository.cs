namespace IntegrityTest.Repositories.Interfaces;

public interface IRepository
{
    public bool isConnected();
    
    // Methods for counting records
    int GetUserCount();
    int GetGroupCount();
    int GetExpenseCount();
    
    List<string> getTableNames();
    Dictionary<string, string> getFields(string tableName); // tableName -> fieldName -> fieldType
}