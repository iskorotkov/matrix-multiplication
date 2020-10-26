using MatrixTypes;
using MatrixTypes.Exceptions;

namespace NaiveMultiplication
{
    public class NaiveSolver : ISolver
    {
        public double[,] Multiply(double[,] a, double[,] b) => Multiply(new MatrixView(a), new MatrixView(b)).ToArray();

        public MatrixView Multiply(MatrixView a, MatrixView b)
        {
            if (a.Columns.Count != b.Rows.Count)
            {
                throw new MultiplicationDimensionsMismatchException();
            }

            var rows = a.Rows.Count;
            var columns = b.Columns.Count;
            var result = new double[rows, columns];
            for (var row = 0; row < rows; row++)
            {
                for (var column = 0; column < columns; column++)
                {
                    for (var i = 0; i < a.Columns.Count; i++)
                    {
                        result[row, column] += a[row, i] * b[i, column];
                    }
                }
            }

            return new MatrixView(result);
        }
    }
}
