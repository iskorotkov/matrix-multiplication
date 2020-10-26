using MatrixTypes;
using Range = MatrixTypes.Range;

namespace StrassenAlgorithm
{
    public class Partitioner
    {
        public MatrixDivision Subdivide(MatrixView x)
        {
            var (rowStart, rowEnd) = SubdivideRange(x.Rows);
            var (columnStart, columnEnd) = SubdivideRange(x.Columns);
            return new MatrixDivision
            {
                X11 = x.Slice(rowStart, columnStart),
                X12 = x.Slice(rowStart, columnEnd),
                X21 = x.Slice(rowEnd, columnStart),
                X22 = x.Slice(rowEnd, columnEnd)
            };
        }

        private static (Range Start, Range End) SubdivideRange(Range r)
        {
            var start = new Range(0, r.Count / 2);
            var end = new Range(r.Count / 2, r.Count);

            return (start, end);
        }
    }
}
