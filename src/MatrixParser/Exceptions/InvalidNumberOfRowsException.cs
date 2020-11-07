using System;

namespace MatrixParser.Exceptions
{
    /// <summary>
    /// The exception that is thrown when there are less rows in input data than the number of rows of the matrix
    /// </summary>
    public class InvalidNumberOfRowsException : ArgumentException
    {
        /// <summary>
        /// Initializes new instance of InvalidNumberOfRowsException
        /// </summary>
        /// <param name="n">Number of rows in the matrix</param>
        public InvalidNumberOfRowsException(int n) : base(
            $"There should be exactly {n} lines with values.")
        {
        }
    }
}
