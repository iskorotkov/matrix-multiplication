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
            var (viewA, viewB) = (new MatrixView(a), new MatrixView(b));

            Console.WriteLine("\nA =");
            Console.WriteLine(viewA);
            Console.WriteLine("\nB =");
            Console.WriteLine(viewB);

            var naive = new NaiveSolver();
            Console.WriteLine("\nNaive =");
            Console.WriteLine(naive.Multiply(viewA, viewB));

            var strassen = new StrassenSolver();
            Console.WriteLine("\nStrassen =");
            Console.WriteLine(strassen.Multiply(viewA, viewB));

            var reference = DenseMatrix.OfArray(a) * DenseMatrix.OfArray(b);
            var referenceView = new MatrixView(reference.ToArray());
            Console.WriteLine("\nMathNet.Numerics =");
            Console.WriteLine(referenceView);
        }
    }
}
