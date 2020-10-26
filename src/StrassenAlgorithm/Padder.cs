using System;
using MatrixTypes;
using Range = MatrixTypes.Range;

namespace StrassenAlgorithm
{
    public class Padder
    {
        public MatrixView Pad(MatrixView view)
        {
            var dimension = Math.Max(view.Rows.Count, view.Columns.Count);
            if (dimension % 2 == 1)
            {
                dimension++;
            }

            return view.Slice(
                new Range(0, dimension),
                new Range(0, dimension),
                true);
        }
    }
}
