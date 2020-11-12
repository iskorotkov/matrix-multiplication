using System;

namespace MatrixParser.Exceptions
{
    public class InvalidNumberOfRowsException : ArgumentException
    {
        public InvalidNumberOfRowsException(int n) : base(
            $"There should be exactly {n} lines with values.")
        {
        }
    }
}