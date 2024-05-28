using System.Text.Json;

namespace BenchmarkingTool;

public class BenchmarkQuery
{
    public string Name { get; set; }
    public string Query { get; set; }
}

public class BenchmarkConfig
{
    public string Name { get; set; }
    public string DatabaseType { get; set; }
    public BenchmarkQuery[] Queries { get; set; }
    
    public KeyValuePair<string, string>[] NameQueryPairs => Queries.Select(q => new KeyValuePair<string, string>(q.Name, q.Query)).ToArray();
    
    public static BenchmarkConfig FromFile(string path)
    {
        var json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<BenchmarkConfig>(json);
    }
}