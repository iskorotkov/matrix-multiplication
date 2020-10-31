using System;
using MatrixTypes;
using Shouldly;

namespace StrassenAlgorithm.Tests
{
    public static class TestExtensions
    {
        public static void EnsureAlmostEqual(this double[,] actual, double[,] expected)
        {
            var diff = actual.Subtract(expected);
            foreach (var d in diff)
            {
                d.ShouldBeLessThan(Math.Abs(1e-6));
            }
        }
    }
}
