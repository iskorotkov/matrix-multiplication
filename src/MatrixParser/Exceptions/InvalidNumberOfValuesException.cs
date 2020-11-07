using System;

namespace MatrixParser.Exceptions
{
    /// <summary>
    /// The exception that is thrown when there are less columns in input data than the number of columns of the matrix
    /// </summary>
    public class InvalidNumberOfValuesException : ArgumentException
    {
        /// <summary>
        /// Initializes new instance of InvalidNumberOfValuesException
        /// </summary>
        /// <param name="n">Number of columns in the matrix</param>
        public InvalidNumberOfValuesException(int n) : base(
            $"There should be exactly {n} doubles per line separated by a single space.")
        {
        }
    }
}
