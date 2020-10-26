using System;

namespace MatrixTypes.Exceptions
{
    public class AdditionDimensionsMismatchException : ArgumentException
    {
        public AdditionDimensionsMismatchException() : base("Matrices should have dimensions NxM and NxM.")
        {
        }
    }
}
