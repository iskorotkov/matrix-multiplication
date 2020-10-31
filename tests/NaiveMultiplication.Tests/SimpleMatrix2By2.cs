using MatrixTypes;
using Xunit;
using Shouldly;

namespace NaiveMultiplication.Tests
{
    public class SimpleMatrix2By2
    {
        private readonly ISolver _naiveSolver = new NaiveSolver();

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
            _naiveSolver.Multiply(_a, _b).ShouldBe(_result);
        }
    }
}
