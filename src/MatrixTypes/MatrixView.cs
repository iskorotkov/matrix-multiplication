using System.Text;

namespace MatrixTypes
{
    public readonly struct MatrixView
    {
        private readonly double[,] _data;

        public Range Rows { get; }
        public Range Columns { get; }

        public MatrixView(double[,] data)
            : this(data, new Range(0, data.GetLength(0)), new Range(0, data.GetLength(1)))
        {
        }

        private MatrixView(double[,] data, Range rows, Range columns)
        {
            _data = data;
            Rows = rows;
            Columns = columns;
        }

        public MatrixView Slice(Range rows, Range columns) =>
            new MatrixView(_data, Rows.Slice(rows), Columns.Slice(columns));

        public static MatrixView operator +(MatrixView a, MatrixView b)
        {
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
                return IsCellVirtual(row, column) ? 0 : _data[row, column];
            }
            set => _data[row + Rows.Start, column + Columns.Start] = value;
        }

        private bool IsCellVirtual(int row, int column)
            => row < 0 || row >= _data.GetLength(0) || column < 0 || column >= _data.GetLength(1);

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
