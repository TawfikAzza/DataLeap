namespace BenchmarkingTool;

public class BenchmarkResult
{
    public string? Name { get; set; }
    public double Average { get; set; }
    public double Minimum { get; set; }
    public double Maximum { get; set; }
    public double Percentile10 { get; set; }
    public double Percentile50 { get; set; }
    public double Percentile90 { get; set; }
    public double QueriesPerSecond { get; set; }
    
    public static Task<BenchmarkResult> FromIterations(double[] iterations, double duration, string? name = null)
    {
        var result = new BenchmarkResult
        {
            Name = name,
            Average = iterations.Average(),
            Minimum = iterations.Min(),
            Maximum = iterations.Max(),
            Percentile10 = Percentile(iterations, 10),
            Percentile50 = Percentile(iterations, 50),
            Percentile90 = Percentile(iterations, 90),
            QueriesPerSecond = (iterations.Length / duration)
        };
        
        return Task.FromResult(result);
    }
    
    private static double Percentile(double[] sequence, int percentile)
    {
        var sorted = sequence.OrderBy(x => x).ToArray();
        var n = sorted.Length;
        var p = (n - 1) * percentile / 100.0;
        var k = (int) p;
        var d = p - k;
        return sorted[k] + d * (sorted[k + 1] - sorted[k]);
    }
}