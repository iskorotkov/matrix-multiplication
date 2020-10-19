using System;

namespace MatrixTypes
{
    public class DimensionsMismatchException : ArgumentException
    {
        public DimensionsMismatchException() : base("Matrices should have dimensions (NxM)*(MxK).")
        {
        }
    }
}