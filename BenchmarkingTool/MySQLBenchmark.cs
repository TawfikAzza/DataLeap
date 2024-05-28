using System.Data;
using System.Diagnostics;
using MySql.Data.MySqlClient;

namespace BenchmarkingTool;

public class MySQLBenchmark : DatabaseBenchmark
{
    private readonly string _connectionString;
    
    public MySQLBenchmark(string connectionString)
    {
        _connectionString = connectionString;
    }
    
    public override Task<BenchmarkResult> MeasureQueryPerformanceAsync(string name, string query, int iterations)
    {
        using var connection = new MySqlConnection(_connectionString);
        connection.Open();
        
        using var command = connection.CreateCommand();
        command.CommandText = query;
        command.CommandType = CommandType.Text;
        command.Prepare();
        
        var queryStopwatch = new Stopwatch();
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        
        for (var i = 0; i < iterations; i++)
        {
            // Measure the time it takes to execute the query
            queryStopwatch.Start();
            command.ExecuteNonQuery();
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