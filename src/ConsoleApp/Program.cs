using System;
using MatrixParser;
using MatrixTypes;

namespace ConsoleApp
{
    internal static class Program
    {
        private static void Main()
        {
            var parser = new Parser();
            var (a, b, _) = parser.Parse(Console.In);

            Console.WriteLine("A =");
            Console.WriteLine(new MatrixView(a));
            Console.WriteLine("B =");
            Console.WriteLine(new MatrixView(b));
        }
    }
}
