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

        public double this[int row, int column]
        {
            get
            {
                EnsureIndexInRange(row, column);
                return _data[row, column];
            }
            set
            {
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