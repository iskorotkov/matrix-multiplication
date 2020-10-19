using System;

namespace TestGenerator
{
    public class MatrixGenerator
    {
        private readonly double _min;
        private readonly double _max;
        private readonly Random _random = new Random();

        public MatrixGenerator(double min, double max)
        {
            _min = min;
            _max = max;
        }

        public double[,] NewMatrix(int rows, int columns)
        {
            var result = new double[rows, columns];
            for (var i = 0; i < rows; i++)
            {
                for (var j = 0; j < columns; j++)
                {
                    result[i, j] = NewValue();
                }
            }

            return result;
        }

        public (double[,] A, double[,] B) NewMatrixPair(int aRows, int aColumns, int bColumns)
            => (NewMatrix(aRows, aColumns), NewMatrix(aColumns, bColumns));

        private double NewValue() => _random.NextDouble() * (_max - _min) + _min;
    }
}
