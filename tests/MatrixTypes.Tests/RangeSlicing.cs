using MatrixTypes.Exceptions;
using Shouldly;
using Xunit;

namespace MatrixTypes.Tests
{
    public class RangeSlicing
    {
        private readonly Range _r = new Range(0, 10);

        [Fact]
        public void FromStart()
        {
            _r.Slice(new Range(8, 10)).ShouldBe(new Range(8, 10));
        }

        [Fact]
        public void FromEnd()
        {
            _r.Slice(new Range(0, 9)).ShouldBe(new Range(0, 9));
        }

        [Fact]
        public void FromBothSides()
        {
            _r.Slice(new Range(3, 8)).ShouldBe(new Range(3, 8));
        }

        [Fact]
        public void EmptyRange()
        {
            _r.Slice(new Range(5, 2)).ShouldBe(new Range(5, 2));
            _r.Slice(new Range(0, 0)).ShouldBe(new Range(0, 0));
            _r.Slice(new Range(9, 9)).ShouldBe(new Range(9, 9));
        }

        [Theory, InlineData(0, 20), InlineData(-1, 5), InlineData(-2, 12)]
        public void NonExtendingSlice(int start, int end)
        {
            Assert.Throws<RangeAccessException>(() => _r.Slice(new Range(start, end)));
        }

        [Fact]
        public void ExtendingSlice()
        {
            var slice = _r.Slice(new Range(-10, 20), true);
            slice.Start.ShouldBe(-10);
            slice.End.ShouldBe(20);
            slice.Count.ShouldBe(30);

            slice.Contains(-11).ShouldBeFalse();
            slice.Contains(-10).ShouldBeTrue();
            slice.Contains(0).ShouldBeTrue();
            slice.Contains(19).ShouldBeTrue();
            slice.Contains(20).ShouldBeFalse();
        }
    }
}
