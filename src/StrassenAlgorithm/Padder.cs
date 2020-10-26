using System;
using MatrixTypes;
using Range = MatrixTypes.Range;

namespace StrassenAlgorithm
{
    public class Padder
    {
        public (MatrixView, MatrixView) PadToEven(MatrixView a, MatrixView b)
        {
            var dimension = Math.Max(
                Math.Max(a.Rows.Count, a.Columns.Count),
                Math.Max(b.Rows.Count, b.Columns.Count)
            );

            if (dimension % 2 == 1)
            {
                dimension++;
            }

            var range = new Range(0, dimension);
            return (a.Slice(range, range, true), b.Slice(range, range, true));
        }

        public (MatrixView, MatrixView) PadToEven(MatrixView a, MatrixView b, int target)
        {
            if (target % 2 == 1)
            {
                target++;
            }

            var range = new Range(0, target);
            return (a.Slice(range, range, true), b.Slice(range, range, true));
        }
    }
}
