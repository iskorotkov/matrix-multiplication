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
            var (viewA, viewB) = (a, b);

            Console.WriteLine("\nA =");
            Console.WriteLine(viewA.Format());
            Console.WriteLine("\nB =");
            Console.WriteLine(viewB.Format());

            var naive = new NaiveSolver();
            Console.WriteLine("\nNaive =");
            Console.WriteLine(naive.Multiply(viewA, viewB).Format());

            var strassen = new StrassenSolver();
            Console.WriteLine("\nStrassen =");
            Console.WriteLine(strassen.Multiply(viewA, viewB).Format());

            var reference = DenseMatrix.OfArray(a) * DenseMatrix.OfArray(b);
            var referenceArray = reference.ToArray();
            Console.WriteLine("\nMathNet.Numerics =");
            Console.WriteLine(referenceArray.Format());
        }
    }
}
