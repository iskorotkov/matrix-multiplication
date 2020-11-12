using System;
using MatrixTypes;
using NaiveMultiplication;

namespace StrassenAlgorithm
{
    public class StrassenSolver : ISolver
    {
        private const int FallbackDimension = 64;
        private readonly NaiveSolver _naiveSolver = new NaiveSolver();
        // Умножение алгоритм Штрассена
        public double[,] Multiply(double[,] a, double[,] b)
        {
            var (rows, cols) = (a.GetLength(0), b.GetLength(1));//2 
            // Get highest dimension of both matrices
            var dimension = Math.Max(
                Math.Max(a.GetLength(0), a.GetLength(1)),
                Math.Max(b.GetLength(0), b.GetLength(1))
            ); // 7
            dimension = CalculateDesiredDimension(dimension); //3 ИЛИ 4
            
            // Use naive solver if matrices are too small      
            if (dimension <= FallbackDimension) //1
            {
                return _naiveSolver.Multiply(a, b); //1+7+4N+4*N^2+5*N^3
            }

            // Resize A if necessary
            if (a.GetLength(0) != dimension || a.GetLength(1) != dimension) //1 ИЛИ 3
            {
                a = Extend(a, dimension); // 1 + 2N^2+12N+15
            }
            // Resize B if necessary
            if (b.GetLength(0) != dimension || b.GetLength(1) != dimension) //1 ИЛИ 3
            {
                b = Extend(b, dimension); // 1 + 2N^2+12N+15
            }
            var result = MultiplyRecursive(a, b); // 1 + t(MultiplyRecursive)
            
            // Resize resulting matrix back to correct dimensions
            return Shrink(result, rows, cols, 0, 0); // 1 +  5*N^2+4N+4
        }

        //Рекурсивное Умножение
        private double[,] MultiplyRecursive(double[,] a, double[,] b)
        {
            var dimension = a.GetLength(0); //1
            // Use naive solver if matrices are too small
            if (dimension <= FallbackDimension) //1
            {
                return _naiveSolver.Multiply(a, b); //1+7+4N+4*N^2+5*N^3
            }

            dimension = CalculateDesiredDimension(dimension); //3 ИЛИ 4
            var (a11, a12, a21, a22) = SubdivideSquareMatrix(a, dimension); // 4 + 
            var (b11, b12, b21, b22) = SubdivideSquareMatrix(b, dimension); //4 +

            var m1 = MultiplyRecursive(a12.Subtract(a22), b21.Add(b22)); //(4*N^2+4N+6)*2 + T(N/2) + 1
            var m2 = MultiplyRecursive(a11.Add(a22), b11.Add(b22));//(4*N^2+4N+6)*2 + T(N/2) + 1
            var m3 = MultiplyRecursive(a11.Subtract(a21), b11.Add(b12));//(4*N^2+4N+6)*2 + T(N/2) + 1
            var m4 = MultiplyRecursive(a11.Add(a12), b22);//4*N^2+4N+6 + T(N/2) + 1
            var m5 = MultiplyRecursive(a11, b12.Subtract(b22));//4*N^2+4N+6 + T(N/2) + 1
            var m6 = MultiplyRecursive(a22, b21.Subtract(b11));//4*N^2+4N+6 + T(N/2) + 1
            var m7 = MultiplyRecursive(a21.Add(a22), b11);//4*N^2+4N+6+ T(N/2) + 1
            //10*(4*N^2+4N+6)+7+7*T(N/2) или 7*T(N/2)+7+10*(4*(N+1)^2+4N+10)

            // Result
            // c11 c12
            // c21 c22
            var c = new double[dimension, dimension]; //1
            var halfDimension = dimension / 2; //2
            // Fill resulting matrix
            for (var i = 0; i < halfDimension; i++) //n+2  или n+3
            {
                for (var j = 0; j < halfDimension; j++) //(n+2)*n/2 или (n+3)*(n+1)/2
                {
                    c[i, j] = m1[i, j] + m2[i, j] - m4[i, j] + m6[i, j]; 
                    c[i, halfDimension + j] = m4[i, j] + m5[i, j]; 
                    c[halfDimension + i, j] = m6[i, j] + m7[i, j];
                    c[halfDimension + i, halfDimension + j] = m2[i, j] - m3[i, j] + m5[i, j] - m7[i, j];
                    //(4+3+3+6)* ((N/2)^2 ИЛИ ((N+1)/2)^2)
                }
            }
            return c; //1
        }

