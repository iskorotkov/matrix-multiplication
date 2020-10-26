using System;

namespace MatrixTypes
{
    public class MatrixVirtualWriteException : InvalidOperationException
    {
        public MatrixVirtualWriteException() : base( "Can't write to a virtual cell (the cell is only added as a padding to real underlying matrix and doesn't map to any memory location).")
        {
        }
    }
}
