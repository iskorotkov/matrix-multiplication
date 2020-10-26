using System;
using MatrixTypes;
using Range = MatrixTypes.Range;

namespace StrassenAlgorithm
{
    public class Padder
    {
        public (MatrixView, MatrixView) Pad(MatrixView a, MatrixView b)
        {
            var dimension = Math.Max(
                Math.Max(a.Rows.Count, a.Columns.Count),
                Math.Max(b.Rows.Count, b.Columns.Count)
            );

            if (dimension % 2 == 1)
            {
                dimension++;
            }

            var (rows, columns) = (new Range(0, dimension), new Range(0, dimension));
            return (a.Slice(rows, columns, true), b.Slice(rows, columns, true));
        }
    }
}