        //Расширить
        private static double[,] Extend(double[,] x, int dimension) // 2N^2+12N+15
        {
            var result = new double[dimension, dimension];//1
            var (rows, columns) = (x.GetLength(0), x.GetLength(1)); //2 

            for (var i = 0; i < rows; i++) //2N+2
            {
                for (var j = 0; j < columns; j++)//(2N+2)*N
                {
                    result[i, j] = x[i, j]; //N^2
                }

                for (var j = columns; j < dimension; j++) //4*N
                {
                    result[i, j] = 0d;  //N
                }
            }

            for (var i = rows; i < dimension; i++) // 4
            {
                for (var j = 0; j < dimension; j++)// (1+N+2+1+N)
                {
                    result[i, j] = 0d; //(N+1)
                }
            }

            return result; //1
        }

        //Сократить
        private static double[,] Shrink(double[,] x, int rows, int columns, int rowsOffset, int columnsOffset)//5*N^2+4N+4
        {
            var result = new double[rows, columns];//1
            for (var i = 0; i < rows; i++) //2+2N
            {
                for (var j = 0; j < columns; j++)//(2N+2)*N
                {
                    result[i, j] = x[rowsOffset + i, columnsOffset + j];//3*N^2
                }
            }

            return result;//1
        }
        //Рассчитать Необходимый Размер
        private static int CalculateDesiredDimension(int dimension) => dimension % 2 == 0 ? dimension : dimension + 1; //3 ИЛИ 4
        
        
        //Разделить Квадратную Матрицу
        private static (double[,], double[,], double[,], double[,]) SubdivideSquareMatrix(double[,] x, int dimension) //13+7,5N+4.5*N^2  или 4.5N^2+6N+12.5
        {
            // Dimension of each submatrix
            var halfDimension = dimension / 2; //2
            var x11 = new double[halfDimension, halfDimension]; //1
            var x12 = new double[halfDimension, halfDimension]; //1
            var x21 = new double[halfDimension, halfDimension]; //1
            var x22 = new double[halfDimension, halfDimension]; //1
            
            // Real dimension of matrix X
            var xDimension = x.GetLength(0);//1
            for (var i = 0; i < halfDimension; i++)//n+2  или n+3
            { 
                // Fill leftmost upper submatrix
                for (var j = 0; j < halfDimension; j++)//((n+2 )*n/2 или (n+3)*(n+1)/2
                {
                    x11[i, j] = x[i, j];//(n/2)^2 или ((n+1)/2)^2
                }
                // Fill rightmost upper submatrix
                for (var j = 0; j < xDimension - halfDimension; j++)//(1.5n+3) *n/2 или (1,5n+1.5)*(n+1)/2(худ)
                {
                    x12[i, j] = x[i, halfDimension + j];//2*(n/2)^2  или (n-1)*(n+1)/2
                }
            }

            for (var i = 0; i < xDimension - halfDimension; i++)//(1.5n+3)или (1,5n+1.5)(худ)
            {
                // Fill leftmost bottom submatrix
                for (var j = 0; j < halfDimension; j++)//(n+2)*n/2 или (n+3)*(n-1)/2
                {
                    x21[i, j] = x[halfDimension + i, j];//2*(n/2)^2   или (n-1)*(n+1)/2
                }
                // Fill rightmost bottom submatrix
                for (var j = 0; j < xDimension - halfDimension; j++)//(1.5n+3)*n/2 или (1,5n+1.5)*(n-1)/2
                {
                    x22[i, j] = x[halfDimension + i, halfDimension + j];//3*(n/2)^2 или 3*((n-1)/2)^2
                }
            }

            return (x11, x12, x21, x22);//1
        }
    }
}
