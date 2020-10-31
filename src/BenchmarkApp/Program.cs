namespace BenchmarkApp
{
    internal static class Program
    {
        private static void Main()
        {
            var benchmark = new Benchmark(10);
            benchmark.StartForSeries(50, 1000, 50);
            benchmark.StartForBestCase(64, 1024);
            benchmark.StartForWorstCase(65, 1025);
        }
    }
}
