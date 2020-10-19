using System;

namespace MatrixTypes
{
    public class MultiplicationDimensionsMismatchException : ArgumentException
    {
        public MultiplicationDimensionsMismatchException() : base("Matrices should have dimensions NxM and MxK.")
        {
        }
    }
}