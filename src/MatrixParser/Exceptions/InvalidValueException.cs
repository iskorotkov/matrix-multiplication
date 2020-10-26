using System;

namespace MatrixParser.Exceptions
{
    public class InvalidValueException : ArgumentException
    {
        public InvalidValueException(string value) : base(
            $"Can't parse value '{value}'. Matrix values should be doubles.")
        {
        }
    }
}