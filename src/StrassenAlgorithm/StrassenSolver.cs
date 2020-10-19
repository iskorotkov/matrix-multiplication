using MatrixTypes;

namespace StrassenAlgorithm
{
    public class StrassenSolver : ISolver
    {
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

            var (a11, a12, a21, a22) = SubdivideMatrix(a);
            var (b11, b12, b21, b22) = SubdivideMatrix(b);

            var m1 = Multiply(a12 - a22, b21 + b22);
            var m2 = Multiply(a11 + a22, b11 + b22);
            var m3 = Multiply(a11 - a21, b11 + b12);
            var m4 = Multiply(new MatrixView(a11 + a12), b22);
            var m5 = Multiply(a11, new MatrixView(b12 - b22));
            var m6 = Multiply(a22, new MatrixView(b21 - b11));
            var m7 = Multiply(new MatrixView(a21 + a22), b11);

            var result = new double[a.Rows.Count, b.Columns.Count];
            var view = new MatrixView(result);
            var (v11, v12, v21, v22) = SubdivideMatrix(view);

            for (var i = 0; i < v11.Rows.Count; i++)
            {
                for (var j = 0; j < v11.Columns.Count; j++)
                {
                    v11[i, j] = m1[i, j] + m2[i, j] - m4[i, j] + m6[i, j];
                }
            }

            for (var i = 0; i < v12.Rows.Count; i++)
            {
                for (var j = 0; j < v12.Columns.Count; j++)
                {
                    v12[i, j] = m4[i, j] + m5[i, j];
                }
            }

            for (var i = 0; i < v21.Rows.Count; i++)
            {
                for (var j = 0; j < v21.Columns.Count; j++)
                {
                    v21[i, j] = m6[i, j] + m7[i, j];
                }
            }

            for (var i = 0; i < v22.Rows.Count; i++)
            {
                for (var j = 0; j < v22.Columns.Count; j++)
                {
                    v22[i, j] = m2[i, j] - m3[i, j] + m5[i, j] - m7[i, j];
                }
            }

            return result;
        }

        private (MatrixView X11, MatrixView X12, MatrixView X21, MatrixView X22) SubdivideMatrix(MatrixView x)
        {
            var (rowStart, rowEnd) = SubdivideRange(x.Rows);
            var (columnStart, columnEnd) = SubdivideRange(x.Columns);

            var x11 = x.Slice(rowStart, columnStart);
            var x12 = x.Slice(rowStart, columnEnd);
            var x21 = x.Slice(rowEnd, columnStart);
            var x22 = x.Slice(rowEnd, columnEnd);

            return (x11, x12, x21, x22);
        }

        private (Range Start, Range End) SubdivideRange(Range r)
        {
            var start = r.Slice(new Range(0, r.Count / 2));
            var end = r.Slice(new Range(r.Count / 2, r.Count));

            return (start, end);
        }
    }
}
