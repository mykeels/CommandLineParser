using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CommandLineParser.Console
{
    public partial class Program
    {
        public class Console
        {
            internal static string Write(string text, ConsoleColor color = ConsoleColor.White)
            {
                if (color != ConsoleColor.White) System.Console.ForegroundColor = color;
                System.Console.Write(text);
                if (color != ConsoleColor.White) System.Console.ResetColor();
                return text;
            }

            internal static string WriteLine(string text, ConsoleColor color = ConsoleColor.White)
            {
                if (color != ConsoleColor.White) System.Console.ForegroundColor = color;
                System.Console.WriteLine(text);
                if (color != ConsoleColor.White) System.Console.ResetColor();
                return text;
            }

            internal static string WriteLine<T>(T data, Formatting formatting = Formatting.Indented, ConsoleColor color = ConsoleColor.White)
            {
                return WriteLine(JsonConvert.SerializeObject(data, formatting), color);
            }

            internal static int Read()
            {
                return System.Console.Read();
            }

            internal static string ReadLine()
            {
                return System.Console.ReadLine();
            }
        }
    }
}
