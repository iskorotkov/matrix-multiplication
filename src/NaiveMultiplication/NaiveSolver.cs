using MatrixTypes;

namespace NaiveMultiplication
{
    public class NaiveSolver : ISolver
    {
        public double[,] Multiply(double[,] a, double[,] b) => Multiply(new MatrixView(a), new MatrixView(b));

        public double[,] Multiply(MatrixView a, MatrixView b)
        {
            if (a.Columns.Count != b.Rows.Count)
            {
                throw new DimensionsMismatchException();
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

            return result;
        }
    }
}
