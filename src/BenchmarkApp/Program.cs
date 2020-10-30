namespace BenchmarkApp
{
    internal static class Program
    {
        private static void Main()
        {
            var benchmark = new Benchmark(1);
            benchmark.StartForSeries(5, 256, 5);
            benchmark.StartForBestCase(1, 512);
            benchmark.StartForWorstCase(1, 257);
        }
    }
}
