using System;

namespace MatrixTypes.Exceptions
{
    public class MultiplicationDimensionsMismatchException : ArgumentException
    {
        public MultiplicationDimensionsMismatchException() : base("Matrices should have dimensions NxM and MxK.")
        {
        }
    }
}