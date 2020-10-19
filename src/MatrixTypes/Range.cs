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

        public bool Contains(int value) => value >= Start && value < End;
    }
}
