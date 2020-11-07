using System;
using System.Collections.Generic;
using System.Diagnostics;
using MatrixTypes;
using NaiveMultiplication;
using StrassenAlgorithm;
using TestGenerator;

namespace BenchmarkApp
{
    /// <summary>
    /// Benchmark to measure execution time of ISolver implementations
    /// </summary>
    public class Benchmark
    {
        private readonly int _iterations;
        private const string Format = "{0}\t{1}\t{2}\t{3:n2}";

        private readonly MatrixGenerator _generator = new MatrixGenerator(-100, 100);
        private readonly NaiveSolver _naiveSolver = new NaiveSolver();
        private readonly StrassenSolver _strassenSolver = new StrassenSolver();

        /// <summary>Create benchmark with given amount of <paramref name="iterations"/> per test</summary>
        /// <param name="iterations">Number of times to test a solver with a dimension</param>
        public Benchmark(int iterations)
        {
            _iterations = iterations;
        }

        /// <summary>
        /// Start benchmark for arithmetic progression
        /// </summary>
        /// <param name="start">Initial dimension value</param>
        /// <param name="max">Max dimension value (inclusive)</param>
        /// <param name="step">Arithmetic progression step</param>
        public void StartForSeries(int start, int max, int step)
        {
            var dimensions = GenerateSequence(start, max, step);
            Console.WriteLine("Series benchmark:");
            ExecuteBenchmark(dimensions);
        }

        /// <summary>
        /// Start benchmark for best case scenario (powers of two)
        /// </summary>
        /// <param name="start">Initial dimension value</param>
        /// <param name="max">max dimension value (inclusive)</param>
        public void StartForBestCase(int start, int max)
        {
            var dimensions = GeneratePowersOfTwo(start, max);
            Console.WriteLine("Best case benchmark:");
            ExecuteBenchmark(dimensions);
        }

        /// <summary>
        /// Start benchmark for worst case scenario (powers of two plus one)
        /// </summary>
        /// <param name="start">Initial dimension value</param>
        /// <param name="max">max dimension value (inclusive)</param>
        public void StartForWorstCase(int start, int max)
        {
            var dimensions = GeneratePowersOfTwoPlusOne(start, max);
            Console.WriteLine("Worst case benchmark:");
            ExecuteBenchmark(dimensions);
        }

        /// <summary>
        /// Execute benchmark for given sequence of matrix dimensions
        /// </summary>
        /// <param name="dimensions">Sequence of matrix dimensions</param>
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

        /// <summary>
        /// Run benchmark for given solver and matrices
        /// </summary>
        /// <param name="solver">Solver to run benchmark for</param>
        /// <param name="iterations">Times to repeat matrix multiplication</param>
        /// <param name="a">First matrix</param>
        /// <param name="b">Second matrix</param>
        /// <returns>Execution time in ms</returns>
        private double BenchmarkSolver(ISolver solver, int iterations, double[,] a, double[,] b)
        {
            var watch = new Stopwatch();
            watch.Start();

            for (var iteration = 0; iteration < iterations; iteration++)
            {
                var _ = solver.Multiply(a, b);
            }

            watch.Stop();
            return (double)watch.ElapsedMilliseconds / iterations;
        }

        /// <summary>
        /// Generate arithmetic progression
        /// </summary>
        /// <param name="start">Start value</param>
        /// <param name="max">Max value (inclusive)</param>
        /// <param name="step">Progression step</param>
        /// <returns>Sequence of values in arithmetic progression</returns>
        private static IEnumerable<int> GenerateSequence(int start, int max, int step)
        {
            while (start <= max)
            {
                yield return start;
                start += step;
            }
        }

        /// <summary>
        /// Generate sequence of powers of two
        /// </summary>
        /// <param name="start">Start value</param>
        /// <param name="max">Max value (inclusive)</param>
        /// <returns>Sequence of powers of two between <paramref name="start"/> and <paramref name="max"/> (both inclusive)</returns>
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

        /// <summary>
        /// Generate sequence of powers of two plus one
        /// </summary>
        /// <param name="start">Start value</param>
        /// <param name="max">Max value (inclusive)</param>
        /// <returns>Sequence of powers of two plus one between <paramref name="start"/> and <paramref name="max"/> (both inclusive)</returns>
        private static IEnumerable<int> GeneratePowersOfTwoPlusOne(int start, int max)
        {
            var power = 1;
            while (power <= max)
            {
                if (power >= start - 1)
                {
                    yield return power + 1;
                }

                power *= 2;
            }
        }
    }
}
