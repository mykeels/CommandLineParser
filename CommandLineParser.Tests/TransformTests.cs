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
    public class TransformTests
    {
        [TestMethod]
        [TestDescription("program --address \"{'HomeAddress':'London','Latitude':1.5678,'Longitude':0.2345}\"")]
        public void Transform_Works()
        {
            string[] args = CommandManager.CommandLineToArgs("program --address \"{'HomeAddress':'London','Latitude':1.5678,'Longitude':0.2345}\"");
            CommandParser parser = new CommandParser(args);
            var model = parser.Parse<TestModel>();
            Assert.IsTrue(model.Address.HomeAddress == "London");
        }
    }
}
