using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLineParser;
using CommandLineParser.Console.Models;
using Newtonsoft.Json;

namespace CommandLineParser.Console
{
    public partial class Program
    {
        public static void Main(string[] args)
        {
            if (args != null && args.Length > 0)
            {
                CommandParser parser = new CommandParser(args);
                Console.WriteLine(parser.GetHelpInfo<SetupModel>(), ConsoleColor.White);
            }
            else
            {
                Program p = new Program();
                p.testNameHelpInfo();
            }
            
            Console.Read();
        }

        private void testArgumentParser()
        {
            Console.WriteLine(new CommandParser().GetDictionary(new string[] { "--name", "Ikechi Michael", "--empty", "true" }));
        }

        private void testCliParser()
        {
            CommandParser parser = new CommandParser(new string[] { "-n", "Ikechi Michael", "-f", "true", "--age", "13", "--address", JsonConvert.SerializeObject(new AddressModel()) });
            Console.WriteLine(parser.Parse<SetupModel>());
        }

        private void testHelpInfo()
        {

            CommandParser parser = new CommandParser(new string[] { "--help" });
            //var setup = parser.Parse<SetupModel>();
            Console.WriteLine(parser.GetHelpInfo<SetupModel>(), ConsoleColor.White);
        }

        private void testNameHelpInfo()
        {

            CommandParser parser = new CommandParser(new string[] { "-n", "--help" });
            Console.WriteLine(parser.GetHelpInfo<SetupModel>(), ConsoleColor.White);
        }
    }
}
