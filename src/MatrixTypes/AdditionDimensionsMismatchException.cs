using System;

namespace MatrixTypes
{
    public class AdditionDimensionsMismatchException : ArgumentException
    {
        public AdditionDimensionsMismatchException() : base("Matrices should have dimensions NxM and NxM.")
        {
        }
    }
}
