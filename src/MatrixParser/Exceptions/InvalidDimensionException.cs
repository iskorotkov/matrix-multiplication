using System;

namespace MatrixParser.Exceptions
{
    public class InvalidDimensionException : ArgumentException
    {
        public InvalidDimensionException() : base("Dimension should be a single positive integer.")
        {
        }
    }
}