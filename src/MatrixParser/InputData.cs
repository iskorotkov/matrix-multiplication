namespace MatrixParser
{
    public readonly struct InputData
    {
        public InputData(double[,] a, double[,] b, int n)
        {
            A = a;
            B = b;
            N = n;
        }

        public double[,] A { get; }
        public double[,] B { get; }
        public int N { get; }

        public void Deconstruct(out double[,] a, out double[,] b, out int n) =>
            (a, b, n) = (A, B, N);
    }
}
