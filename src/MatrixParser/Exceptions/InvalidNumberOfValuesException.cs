using System;

namespace MatrixParser.Exceptions
{
    public class InvalidNumberOfValuesException : ArgumentException
    {
        public InvalidNumberOfValuesException(int n) : base(
            $"There should be exactly {n} doubles per line separated by a single space.")
        {
        }
    }
}