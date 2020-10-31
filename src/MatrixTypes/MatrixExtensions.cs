using System.Text;

namespace MatrixTypes
{
    public static class MatrixExtensions
    {
        public static double[,] Add(this double[,] a, double[,] b)
        {
            var (rows, cols) = (a.GetLength(0), a.GetLength(1));
            var result = new double[rows, cols];
            for (var i = 0; i < rows; i++)
            {
                for (var j = 0; j < cols; j++)
                {
                    result[i, j] = a[i, j] + b[i, j];
                }
            }

            return result;
        }

        public static double[,] Subtract(this double[,] a, double[,] b)
        {
            var (rows, cols) = (a.GetLength(0), a.GetLength(1));
            var result = new double[rows, cols];
            for (var i = 0; i < rows; i++)
            {
                for (var j = 0; j < cols; j++)
                {
                    result[i, j] = a[i, j] - b[i, j];
                }
            }

            return result;
        }

        public static string Format(this double[,] x)
        {
            var (rows, cols) = (x.GetLength(0), x.GetLength(1));
            var builder = new StringBuilder();
            for (var i = 0; i < rows; i++)
            {
                builder.Append("| ");
                for (var j = 0; j < cols; j++)
                {
                    builder.Append(x[i, j]);
                    if (j != cols - 1)
                    {
                        builder.Append(", ");
                    }
                }

                builder.Append(" |");
                if (i != rows - 1)
                {
                    builder.Append('\n');
                }
            }

            return builder.ToString();
        }
    }
}
