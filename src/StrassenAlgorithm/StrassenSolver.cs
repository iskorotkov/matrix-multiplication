using MatrixTypes;

namespace StrassenAlgorithm
{
    public class StrassenSolver : ISolver
    {
        private readonly Padder _padder = new Padder();
        private readonly Partitioner _partitioner = new Partitioner();

        public double[,] Multiply(double[,] a, double[,] b) => Multiply(new MatrixView(a), new MatrixView(b)).ToArray();

        public MatrixView Multiply(MatrixView a, MatrixView b) => MultiplyRecursive(a, b).Slice(a.Rows, b.Columns);

        private MatrixView MultiplyRecursive(MatrixView a, MatrixView b)
        {
            if (a.Columns.Count != b.Rows.Count)
            {
                throw new MultiplicationDimensionsMismatchException();
            }

            if (a.Rows.Count == 1 && a.Columns.Count == 1 && b.Rows.Count == 1 && b.Columns.Count == 1)
            {
                return new MatrixView(new[,] {{a[0, 0] * b[0, 0]}});
            }

            (a, b) = _padder.Pad(a, b);

            var (a11, a12, a21, a22) = _partitioner.Subdivide(a);
            var (b11, b12, b21, b22) = _partitioner.Subdivide(b);

            var m1 = MultiplyRecursive(a12 - a22, b21 + b22);
            var m2 = MultiplyRecursive(a11 + a22, b11 + b22);
            var m3 = MultiplyRecursive(a11 - a21, b11 + b12);
            var m4 = MultiplyRecursive(a11 + a12, b22);
            var m5 = MultiplyRecursive(a11, b12 - b22);
            var m6 = MultiplyRecursive(a22, b21 - b11);
            var m7 = MultiplyRecursive(a21 + a22, b11);

            var view = new MatrixView(new double[a.Rows.Count, b.Columns.Count]);
            var (v11, v12, v21, v22) = _partitioner.Subdivide(view);

            v11.ForEach((i, j) => m1[i, j] + m2[i, j] - m4[i, j] + m6[i, j]);
            v12.ForEach((i, j) => m4[i, j] + m5[i, j]);
            v21.ForEach((i, j) => m6[i, j] + m7[i, j]);
            v22.ForEach((i, j) => m2[i, j] - m3[i, j] + m5[i, j] - m7[i, j]);

            return view;
        }
    }
}
