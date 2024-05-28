namespace EzMigrationTool.Entities.Mongo;

public class MongoGroup {
    public string _id { get; set; }

    public string name { get; set; }

    public string token { get; set; }

    public List<MongoUser> users { get; set; }

    public List<MongoExpense> expenses { get; set; }
}
