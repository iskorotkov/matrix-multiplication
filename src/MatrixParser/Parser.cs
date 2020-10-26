using System.IO;
using MatrixParser.Exceptions;

namespace MatrixParser
{
    public class Parser
    {
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
