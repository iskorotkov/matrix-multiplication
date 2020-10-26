using MathNet.Numerics.LinearAlgebra.Double;
using MatrixTypes;
using TestGenerator;
using Xunit;

namespace StrassenAlgorithm.Tests
{
    public class LargeMatricesOfPowerOf2
    {
        private readonly ISolver _solver = new StrassenSolver();
        private readonly MatrixGenerator _generator = new MatrixGenerator(-100, 100);

        [Theory, InlineData(2, 8, 4), InlineData(8, 16, 4), InlineData(16, 32, 32)]
        public void MultiplyRectangularMatrices(int aRows, int aColumns, int bColumns)
        {
            var (a, b) = _generator.NewMatrixPair(aRows, aColumns, bColumns);

            var expected = DenseMatrix.OfArray(a) * DenseMatrix.OfArray(b);
            var actual = _solver.Multiply(a, b);

            actual.EnsureAlmostEqual(expected.ToArray());
        }

        [Theory, InlineData(2), InlineData(4), InlineData(16), InlineData(32)]
        public void MultiplySquareMatrices(int size)
        {
            var (a, b) = _generator.NewMatrixPair(size, size, size);

            var expected = DenseMatrix.OfArray(a) * DenseMatrix.OfArray(b);
            var actual = _solver.Multiply(a, b);

            actual.EnsureAlmostEqual(expected.ToArray());
        }
    }
}
