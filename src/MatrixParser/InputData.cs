namespace MatrixParser
{
    /// <summary>
    /// Container for storing input data
    /// </summary>
    public readonly struct InputData
    {
        /// <summary>
        /// Initialize new instance of InputData
        /// </summary>
        /// <param name="a">First matrix</param>
        /// <param name="b">Second matrix</param>
        /// <param name="n">Matrix dimensions</param>
        public InputData(double[,] a, double[,] b, int n)
        {
            A = a;
            B = b;
            N = n;
        }

        /// <summary>
        /// First matrix
        /// </summary>
        public double[,] A { get; }

        /// <summary>
        /// Second matrix
        /// </summary>
        public double[,] B { get; }

        /// <summary>
        /// Matrix dimensions
        /// </summary>
        public int N { get; }

        /// <summary>
        /// Deconstruct InputData into several variables
        /// </summary>
        /// <param name="a">First matrix</param>
        /// <param name="b">Second matrix</param>
        /// <param name="n">Matrix dimensions</param>
        public void Deconstruct(out double[,] a, out double[,] b, out int n) =>
            (a, b, n) = (A, B, N);
    }
}
