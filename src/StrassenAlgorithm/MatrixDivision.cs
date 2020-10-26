using MatrixTypes;

namespace StrassenAlgorithm
{
    public struct MatrixDivision
    {
        public MatrixView X11 { get; set; }
        public MatrixView X12 { get; set; }
        public MatrixView X21 { get; set; }
        public MatrixView X22 { get; set; }

        public void Deconstruct(out MatrixView x11, out MatrixView x12, out MatrixView x21, out MatrixView x22) =>
            (x11, x12, x21, x22) = (X11, X12, X21, X22);
    }
}
