using System;
using MatrixTypes;

namespace StrassenAlgorithm
{
    public readonly struct MatrixView
    {
        public readonly double[,] Values;
        public readonly int StartRow;
        public readonly int Rows;
        public readonly int StartColumn;
        public readonly int Columns;
        public readonly int RealRows;
        public readonly int RealColumns;

        public MatrixView(double[,] values, int startRow, int startColumn, int rows, int columns, int realRows,
            int realColumns)
        {
            Values = values;
            StartRow = startRow;
            Rows = rows;
            StartColumn = startColumn;
            Columns = columns;
            RealRows = realRows;
            RealColumns = realColumns;
        }
    }

    public class StrassenSolver : ISolver
    {
        public double[,] Multiply(double[,] a, double[,] b)
        {
            var (rows, cols) = (a.GetLength(0), b.GetLength(1));

            var dimension = Math.Max(
                Math.Max(a.GetLength(0), a.GetLength(1)),
                Math.Max(b.GetLength(0), b.GetLength(1))
            );
            var desiredDimension = CalculateDesiredDimension(dimension);

            MatrixView viewA, viewB;
            if (a.GetLength(0) != desiredDimension || a.GetLength(1) != desiredDimension)
            {
                viewA = new MatrixView(a, 0, 0, desiredDimension, desiredDimension, a.GetLength(0), a.GetLength(1));
            }
            else
            {
                viewA = new MatrixView(a, 0, 0, desiredDimension, desiredDimension, desiredDimension, desiredDimension);
            }

            if (b.GetLength(0) != desiredDimension || b.GetLength(1) != desiredDimension)
            {
                viewB = new MatrixView(b, 0, 0, desiredDimension, desiredDimension, b.GetLength(0), b.GetLength(1));
            }
            else
            {
                viewB = new MatrixView(b, 0, 0, desiredDimension, desiredDimension, desiredDimension, desiredDimension);
            }

            var result = MultiplyRecursive(viewA, viewB);
            var shrinked = new MatrixView(result, 0, 0, rows, cols, rows, cols);
            return ToArray(shrinked);
        }

        private static double[,] MultiplyRecursive(MatrixView a, MatrixView b)
        {
            // a and b are always square matrices of the same dimension => check if contain single cell
            if (a.Rows == 1)
            {
                return new[,] {{a.Values[a.StartRow, a.StartColumn] * b.Values[b.StartRow, b.StartColumn]}};
            }

            // a and b are always square matrices of the same dimension => can skip taking max dimension and use number of rows
            var actualDimension = a.Rows;
            var desiredDimension = CalculateDesiredDimension(actualDimension);
            if (actualDimension != desiredDimension)
            {
                (a, b) = (Extend(a, desiredDimension, desiredDimension),
                    Extend(b, desiredDimension, desiredDimension));
            }

            var (a11, a12, a21, a22) = SubdivideSquareMatrix(a);
            var (b11, b12, b21, b22) = SubdivideSquareMatrix(b);

            var m1 = MultiplyRecursive(Subtract(a12, a22), Add(b21, b22));
            var m2 = MultiplyRecursive(Add(a11, a22), Add(b11, b22));
            var m3 = MultiplyRecursive(Subtract(a11, a21), Add(b11, b12));
            var m4 = MultiplyRecursive(Add(a11, a12), b22);
            var m5 = MultiplyRecursive(a11, Subtract(b12, b22));
            var m6 = MultiplyRecursive(a22, Subtract(b21, b11));
            var m7 = MultiplyRecursive(Add(a21, a22), b11);

            var c = new double[desiredDimension, desiredDimension];
            var halfDimension = desiredDimension / 2;

            for (var i = 0; i < halfDimension; i++)
            {
                for (var j = 0; j < halfDimension; j++)
                {
                    c[i, j] = m1[i, j] + m2[i, j] - m4[i, j] + m6[i, j];
                    c[i, halfDimension + j] = m4[i, j] + m5[i, j];
                    c[halfDimension + i, j] = m6[i, j] + m7[i, j];
                    c[halfDimension + i, halfDimension + j] = m2[i, j] - m3[i, j] + m5[i, j] - m7[i, j];
                }
            }

            return c;
        }

        private static double[,] ToArray(MatrixView x)
        {
            var result = new double[x.Rows, x.Columns];
            for (var i = 0; i < x.Rows; i++)
            {
                for (var j = 0; j < x.Columns; j++)
                {
                    result[i, j] = x.Values[x.StartRow + i, x.StartColumn + j];
                }
            }

            return result;
        }

        private static MatrixView Extend(MatrixView x, int rows, int columns) =>
            new MatrixView(x.Values, 0, 0, rows, columns, x.RealRows, x.RealColumns);

        private static MatrixView Add(MatrixView a, MatrixView b)
        {
            var result = new double[a.Rows, b.Columns];
            var (realRows, realColumns) = (Math.Min(a.RealRows, b.RealRows), Math.Min(a.RealColumns, b.RealColumns));
            for (var i = 0; i < realRows; i++)
            {
                for (var j = 0; j < realColumns; j++)
                {
                    result[i, j] = a.Values[a.StartRow + i, a.StartColumn + j] +
                                   b.Values[b.StartRow + i, b.StartColumn + j];
                }

                for (var j = realColumns; j < b.Columns; j++)
                {
                    result[i, j] = 0d;
                }
            }

            for (var i = realRows; i < a.Rows; i++)
            {
                for (var j = 0; j < b.Columns; j++)
                {
                    result[i, j] = 0d;
                }
            }

            return new MatrixView(result, 0, 0, a.Rows, b.Columns, a.Rows, b.Columns);
        }

        private static MatrixView Subtract(MatrixView a, MatrixView b)
        {
            var result = new double[a.Rows, b.Columns];
            var (realRows, realColumns) = (Math.Min(a.RealRows, b.RealRows), Math.Min(a.RealColumns, b.RealColumns));
            for (var i = 0; i < realRows; i++)
            {
                for (var j = 0; j < realColumns; j++)
                {
                    result[i, j] = a.Values[a.StartRow + i, a.StartColumn + j] -
                                   b.Values[b.StartRow + i, b.StartColumn + j];
                }

                for (var j = realColumns; j < b.Columns; j++)
                {
                    result[i, j] = 0d;
                }
            }

            for (var i = realRows; i < a.Rows; i++)
            {
                for (var j = 0; j < b.Columns; j++)
                {
                    result[i, j] = 0d;
                }
            }

            return new MatrixView(result, 0, 0, a.Rows, b.Columns, a.Rows, b.Columns);
        }

        private static MatrixView Shrink(MatrixView x, int rows, int columns, int rowsOffset, int columnsOffset) =>
            new MatrixView(x.Values, rowsOffset, columnsOffset, rows, columns,
                Math.Min(Math.Abs(x.RealRows - rowsOffset), rows),
                Math.Min(Math.Abs(x.RealColumns - columnsOffset), columns));

        private static int CalculateDesiredDimension(int dimension) => dimension % 2 == 0 ? dimension : dimension + 1;

        private static (MatrixView X11, MatrixView X12, MatrixView X21, MatrixView X22) SubdivideSquareMatrix(
            MatrixView x)
        {
            var halfSize = x.Rows / 2;
            return (
                Shrink(x, halfSize, halfSize, 0, 0),
                Shrink(x, halfSize, halfSize, 0, halfSize),
                Shrink(x, halfSize, halfSize, halfSize, 0),
                Shrink(x, halfSize, halfSize, halfSize, halfSize)
            );
        }
    }
}
