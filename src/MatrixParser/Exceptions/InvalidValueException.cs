using System;

namespace MatrixParser.Exceptions
{
    /// <summary>
    /// The exception that is thrown when parsed failed to parse matrix value
    /// </summary>
    public class InvalidValueException : ArgumentException
    {
        /// <summary>
        /// Initializes new instance of InvalidValueException for specified input value
        /// </summary>
        /// <param name="value">Matrix value that couldn't be parsed</param>
        public InvalidValueException(string value) : base(
            $"Can't parse value '{value}'. Matrix values should be doubles.")
        {
        }
    }
}
