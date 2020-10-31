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

        [Theory, InlineData(32, 128, 64), InlineData(64, 128, 256), InlineData(128, 512, 512)]
        public void MultiplyRectangularMatrices(int aRows, int aColumns, int bColumns)
        {
            var (a, b) = _generator.NewMatrixPair(aRows, aColumns, bColumns);

            var expected = DenseMatrix.OfArray(a) * DenseMatrix.OfArray(b);
            var actual = _solver.Multiply(a, b);

            actual.EnsureAlmostEqual(expected.ToArray());
        }

        [Theory, InlineData(32), InlineData(64), InlineData(128), InlineData(256), InlineData(512)]
        public void MultiplySquareMatrices(int size)
        {
            var (a, b) = _generator.NewMatrixPair(size, size, size);

            var expected = DenseMatrix.OfArray(a) * DenseMatrix.OfArray(b);
            var actual = _solver.Multiply(a, b);

            actual.EnsureAlmostEqual(expected.ToArray());
        }
    }
}
