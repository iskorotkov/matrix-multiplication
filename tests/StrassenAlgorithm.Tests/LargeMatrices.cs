using MathNet.Numerics.LinearAlgebra.Double;
using MatrixTypes;
using Shouldly;
using TestGenerator;
using Xunit;

namespace StrassenAlgorithm.Tests
{
    public class LargeMatrices
    {
        private readonly ISolver _solver = new StrassenSolver();
        private readonly MatrixGenerator _generator = new MatrixGenerator(-100, 100);

        [Theory, InlineData(5), InlineData(10), InlineData(20)]
        public void MultiplySquareMatrices(int size)
        {
            var (a, b) = _generator.NewMatrixPair(size, size, size);
            var expected = DenseMatrix.OfArray(a) * DenseMatrix.OfArray(b);
            _solver.Multiply(a, b).ShouldBe(expected.ToArray());
        }

        [Theory, InlineData(5, 10, 15), InlineData(10, 15, 10), InlineData(20, 25, 15)]
        public void MultiplyRectangularMatrices(int aRows, int aColumns, int bColumns)
        {
            var (a, b) = _generator.NewMatrixPair(aRows, aColumns, bColumns);
            var expected = DenseMatrix.OfArray(a) * DenseMatrix.OfArray(b);
            _solver.Multiply(a, b).ShouldBe(expected.ToArray());
        }
    }
}
