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
    }
}
