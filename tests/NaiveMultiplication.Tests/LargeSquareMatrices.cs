using MathNet.Numerics.LinearAlgebra.Double;
using MatrixTypes;
using Shouldly;
using TestGenerator;
using Xunit;

namespace NaiveMultiplication.Tests
{
    public class LargeSquareMatrices
    {
        private readonly ISolver _solver = new NaiveSolver();
        private readonly MatrixGenerator _generator = new MatrixGenerator(-100, 100);

        [Theory, InlineData(5), InlineData(10), InlineData(20), InlineData(400), InlineData(561)]
        public void MultiplySquareMatrices(int size)
        {
            var (a, b) = _generator.NewMatrixPair(size, size, size);
            var expected = DenseMatrix.OfArray(a) * DenseMatrix.OfArray(b);
            _solver.Multiply(a, b).ShouldBe(expected.ToArray());
        }

        [Theory, InlineData(5, 10, 15), InlineData(10, 15, 10), InlineData(20, 25, 15), InlineData(431, 399, 530)]
        public void MultiplyRectangularMatrices(int aRows, int aColumns, int bColumns)
        {
            var (a, b) = _generator.NewMatrixPair(aRows, aColumns, bColumns);
            var expected = DenseMatrix.OfArray(a) * DenseMatrix.OfArray(b);
            _solver.Multiply(a, b).ShouldBe(expected.ToArray());
        }
    }
}
