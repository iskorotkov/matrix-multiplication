namespace MatrixTypes
{
    public readonly struct Range
    {
        public int Start { get; }
        public int End { get; }

        public Range(int start, int end)
        {
            Start = start;
            End = end;
        }

        public int Count => End - Start;

        public Range Slice(Range range)
        {
            if (!ContainsInclusive(Start + range.Start) || !ContainsInclusive(Start + range.End))
            {
                throw new RangeAccessException(nameof(range));
            }

            return new Range(Start + range.Start, Start + range.End);
        }

        public bool Contains(int value) => value >= Start && value < End;

        private bool ContainsInclusive(int value) => value >= Start && value <= End;
    }
}
