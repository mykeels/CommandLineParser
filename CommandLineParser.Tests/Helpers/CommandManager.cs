using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CommandLineParser.Tests.Helpers
{
    public class CommandManager
    {
        private static string trimMatchingQuotes(string input, char quote) {
            if ((input.Length >= 2) && 
                (input[0] == quote) && (input[input.Length - 1] == quote))
                return input.Substring(1, input.Length - 2);

            return input;
        }

        private static IEnumerable<string> split(string input, Func<char, bool> fn) {
            int nextPiece = 0;

            for (int c = 0; c < input.Length; c++)
            {
                if (fn(input[c]))
                {
                    yield return input.Substring(nextPiece, c - nextPiece);
                    nextPiece = c + 1;
                }
            }

            yield return input.Substring(nextPiece);
        }

        public static string[] CommandLineToArgs(string commandLine)
        {
            bool inQuotes = false;

            return split(commandLine, (c) =>
                            {
                                if (c == '\"')
                                    inQuotes = !inQuotes;

                                return !inQuotes && c == ' ';
                            })
                            .Select(arg => trimMatchingQuotes(arg.Trim(), '\"'))
                            .Where(arg => !string.IsNullOrEmpty(arg)).ToArray();
        }
    }
}
