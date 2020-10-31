﻿using MatrixTypes;

namespace NaiveMultiplication
{
    public class NaiveSolver : ISolver
    {
        public double[,] Multiply(double[,] a, double[,] b)
        {
            var rows = a.GetLength(0);
            var columns = b.GetLength(1);
            var length = a.GetLength(1);

            var result = new double[rows, columns];
            for (var row = 0; row < rows; row++)
            {
                for (var column = 0; column < columns; column++)
                {
                    for (var i = 0; i < length; i++)
                    {
                        result[row, column] += a[row, i] * b[i, column];
                    }
                }
            }

            return result;
        }
    }
}
