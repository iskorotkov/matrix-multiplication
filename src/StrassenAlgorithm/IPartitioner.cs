using MatrixTypes;

namespace StrassenAlgorithm
{
    public interface IPartitioner
    {
        MatrixDivision Subdivide(MatrixView x);
    }
}
