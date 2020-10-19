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

        public MatrixView Slice(Range rows, Range columns) =>
            new MatrixView(_data, Rows.Slice(rows), Columns.Slice(columns));

        public static double[,] operator +(MatrixView a, MatrixView b)
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

            return result;
        }

        public static double[,] operator -(MatrixView a, MatrixView b)
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

            return result;
        }

        public double this[int row, int column]
        {
            get
            {
                row += Rows.Start;
                column += Columns.Start;

                EnsureIndexInRange(row, column);
                return _data[row, column];
            }
            set
            {
                row += Rows.Start;
                column += Columns.Start;

                EnsureIndexInRange(row, column);
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
    }
}
