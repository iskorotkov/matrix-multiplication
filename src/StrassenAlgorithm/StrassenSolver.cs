using System;
using MatrixTypes;
using NaiveMultiplication;

namespace StrassenAlgorithm
{
    public class StrassenSolver : ISolver
    {
        private readonly NaiveSolver _naiveSolver = new NaiveSolver();

        public double[,] Multiply(double[,] a, double[,] b)
        {
            var (rows, cols) = (a.GetLength(0), b.GetLength(1));

            var dimension = Math.Max(
                Math.Max(a.GetLength(0), a.GetLength(1)),
                Math.Max(b.GetLength(0), b.GetLength(1))
            );
            dimension = CalculateDesiredDimension(dimension);
            if (dimension <= 64)
            {
                return _naiveSolver.Multiply(a, b);
            }

            if (a.GetLength(0) != dimension || a.GetLength(1) != dimension)
            {
                a = Extend(a, dimension);
            }

            if (b.GetLength(0) != dimension || b.GetLength(1) != dimension)
            {
                b = Extend(b, dimension);
            }

            var result = MultiplyRecursive(a, b);
            return Shrink(result, rows, cols, 0, 0);
        }

        private double[,] MultiplyRecursive(double[,] a, double[,] b)
        {
            // a and b are always square matrices of the same dimension
            if (a.GetLength(0) <= 64)
            {
                return _naiveSolver.Multiply(a, b);
            }

            // a and b are always square matrices of the same dimension => can skip taking max dimension and use number of rows
            var dimension = a.GetLength(0);
            dimension = CalculateDesiredDimension(dimension);

            var (a11, a12, a21, a22) = SubdivideSquareMatrix(a, dimension);
            var (b11, b12, b21, b22) = SubdivideSquareMatrix(b, dimension);

            var m1 = MultiplyRecursive(a12.Subtract(a22), b21.Add(b22));
            var m2 = MultiplyRecursive(a11.Add(a22), b11.Add(b22));
            var m3 = MultiplyRecursive(a11.Subtract(a21), b11.Add(b12));
            var m4 = MultiplyRecursive(a11.Add(a12), b22);
            var m5 = MultiplyRecursive(a11, b12.Subtract(b22));
            var m6 = MultiplyRecursive(a22, b21.Subtract(b11));
            var m7 = MultiplyRecursive(a21.Add(a22), b11);

            var c = new double[dimension, dimension];
            var halfDimension = dimension / 2;

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

        private static double[,] Extend(double[,] x, int dimension)
        {
            var result = new double[dimension, dimension];
            for (var i = 0; i < x.GetLength(0); i++)
            {
                for (var j = 0; j < x.GetLength(1); j++)
                {
                    result[i, j] = x[i, j];
                }

                for (var j = x.GetLength(1); j < dimension; j++)
                {
                    result[i, j] = 0d;
                }
            }

            for (var i = x.GetLength(0); i < dimension; i++)
            {
                for (var j = 0; j < dimension; j++)
                {
                    result[i, j] = 0d;
                }
            }

            return result;
        }

        private static double[,] Shrink(double[,] x, int rows, int columns, int rowsOffset, int columnsOffset)
        {
            var result = new double[rows, columns];
            for (var i = 0; i < rows; i++)
            {
                for (var j = 0; j < columns; j++)
                {
                    result[i, j] = x[rowsOffset + i, columnsOffset + j];
                }
            }

            return result;
        }

        private static int CalculateDesiredDimension(int dimension) => dimension % 2 == 0 ? dimension : dimension + 1;

        private static (double[,], double[,], double[,], double[,]) SubdivideSquareMatrix(double[,] x, int dimension)
        {
            var halfDimension = dimension / 2;
            var x11 = new double[halfDimension, halfDimension];
            var x12 = new double[halfDimension, halfDimension];
            var x21 = new double[halfDimension, halfDimension];
            var x22 = new double[halfDimension, halfDimension];

            var xDimension = x.GetLength(0);
            for (var i = 0; i < halfDimension; i++)
            {
                for (var j = 0; j < halfDimension; j++)
                {
                    x11[i, j] = x[i, j];
                }

                for (var j = 0; j < xDimension - halfDimension; j++)
                {
                    x12[i, j] = x[i, halfDimension + j];
                }
            }

            for (var i = 0; i < xDimension - halfDimension; i++)
            {
                for (var j = 0; j < halfDimension; j++)
                {
                    x21[i, j] = x[halfDimension + i, j];
                }

                for (var j = 0; j < xDimension - halfDimension; j++)
                {
                    x22[i, j] = x[halfDimension + i, halfDimension + j];
                }
            }

            return (x11, x12, x21, x22);
        }
    }
}
