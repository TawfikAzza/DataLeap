namespace EzMigrationTool.Entities.Mongo;

public class MongoExpense {
    public string _id { get; set; }

    public string title { get; set; }

    public string amount { get; set; }

    public string date { get; set; }

    public MongoUser owner { get; set; }

    public MongoGroup group { get; set; }
}
