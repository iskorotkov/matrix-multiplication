namespace BenchmarkApp
{
    internal static class Program
    {
        private static void Main()
        {
            var benchmark = new Benchmark(5);
            //benchmark.StartForSeries(50, 1000, 50);
            benchmark.StartForBestCase(64, 512);
            benchmark.StartForWorstCase(65, 513);
        }
    }
}
