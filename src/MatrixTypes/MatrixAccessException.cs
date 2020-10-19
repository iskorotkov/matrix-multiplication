using System;

namespace MatrixTypes
{
    public class MatrixAccessException : ArgumentOutOfRangeException
    {
        public MatrixAccessException(string paramName) : base(paramName, "Value should be in a valid range of matrix view.")
        {
        }
    }
}