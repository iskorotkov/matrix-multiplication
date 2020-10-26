using MatrixTypes;

namespace StrassenAlgorithm
{
    public class StrassenSolver : ISolver
    {
        private readonly IPartitioner _partitioner = new Partitioner();

        public double[,] Multiply(double[,] a, double[,] b) => Multiply(new MatrixView(a), new MatrixView(b));

        public double[,] Multiply(MatrixView a, MatrixView b)
        {
            if (a.Columns.Count != b.Rows.Count)
            {
                throw new MultiplicationDimensionsMismatchException();
            }

            if (a.Rows.Count == 1 && a.Columns.Count == 1 && b.Rows.Count == 1 && b.Columns.Count == 1)
            {
                return new[,] {{a[0, 0] * b[0, 0]}};
            }

            var (a11, a12, a21, a22) = _partitioner.Subdivide(a);
            var (b11, b12, b21, b22) = _partitioner.Subdivide(b);

            var m1 = Multiply(a12 - a22, b21 + b22);
            var m2 = Multiply(a11 + a22, b11 + b22);
            var m3 = Multiply(a11 - a21, b11 + b12);
            var m4 = Multiply(new MatrixView(a11 + a12), b22);
            var m5 = Multiply(a11, new MatrixView(b12 - b22));
            var m6 = Multiply(a22, new MatrixView(b21 - b11));
            var m7 = Multiply(new MatrixView(a21 + a22), b11);

            var result = new double[a.Rows.Count, b.Columns.Count];
            var view = new MatrixView(result);
            var (v11, v12, v21, v22) = _partitioner.Subdivide(view);

            v11.ForEach((i, j) => m1[i, j] + m2[i, j] - m4[i, j] + m6[i, j]);
            v12.ForEach((i, j) => m4[i, j] + m5[i, j]);
            v21.ForEach((i, j) => m6[i, j] + m7[i, j]);
            v22.ForEach((i, j) => m2[i, j] - m3[i, j] + m5[i, j] - m7[i, j]);

            return result;
        }
    }
}
