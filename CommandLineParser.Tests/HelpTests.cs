using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CommandLineParser.Tests.Helpers;
using CommandLineParser.Tests.Models;
using CommandLineParser.Tests.Attributes;

namespace CommandLineParser.Tests
{
    [TestClass]
    public class HelpTests
    {
        [TestMethod]
        [TestDescription("program --help")]
        public void Help_Long_Option_Works()
        {
            string[] args = CommandManager.CommandLineToArgs("program --help");
            CommandParser parser = new CommandParser(args);
            string helpText = parser.GetHelpInfo<TestModel>();
            Console.WriteLine(helpText);
            StringAssert.Contains(helpText, "== This is a Test Model ==");
        }

        [TestMethod]
        [TestDescription("program -h")]
        public void Help_Short_Option_Works()
        {
            string[] args = CommandManager.CommandLineToArgs("program -h");
            CommandParser parser = new CommandParser(args);
            string helpText = parser.GetHelpInfo<TestModel>();
            Console.WriteLine(helpText);
            StringAssert.Contains(helpText, "== This is a Test Model ==");
        }

        [TestMethod]
        [TestDescription("program --username --help", "Usage: [-u --username (This is the Username property)]")]
        public void Help_Property_Long_Option_Works()
        {
            string[] args = CommandManager.CommandLineToArgs("program --username --help");
            CommandParser parser = new CommandParser(args);
            string helpText = parser.GetHelpInfo<TestModel>();
            Console.WriteLine(helpText);
            Assert.AreEqual(helpText, "Usage: [-u --username (This is the Username property)]");
        }

        [TestMethod]
        [TestDescription("program -u -h", "Usage: [-u --username (This is the Username property)]")]
        public void Help_Property_Short_Option_Works()
        {
            string[] args = CommandManager.CommandLineToArgs("program -u -h");
            CommandParser parser = new CommandParser(args);
            string helpText = parser.GetHelpInfo<TestModel>();
            Console.WriteLine(helpText);
            Assert.AreEqual(helpText, "Usage: [-u --username (This is the Username property)]");
        }

        [TestMethod]
        [TestDescription("program -h")]
        public void Help_Print_Help_Text()
        {
            string[] args = CommandManager.CommandLineToArgs("program -h");
            CommandParser parser = new CommandParser(args);
            Console.WriteLine(parser.GetHelpInfo<TestNoHelpTextModel>());
        }

        [TestMethod]
        [TestDescription("program -n -h")]
        public void Help_Check_When_No_Help_Text_Exists()
        {
            string[] args = CommandManager.CommandLineToArgs("program -n -h");
            CommandParser parser = new CommandParser(args);
            string helpText = parser.GetHelpInfo<TestNoHelpTextModel>();
            Console.WriteLine(helpText);
            StringAssert.Equals(helpText, "No Help Attribute found on Property [Name]");
        }
    }
}
