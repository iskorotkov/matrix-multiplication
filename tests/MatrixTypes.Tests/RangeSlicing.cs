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

        [Fact]
        public void NonExtendingSlice()
        {
            Assert.Throws<RangeAccessException>(() => _r.Slice(new Range(0, 20)));
            Assert.Throws<RangeAccessException>(() => _r.Slice(new Range(-1, 5)));
            Assert.Throws<RangeAccessException>(() => _r.Slice(new Range(-2, 12)));
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
