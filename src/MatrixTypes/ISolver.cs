namespace MatrixTypes
{
    public interface ISolver
    {
        double[,] Multiply(double[,] a, double[,] b);
        MatrixView Multiply(MatrixView a, MatrixView b);
    }
}
