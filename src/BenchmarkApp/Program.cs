namespace BenchmarkApp
{
    internal static class Program
    {
        private static void Main()
        {
            var benchmark = new Benchmark(1);
            benchmark.StartForSeries(5, 512, 5);
            benchmark.StartForBestCase(1, 2048);
            benchmark.StartForWorstCase(1, 1025);
        }
    }
}
