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
    public class NonOptionTests
    {
        [TestMethod]
        [TestDescription("program hello")]
        public void NonOptions_Are_Bound_To_Options()
        {
            string[] args = CommandManager.CommandLineToArgs("program hello");
            CommandParser parser = new CommandParser(args);
            var model = parser.Parse<TestModel>();
            CollectionAssert.Contains(model.Options, "hello");
        }
    }
}
