using System;
using System.Collections.Generic;
using System.Diagnostics;
using MatrixTypes;
using NaiveMultiplication;
using StrassenAlgorithm;
using TestGenerator;

namespace BenchmarkApp
{
    public class Benchmark
    {
        private readonly int _iterations;
        private const string Format = "{0}\t{1}\t{2}\t{3:n2}";

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
            var dimensions = GeneratePowersOfTwo(start, max);
            Console.WriteLine("Best case benchmark:");
            ExecuteBenchmark(dimensions);
        }

        public void StartForWorstCase(int start, int max)
        {
            var dimensions = GeneratePowersOfTwoPlusOne(start, max);
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

        private static IEnumerable<int> GeneratePowersOfTwo(int start, int max)
        {
            var power = 1;
            while (power <= max)
            {
                if (power >= start)
                {
                    yield return power;
                }

                power *= 2;
            }
        }

        private static IEnumerable<int> GeneratePowersOfTwoPlusOne(int start, int max)
        {
            var power = 1;
            while (power <= max)
            {
                if (power >= start)
                {
                    yield return power + 1;
                }

                power *= 2;
            }
        }
    }
}
