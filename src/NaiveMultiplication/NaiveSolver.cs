using MatrixTypes;

namespace NaiveMultiplication
{
    public class NaiveSolver : ISolver
    {
        public double[,] Multiply(double[,] a, double[,] b) //7+4N+4N^2+5N^3
        {
            var rows = a.GetLength(0); //1 
            var columns = b.GetLength(1);//1 
            var length = a.GetLength(1);//1 

            var result = new double[rows, columns];//1
            for (var row = 0; row < rows; row++)//2N+2
            {
                for (var column = 0; column < columns; column++)//(2N+2)*N
                {
                    for (var i = 0; i < length; i++)//(2N+2)*N^2
                    {
                        result[row, column] += a[row, i] * b[i, column];//3*N^3
                    }
                }
            }

            return result;//1 
        }
    }
}
