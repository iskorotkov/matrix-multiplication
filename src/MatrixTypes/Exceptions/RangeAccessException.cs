using System;

namespace MatrixTypes.Exceptions
{
    public class RangeAccessException : ArgumentOutOfRangeException
    {
        public RangeAccessException(string paramName) : base(paramName, "Value should be in a valid range.")
        {
        }
    }
}
