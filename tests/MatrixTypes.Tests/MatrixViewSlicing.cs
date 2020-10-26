using MatrixTypes.Exceptions;
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
            EnsureSliceIsCorrect(slice, rows, columns);
        }

        [Fact]
        public void SeveralSlicesInRow()
        {
            var s1 = _view.Slice(new Range(1, 4), new Range(0, 3));
            var s2 = s1.Slice(new Range(0, 2), new Range(1, 3));
            var s3 = s2.Slice(new Range(1, 2), new Range(0, 1));
            s3[0, 0].ShouldBe(_view[2, 1]);
        }

        [Theory, InlineData(0, 5, 0, 5), InlineData(-4, 4, -4, 4), InlineData(-3, 5, -3, 5)]
        public void NonExtendingSlice(int rowsStart, int rowsEnd, int columnsStart, int columnsEnd)
        {
            Assert.Throws<RangeAccessException>(() =>
                _view.Slice(new Range(rowsStart, rowsEnd), new Range(columnsStart, columnsEnd)));
        }

        [Fact]
        public void ExtendingSlice()
        {
            var rows = new Range(-5, 5);
            var cols = new Range(-5, 5);
            var m = _view.Slice(rows, cols, true);
            
            EnsureRangeIsCorrect(m.Rows, rows);
            EnsureRangeIsCorrect(m.Columns, cols);

            m.Slice(new Range(5, 9), new Range(5, 9)).ShouldBe(_view);
            m[0, 0].ShouldBe(0);
            m[3, 4].ShouldBe(0);
            m[8, 9].ShouldBe(0);

            Assert.Throws<MatrixAccessException>(() => m[10, 1]);
            Assert.Throws<MatrixVirtualWriteException>(() => m[4, 5] = 2);
        }

        private void EnsureSliceIsCorrect(MatrixView view, Range originalRows, Range originalColumns)
        {
            for (var i = 0; i < originalRows.Count; i++)
            {
                for (var j = 0; j < originalColumns.Count; j++)
                {
                    view[i, j].ShouldBe(_view[originalRows.Start + i, originalColumns.Start + j]);
                }
            }
        }

        private static void EnsureRangeIsCorrect(Range actual, Range expected)
        {
            actual.Start.ShouldBe(expected.Start);
            actual.End.ShouldBe(expected.End);
            actual.Count.ShouldBe(expected.Count);
        }
    }
}
