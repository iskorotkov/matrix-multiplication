using Shouldly;
using Xunit;

namespace MatrixTypes.Tests
{
    public class MatrixViewSlicing
    {
        private readonly MatrixView _view = new MatrixView(new double[,]
        {
            {1, 2, 3, 4},
            {5, 6, 7, 8},
            {9, 10, 11, 12},
            {13, 14, 15, 16}
        });

        [Fact]
        public void FullSlice()
        {
            _view.Slice(new Range(0, 4), new Range(0, 4)).ShouldBe(_view);
        }

        [Theory, InlineData(0, 3, 0, 3), InlineData(2, 4, 2, 4), InlineData(3, 4, 0, 3), InlineData(2, 4, 1, 4)]
        public void ShrinkingSlice(int rowsStart, int rowsEnd, int columnsStart, int columnsEnd)
        {
            var rows = new Range(rowsStart, rowsEnd);
            var columns = new Range(columnsStart, columnsEnd);

            var slice = _view.Slice(rows, columns);
            EnsureEqual(slice, rows, columns);
        }

        [Fact]
        public void SeveralSlicesInRow()
        {
            var s1 = _view.Slice(new Range(1, 4), new Range(0, 3));
            var s2 = s1.Slice(new Range(0, 2), new Range(1, 3));
            var s3 = s2.Slice(new Range(1, 2), new Range(0, 1));
            s3[0, 0].ShouldBe(_view[2, 1]);
        }

        private void EnsureEqual(MatrixView view, Range rows, Range columns)
        {
            for (var i = 0; i < rows.Count; i++)
            {
                for (var j = 0; j < columns.Count; j++)
                {
                    view[i, j].ShouldBe(_view[rows.Start + i, columns.Start + j]);
                }
            }
        }
    }
}
