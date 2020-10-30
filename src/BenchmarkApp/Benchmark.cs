using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using MatrixTypes;
using NaiveMultiplication;
using StrassenAlgorithm;
using TestGenerator;

namespace BenchmarkApp
{
    public class Benchmark
    {
        private readonly int _iterations;
        private const string Format = "{0,-15}{1,-15}{2,-15}{3,-15:n2}";

        private readonly MatrixGenerator _generator = new MatrixGenerator(1e-6, 1e6);
        private readonly NaiveSolver _naiveSolver = new NaiveSolver();
        private readonly StrassenSolver _strassenSolver = new StrassenSolver();

        public Benchmark(int iterations)
        {
            _iterations = iterations;
        }

        public void StartForSeries(int start, int max, int step)
        {
            var dimensions = GenerateSequence(start, max, step);
            Console.WriteLine("Series benchmark:");
            ExecuteBenchmark(dimensions);
        }

        public void StartForBestCase(int start, int max)
        {
            var dimensions = GenerateSequence(start, max, IsPowerOfTwo);
            Console.WriteLine("Best case benchmark:");
            ExecuteBenchmark(dimensions);
        }

        public void StartForWorstCase(int start, int max)
        {
            var dimensions = GenerateSequence(start, max, IsPowerOfTwoPlusOne);
            Console.WriteLine("Worst case benchmark:");
            ExecuteBenchmark(dimensions);
        }

        private void ExecuteBenchmark(IEnumerable<int> dimensions)
        {
            // ReSharper disable once FormatStringProblem
            Console.WriteLine(Format, "dimension", "naive", "strassen", "strassen/naive");
            foreach (var dimension in dimensions)
            {
                var (a, b) = _generator.NewMatrixPair(dimension, dimension, dimension);

                var naiveTime = BenchmarkSolver(_naiveSolver, _iterations, a, b);
                var strassenTime = BenchmarkSolver(_strassenSolver, _iterations, a, b);

                Console.WriteLine(Format, dimension, naiveTime, strassenTime, strassenTime / naiveTime);
            }
        }

        private double BenchmarkSolver(ISolver solver, int iterations, double[,] a, double[,] b)
        {
            var watch = new Stopwatch();
            watch.Start();

            for (var iteration = 0; iteration < iterations; iteration++)
            {
                var _ = solver.Multiply(a, b);
            }

            watch.Stop();
            return (double) watch.ElapsedMilliseconds / iterations;
        }

        private static IEnumerable<int> GenerateSequence(int start, int max, int step)
        {
            while (start <= max)
            {
                yield return start;
                start += step;
            }
        }

        private static IEnumerable<int> GenerateSequence(int start, int max, Predicate<int> predicate) =>
            GenerateSequence(start, max, 1).Where(value => predicate(value));

        private static bool IsPowerOfTwo(int value)
        {
            while (value > 1)
            {
                if (value % 2 == 1)
                {
                    return false;
                }

                value /= 2;
            }

            return true;
        }

        private static bool IsPowerOfTwoPlusOne(int value) => IsPowerOfTwo(value - 1);
    }
}
