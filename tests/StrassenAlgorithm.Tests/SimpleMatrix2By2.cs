using MatrixTypes;
using Shouldly;
using Xunit;

namespace StrassenAlgorithm.Tests
{
    public class SimpleMatrix2By2
    {
        private readonly ISolver _solver = new StrassenSolver();

        private readonly double[,] _a =
        {
            {1, 2},
            {3, 4}
        };

        private readonly double[,] _b =
        {
            {5, 6},
            {7, 8}
        };

        private readonly double[,] _result =
        {
            {1 * 5 + 2 * 7, 1 * 6 + 2 * 8},
            {3 * 5 + 4 * 7, 3 * 6 + 4 * 8}
        };

        [Fact]
        public void MultiplicationGivesCorrectResult()
        {
            _solver.Multiply(_a, _b).ShouldBe(_result);
        }

        [Fact]
        public void MultiplicationWithViewsGivesCorrectResult()
        {
            _solver.Multiply(new MatrixView(_a), new MatrixView(_b)).ShouldBe(_result);
        }
    }
}
