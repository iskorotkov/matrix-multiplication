using System;

namespace MatrixParser.Exceptions
{
    /// <summary>
    /// The exception that is thrown when number of rows or columns is invalid
    /// </summary>
    public class InvalidDimensionException : ArgumentException
    {
        public InvalidDimensionException() : base("Dimension should be a single positive integer.")
        {
        }
    }
}
