using System.IO;
using MatrixParser.Exceptions;

namespace MatrixParser
{
    /// <summary>
    /// Parser for reading matrices
    /// </summary>
    public class Parser
    {
        /// <summary>
        /// Parse two matrices from reader
        /// </summary>
        /// <param name="reader">Reader to use</param>
        /// <returns>Parse result</returns>
        public InputData Parse(TextReader reader)
        {
            var line = reader.ReadLine();
            if (!int.TryParse(line, out var n) || n <= 0)
            {
                throw new InvalidDimensionException();
            }

            var a = ReadMatrix(reader, n);
            var b = ReadMatrix(reader, n);
            return new InputData(a, b, n);
        }

        /// <summary>
        /// Read single matrix of given dimension
        /// </summary>
        /// <param name="reader">Reader to use</param>
        /// <param name="n">Matrix dimension</param>
        /// <returns>Parsed matrix</returns>
        private static double[,] ReadMatrix(TextReader reader, int n)
        {
            var a = new double[n, n];
            for (var i = 0; i < n; i++)
            {
                var line = reader.ReadLine();
                var row = line?.Split(' ') ?? throw new InvalidNumberOfRowsException(n);
                if (row.Length < n)
                {
                    throw new InvalidNumberOfValuesException(n);
                }

                for (var j = 0; j < row.Length; j++)
                {
                    var str = row[j];
                    if (!double.TryParse(str, out var value))
                    {
                        throw new InvalidValueException(str);
                    }

                    a[i, j] = value;
                }
            }

            return a;
        }
    }
}
