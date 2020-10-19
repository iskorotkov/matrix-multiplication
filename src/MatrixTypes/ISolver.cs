namespace MatrixTypes
{
    public interface ISolver
    {
        double[,] Multiply(double[,] a, double[,] b);
        double[,] Multiply(MatrixView a, MatrixView b);
    }
}
