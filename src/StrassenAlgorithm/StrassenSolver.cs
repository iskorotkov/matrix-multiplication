using System;
using MatrixTypes;
using Range = MatrixTypes.Range;

namespace StrassenAlgorithm
{
    public class StrassenSolver : ISolver
    {
        public double[,] Multiply(double[,] a, double[,] b) => Multiply(new MatrixView(a), new MatrixView(b)).ToArray();

        public MatrixView Multiply(MatrixView a, MatrixView b)
        {
            var (rows, cols) = (a.Rows, b.Columns);
            (a, b) = PadToEven(a, b);
            return MultiplyRecursive(a, b).Slice(rows, cols);
        }

        private MatrixView MultiplyRecursive(MatrixView a, MatrixView b)
        {
            // a and b are always square matrices of the same dimension => check if contain single cell
            if (a.Rows.Count == 1)
            {
                return new MatrixView(new[,] {{a[0, 0] * b[0, 0]}});
            }

            // a and b are always square matrices of the same dimension => can skip taking max dimension and use number of rows
            (a, b) = PadToEven(a, b, a.Rows.Count);

            var (a11, a12, a21, a22) = Subdivide(a);
            var (b11, b12, b21, b22) = Subdivide(b);

            var m1 = MultiplyRecursive(a12 - a22, b21 + b22);
            var m2 = MultiplyRecursive(a11 + a22, b11 + b22);
            var m3 = MultiplyRecursive(a11 - a21, b11 + b12);
            var m4 = MultiplyRecursive(a11 + a12, b22);
            var m5 = MultiplyRecursive(a11, b12 - b22);
            var m6 = MultiplyRecursive(a22, b21 - b11);
            var m7 = MultiplyRecursive(a21 + a22, b11);

            var c = new MatrixView(new double[a.Rows.Count, b.Columns.Count]);
            var (c11, c12, c21, c22) = Subdivide(c);

            // c11, c12, c21, c22 have the same dimensions
            for (var i = 0; i < c11.Rows.Count; i++)
            {
                for (var j = 0; j < c11.Columns.Count; j++)
                {
                    c11[i, j] = m1[i, j] + m2[i, j] - m4[i, j] + m6[i, j];
                    c12[i, j] = m4[i, j] + m5[i, j];
                    c21[i, j] = m6[i, j] + m7[i, j];
                    c22[i, j] = m2[i, j] - m3[i, j] + m5[i, j] - m7[i, j];
                }
            }

            return c;
        }

        private static (MatrixView, MatrixView) PadToEven(MatrixView a, MatrixView b)
        {
            var dimension = Math.Max(
                Math.Max(a.Rows.Count, a.Columns.Count),
                Math.Max(b.Rows.Count, b.Columns.Count)
            );

            if (dimension % 2 == 1)
            {
                dimension++;
            }

            var range = new Range(0, dimension);
            return (a.Slice(range, range), b.Slice(range, range));
        }

        private static (MatrixView, MatrixView) PadToEven(MatrixView a, MatrixView b, int targetValue)
        {
            if (targetValue % 2 == 1)
            {
                targetValue++;
            }

            var range = new Range(0, targetValue);
            return (a.Slice(range, range), b.Slice(range, range));
        }

        private (MatrixView X11, MatrixView X12, MatrixView X21, MatrixView X22) Subdivide(MatrixView x)
        {
            var (rowStart, rowEnd) = SubdivideRange(x.Rows);
            var (columnStart, columnEnd) = SubdivideRange(x.Columns);
            return (x.Slice(rowStart, columnStart), x.Slice(rowStart, columnEnd),
                x.Slice(rowEnd, columnStart), x.Slice(rowEnd, columnEnd));
        }

        private static (Range Start, Range End) SubdivideRange(Range r)
        {
            var start = new Range(0, r.Count / 2);
            var end = new Range(r.Count / 2, r.Count);

            return (start, end);
        }
    }
}
