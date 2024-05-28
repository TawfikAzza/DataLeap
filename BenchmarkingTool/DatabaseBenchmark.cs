using System.Collections.Concurrent;

namespace BenchmarkingTool;

public abstract class DatabaseBenchmark
{    
    protected readonly ConcurrentDictionary<string, double[]> Iterations;
    
    public DatabaseBenchmark()
    {
        Iterations = new ConcurrentDictionary<string, double[]>();
    }
    
    public async Task SaveIterationDuration(string name, double elapsed)
    {
        Iterations.AddOrUpdate(name, s =>
        {
            var iterations = new double[1];
            iterations[0] = elapsed;
            return iterations;
        }, (s, doubles) =>
        {
            var newIterations = new double[doubles.Length + 1];
            doubles.CopyTo(newIterations, 0);
            newIterations[^1] = elapsed;
            return newIterations;
        });
    }

    public void ShowResults(BenchmarkResult[] results)
    {
        if (results == null || !results.Any()) return;

        string[] headers = new string[]
        {
            "Name", "Avg (ms)", "Min (ms)", "Max (ms)", "10% (ms)", "50% (ms)", "90% (ms)", "QPS (queries/s)"
        };

        int[] columnWidths = new int[headers.Length];
        for (int i = 0; i < headers.Length; i++)
        {
            columnWidths[i] = Math.Max(headers[i].Length, results.Max(result => result.GetType().GetProperty(headers[i])?.GetValue(result, null)?.ToString().Length ?? 0));
        }

        string divider = "+" + string.Join("+", columnWidths.Select(width => new string('-', width + 2))) + "+";
        
        Console.WriteLine(divider);
        Console.WriteLine("| " + string.Join(" | ", headers.Select((header, index) => header.PadRight(columnWidths[index]))) + " |");
        Console.WriteLine(divider);

        foreach (var result in results)
        {
            Console.WriteLine("| " + string.Join(" | ", new string[]
            {
                result.Name.PadRight(columnWidths[0]),
                result.Average.ToString("N2", System.Globalization.CultureInfo.InvariantCulture).PadLeft(columnWidths[1]),
                result.Minimum.ToString("N2", System.Globalization.CultureInfo.InvariantCulture).PadLeft(columnWidths[2]),
                result.Maximum.ToString("N2", System.Globalization.CultureInfo.InvariantCulture).PadLeft(columnWidths[3]),
                result.Percentile10.ToString("N2", System.Globalization.CultureInfo.InvariantCulture).PadLeft(columnWidths[4]),
                result.Percentile50.ToString("N2", System.Globalization.CultureInfo.InvariantCulture).PadLeft(columnWidths[5]),
                result.Percentile90.ToString("N2", System.Globalization.CultureInfo.InvariantCulture).PadLeft(columnWidths[6]),
                result.QueriesPerSecond.ToString("N2", System.Globalization.CultureInfo.InvariantCulture).PadLeft(columnWidths[7])
            }) + " |");
        }

        Console.WriteLine(divider);
    }

    private string? ReadQueryFromFile(string path)
    {
        try
        {
            string queryString = File.ReadAllText(path);
            return queryString;
        } 
        catch (Exception e)
        {
            Console.WriteLine($"Failed to read query from file '{path}': {e.Message}");
            return null;
        }
    }
    
    public Task<BenchmarkResult[]> MeasureQuerySuitePerformanceAsync(IEnumerable<KeyValuePair<string, string>> queries, int iterations)
    {
        BenchmarkResult[] results = new BenchmarkResult[queries.Count()];
        for(var query = 0; query < queries.Count(); query++)
        {
            string? queryString = ReadQueryFromFile(queries.ElementAt(query).Value);
            if (queryString == null)
            {
                Console.WriteLine($"Skipping query '{queries.ElementAt(query).Key}'. Could not load file.");
                continue;
            }
            
            results[query] = MeasureQueryPerformanceAsync(queries.ElementAt(query).Key, queryString, iterations).Result;
        }
        return Task.FromResult(results);
    }

    public abstract Task<BenchmarkResult> MeasureQueryPerformanceAsync(string name, string query, int iterations);
}