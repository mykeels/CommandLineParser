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
    public class TerminatorTests
    { 
        [TestMethod]
        [TestDescription("program -- Hello World")]
        public void Terminators_Are_Bound_To_Extras()
        {
            string[] args = CommandManager.CommandLineToArgs("program -- Hello World");
            CommandParser parser = new CommandParser(args);
            var model = parser.Parse<TestModel>();
            CollectionAssert.Contains(model.extras, "Hello");
            CollectionAssert.Contains(model.extras, "World");
        }
    }
}
