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
    public class ValuesOptionTests
    {
        [TestMethod]
        [TestDescription("program --role=SuperAdmin --role=Admin")]
        public void Values_Long_Option_Equals_Separated_Values_Can_Have_Multiple_Values()
        {
            string[] args = CommandManager.CommandLineToArgs("program --role=SuperAdmin --role=Admin");
            CommandParser parser = new CommandParser(args);
            var model = parser.Parse<TestModel>();
            Assert.IsTrue(model.isAdmin);
            Assert.IsTrue(model.isSuperAdmin);
        }

        [TestMethod]
        [TestDescription("program -r=SuperAdmin -r=Admin")]
        public void Values_Short_Option_Equals_Separated_Values_Can_Have_Multiple_Values()
        {
            string[] args = CommandManager.CommandLineToArgs("program -r=SuperAdmin -r=Admin");
            CommandParser parser = new CommandParser(args);
            var model = parser.Parse<TestModel>();
            Assert.IsTrue(model.isAdmin);
            Assert.IsTrue(model.isSuperAdmin);
        }

        [TestMethod]
        [TestDescription("program --role SuperAdmin --role Admin")]
        public void Values_Long_Option_Space_Separated_Values_Can_Have_Multiple_Values()
        {
            string[] args = CommandManager.CommandLineToArgs("program --role SuperAdmin --role Admin");
            CommandParser parser = new CommandParser(args);
            var model = parser.Parse<TestModel>();
            Assert.IsTrue(model.isAdmin);
            Assert.IsTrue(model.isSuperAdmin);
        }

        [TestMethod]
        [TestDescription("program -r SuperAdmin -r Admin")]
        public void Values_Short_Option_Space_Separated_Values_Can_Have_Multiple_Values()
        {
            string[] args = CommandManager.CommandLineToArgs("program -r SuperAdmin -r Admin");
            CommandParser parser = new CommandParser(args);
            var model = parser.Parse<TestModel>();
            Assert.IsTrue(model.isAdmin);
            Assert.IsTrue(model.isSuperAdmin);
        }

        [TestMethod]
        [TestDescription("program --role SuperAdmin Admin")]
        public void Values_Long_Option_Space_Separated_Values_Can_Have_Multiple_Values_2()
        {
            string[] args = CommandManager.CommandLineToArgs("program --role SuperAdmin Admin");
            CommandParser parser = new CommandParser(args);
            var model = parser.Parse<TestModel>();
            Assert.IsTrue(model.isAdmin);
            Assert.IsTrue(model.isSuperAdmin);
        }

        [TestMethod]
        [TestDescription("program -r SuperAdmin Admin")]
        public void Values_Short_Option_Space_Separated_Values_Can_Have_Multiple_Values_2()
        {
            string[] args = CommandManager.CommandLineToArgs("program -r SuperAdmin Admin");
            CommandParser parser = new CommandParser(args);
            var model = parser.Parse<TestModel>();
            Assert.IsTrue(model.isAdmin);
            Assert.IsTrue(model.isSuperAdmin);
        }
    }
}
