using System.IO;

namespace SpaceShooter.Extensions
{
    /// <summary>
    /// StreamReader extension methods
    /// <seealso cref="StreamReader"/>
    /// </summary>
    public static class StreamReaderExtensions
    {
        #region Extension methods
        /// <summary>
        /// Reads and skips blanks line until it reaches the end of the file or a non-blank line
        /// </summary>
        /// <param name="reader">StreamReader to read from</param>
        /// <returns>The first non blank line encountered, or null if at the end of the file</returns>
        public static string SkipBlanks(this StreamReader reader)
        {
            //Get first line
            string line = reader.ReadLineTrimmed();

            //Skip all blank lines
            while (line != null && string.IsNullOrEmpty(line)) { line = reader.ReadLineTrimmed(); }

            //Return true if the 
            return line;
        }

        /// <summary>
        /// Reads a line from the reader and returns it trimmed
        /// </summary>
        /// <param name="reader">StreamReader to read from</param>
        /// <returns>The trimmed read line, or null if at the end of the file</returns>
        public static string ReadLineTrimmed(this StreamReader reader) => reader.ReadLine()?.Trim();
        #endregion
    }
}
