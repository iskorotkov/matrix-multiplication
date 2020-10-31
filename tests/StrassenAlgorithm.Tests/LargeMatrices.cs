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

        [Theory, InlineData(50), InlineData(100), InlineData(200), InlineData(500)]
        public void MultiplySquareMatrices(int size)
        {
            var (a, b) = _generator.NewMatrixPair(size, size, size);
            var expected = DenseMatrix.OfArray(a) * DenseMatrix.OfArray(b);
            _solver.Multiply(a, b).EnsureAlmostEqual(expected.ToArray());
        }

        [Theory, InlineData(50, 100, 150), InlineData(100, 150, 100), InlineData(200, 250, 150), InlineData(400, 350, 250)]
        public void MultiplyRectangularMatrices(int aRows, int aColumns, int bColumns)
        {
            var (a, b) = _generator.NewMatrixPair(aRows, aColumns, bColumns);
            var expected = DenseMatrix.OfArray(a) * DenseMatrix.OfArray(b);
            _solver.Multiply(a, b).EnsureAlmostEqual(expected.ToArray());
        }
    }
}
