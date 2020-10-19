using System;

namespace MatrixTypes
{
    public class MatrixAccessException : ArgumentException
    {
        public MatrixAccessException(string paramName) : base("Value should be in a valid range of matrix view.",
            paramName)
        {
        }
    }
}