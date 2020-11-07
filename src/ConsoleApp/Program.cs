using System;
using MathNet.Numerics.LinearAlgebra.Double;
using MatrixParser;
using MatrixTypes;
using NaiveMultiplication;
using StrassenAlgorithm;

namespace ConsoleApp
{
    internal static class Program
    {
        private static void Main()
        {
            var parser = new Parser();
            var (a, b, _) = parser.Parse(Console.In);

            // Print input values
            Console.WriteLine("\nA =");
            Console.WriteLine(a.Format());
            Console.WriteLine("\nB =");
            Console.WriteLine(b.Format());

            // Solve with naive solver
            var naive = new NaiveSolver();
            Console.WriteLine("\nNaive =");
            Console.WriteLine(naive.Multiply(a, b).Format());

            // Solve with Strassen solver
            var strassen = new StrassenSolver();
            Console.WriteLine("\nStrassen =");
            Console.WriteLine(strassen.Multiply(a, b).Format());

            // Solve using MathNet.Numerics
            var reference = DenseMatrix.OfArray(a) * DenseMatrix.OfArray(b);
            var referenceArray = reference.ToArray();
            Console.WriteLine("\nMathNet.Numerics =");
            Console.WriteLine(referenceArray.Format());
        }
    }
}
