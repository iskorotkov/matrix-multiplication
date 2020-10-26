using System;
using System.Text;
using MatrixTypes.Exceptions;

namespace MatrixTypes
{
    public readonly struct MatrixView
    {
        private readonly double[,] _data;

        public Range Rows { get; }
        public Range Columns { get; }

        public MatrixView(double[,] data)
            : this(data, new Range(0, data.GetUpperBound(0) + 1), new Range(0, data.GetUpperBound(1) + 1))
        {
        }

        public MatrixView(double[,] data, Range rows, Range columns)
        {
            _data = data;
            Rows = rows;
            Columns = columns;
        }

        public MatrixView Slice(Range rows, Range columns, bool allowExtending = false) =>
            new MatrixView(_data, Rows.Slice(rows, allowExtending), Columns.Slice(columns, allowExtending));

        public void ForEach(Func<int, int, double> f)
        {
            for (var i = 0; i < Rows.Count; i++)
            {
                for (var j = 0; j < Columns.Count; j++)
                {
                    this[i, j] = f(i, j);
                }
            }
        }

        public static MatrixView operator +(MatrixView a, MatrixView b)
        {
            if (a.Rows.Count != b.Rows.Count)
            {
                throw new AdditionDimensionsMismatchException();
            }

            if (a.Columns.Count != b.Columns.Count)
            {
                throw new AdditionDimensionsMismatchException();
            }

            var rows = a.Rows.Count;
            var columns = a.Columns.Count;
            var result = new double[rows, columns];
            for (var row = 0; row < rows; row++)
            {
                for (var column = 0; column < columns; column++)
                {
                    result[row, column] = a[row, column] + b[row, column];
                }
            }

            return new MatrixView(result);
        }

        public static MatrixView operator -(MatrixView a, MatrixView b)
        {
            if (a.Rows.Count != b.Rows.Count)
            {
                throw new AdditionDimensionsMismatchException();
            }

            if (a.Columns.Count != b.Columns.Count)
            {
                throw new AdditionDimensionsMismatchException();
            }

            var rows = a.Rows.Count;
            var columns = a.Columns.Count;
            var result = new double[rows, columns];
            for (var row = 0; row < rows; row++)
            {
                for (var column = 0; column < columns; column++)
                {
                    result[row, column] = a[row, column] - b[row, column];
                }
            }

            return new MatrixView(result);
        }

        public double this[int row, int column]
        {
            get
            {
                row += Rows.Start;
                column += Columns.Start;

                EnsureIndexInRange(row, column);
                return IsCellVirtual(row, column) ? 0 : _data[row, column];
            }
            set
            {
                row += Rows.Start;
                column += Columns.Start;

                EnsureIndexInRange(row, column);
                if (IsCellVirtual(row, column))
                {
                    throw new MatrixVirtualWriteException();
                }

                _data[row, column] = value;
            }
        }

        private void EnsureIndexInRange(int row, int column)
        {
            if (!Rows.Contains(row))
            {
                throw new MatrixAccessException(nameof(row));
            }

            if (!Columns.Contains(column))
            {
                throw new MatrixAccessException(nameof(column));
            }
        }

        private bool IsCellVirtual(int row, int column)
            => row < 0
               || row > _data.GetUpperBound(0)
               || column < 0
               || column > _data.GetUpperBound(1);

        public double[,] ToArray()
        {
            var result = new double[Rows.Count, Columns.Count];
            for (var row = 0; row < Rows.Count; row++)
            {
                for (var col = 0; col < Columns.Count; col++)
                {
                    result[row, col] = this[row, col];
                }
            }

            return result;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            for (var i = 0; i < Rows.Count; i++)
            {
                builder.Append("| ");
                for (var j = 0; j < Columns.Count; j++)
                {
                    builder.Append(this[i, j]);
                    if (j != Columns.Count - 1)
                    {
                        builder.Append(", ");
                    }
                }

                builder.Append(" |");
                if (i != Rows.Count - 1)
                {
                    builder.Append('\n');
                }
            }

            return builder.ToString();
        }
    }
}
