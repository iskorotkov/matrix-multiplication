using MatrixTypes;
using MatrixTypes.Exceptions;

namespace StrassenAlgorithm
{
    public class StrassenSolver : ISolver
    {
        private readonly Padder _padder = new Padder();
        private readonly Partitioner _partitioner = new Partitioner();

        public double[,] Multiply(double[,] a, double[,] b) => Multiply(new MatrixView(a), new MatrixView(b)).ToArray();

        public MatrixView Multiply(MatrixView a, MatrixView b)
        {
            var (rows, cols) = (a.Rows, b.Columns);
            (a, b) = _padder.PadToEven(a, b);
            return MultiplyRecursive(a, b).Slice(rows, cols);
        }

        private MatrixView MultiplyRecursive(MatrixView a, MatrixView b)
        {
            if (a.Columns.Count != b.Rows.Count)
            {
                throw new MultiplicationDimensionsMismatchException();
            }

            // a and b are always square matrices of the same dimension => check if contain single cell
            if (a.Rows.Count == 1)
            {
                return new MatrixView(new[,] {{a[0, 0] * b[0, 0]}});
            }

            // a and b are always square matrices of the same dimension => can skip taking max dimension and use number of rows
            (a, b) = _padder.PadToEven(a, b, a.Rows.Count);

            var (a11, a12, a21, a22) = _partitioner.Subdivide(a);
            var (b11, b12, b21, b22) = _partitioner.Subdivide(b);

            var m1 = MultiplyRecursive(a12 - a22, b21 + b22);
            var m2 = MultiplyRecursive(a11 + a22, b11 + b22);
            var m3 = MultiplyRecursive(a11 - a21, b11 + b12);
            var m4 = MultiplyRecursive(a11 + a12, b22);
            var m5 = MultiplyRecursive(a11, b12 - b22);
            var m6 = MultiplyRecursive(a22, b21 - b11);
            var m7 = MultiplyRecursive(a21 + a22, b11);

            var c = new MatrixView(new double[a.Rows.Count, b.Columns.Count]);
            var (c11, c12, c21, c22) = _partitioner.Subdivide(c);

            c11.ForEach((i, j) => m1[i, j] + m2[i, j] - m4[i, j] + m6[i, j]);
            c12.ForEach((i, j) => m4[i, j] + m5[i, j]);
            c21.ForEach((i, j) => m6[i, j] + m7[i, j]);
            c22.ForEach((i, j) => m2[i, j] - m3[i, j] + m5[i, j] - m7[i, j]);

            return c;
        }
    }
}
