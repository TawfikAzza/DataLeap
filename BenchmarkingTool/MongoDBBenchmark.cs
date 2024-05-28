using System.Diagnostics;
using MongoDB.Bson;
using MongoDB.Driver;

namespace BenchmarkingTool;

public class MongoDbBenchmark : DatabaseBenchmark
{
    private readonly MongoClient _client;
    private readonly string _databaseName;
    
    public MongoDbBenchmark(string connectionString, string databaseName)
    {
        _client = new MongoClient(connectionString);
        _databaseName = databaseName;
    }
    public override Task<BenchmarkResult> MeasureQueryPerformanceAsync(string name, string query, int iterations)
    {
        var database = _client.GetDatabase(_databaseName);
        
        var queryStopwatch = new Stopwatch();
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        
        for (var i = 0; i < iterations; i++)
        {
            // Measure the time it takes to execute the query
            queryStopwatch.Start();
            database.RunCommand<BsonDocument>(@query);
            queryStopwatch.Stop();
            
            // Pause the stopwatch while saving the iteration duration and resetting the query stopwatch
            stopwatch.Stop();
            SaveIterationDuration(name, queryStopwatch.Elapsed.TotalMilliseconds);
            queryStopwatch.Reset();
            
            // Resume the stopwatch
            stopwatch.Start();
        }
        
        stopwatch.Stop();
        return BenchmarkResult.FromIterations(Iterations[name], stopwatch.Elapsed.TotalSeconds, name);
    }
}