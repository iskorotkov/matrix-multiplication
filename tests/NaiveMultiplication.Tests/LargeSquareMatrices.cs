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

        [Theory, InlineData(5), InlineData(10), InlineData(20)]
        public void MultiplyMatrices(int size)
        {
            var (a, b) = _generator.NewMatrixPair(size, size, size);
            var expected = DenseMatrix.OfArray(a) * DenseMatrix.OfArray(b);
            _solver.Multiply(a, b).ShouldBe(expected.ToArray());
        }
    }
}
