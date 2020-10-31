﻿using System;
using MatrixTypes;

namespace StrassenAlgorithm
{
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

            if (a.GetLength(0) != desiredDimension || a.GetLength(1) != desiredDimension)
            {
                a = Extend(a, desiredDimension, desiredDimension);
            }

            if (b.GetLength(0) != desiredDimension || b.GetLength(1) != desiredDimension)
            {
                b = Extend(b, desiredDimension, desiredDimension);
            }

            var result = MultiplyRecursive(a, b);
            return Shrink(result, rows, cols, 0, 0);
        }

        private static double[,] MultiplyRecursive(double[,] a, double[,] b)
        {
            // a and b are always square matrices of the same dimension => check if contain single cell
            if (a.GetLength(0) == 1)
            {
                return new[,] {{a[0, 0] * b[0, 0]}};
            }

            // a and b are always square matrices of the same dimension => can skip taking max dimension and use number of rows
            var actualDimension = a.GetLength(0);
            var desiredDimension = CalculateDesiredDimension(actualDimension);
            if (actualDimension != desiredDimension)
            {
                (a, b) = (Extend(a, desiredDimension, desiredDimension), Extend(b, desiredDimension, desiredDimension));
            }

            var (a11, a12, a21, a22) = SubdivideSquareMatrix(a);
            var (b11, b12, b21, b22) = SubdivideSquareMatrix(b);

            var m1 = MultiplyRecursive(a12.Subtract(a22), b21.Add(b22));
            var m2 = MultiplyRecursive(a11.Add(a22), b11.Add(b22));
            var m3 = MultiplyRecursive(a11.Subtract(a21), b11.Add(b12));
            var m4 = MultiplyRecursive(a11.Add(a12), b22);
            var m5 = MultiplyRecursive(a11, b12.Subtract(b22));
            var m6 = MultiplyRecursive(a22, b21.Subtract(b11));
            var m7 = MultiplyRecursive(a21.Add(a22), b11);

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

        private static double[,] Extend(double[,] x, int rows, int columns)
        {
            var result = new double[rows, columns];
            for (var i = 0; i < x.GetLength(0); i++)
            {
                for (var j = 0; j < x.GetLength(1); j++)
                {
                    result[i, j] = x[i, j];
                }

                for (var j = x.GetLength(1); j < columns; j++)
                {
                    result[i, j] = 0d;
                }
            }

            for (var i = x.GetLength(0); i < rows; i++)
            {
                for (var j = 0; j < columns; j++)
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

        private static (double[,] X11, double[,] X12, double[,] X21, double[,] X22) SubdivideSquareMatrix(double[,] x)
        {
            var halfSize = x.GetLength(0) / 2;
            return (
                Shrink(x, halfSize, halfSize, 0, 0),
                Shrink(x, halfSize, halfSize, 0, halfSize),
                Shrink(x, halfSize, halfSize, halfSize, 0),
                Shrink(x, halfSize, halfSize, halfSize, halfSize)
            );
        }
    }
}
