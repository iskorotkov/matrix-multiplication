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

        public Range Slice(Range range) => new Range(Start + range.Start, Start + range.End);

        public bool Contains(int value) => value >= Start && value < End;

        public override string ToString() => $"{Start}..{End}";
    }
}
