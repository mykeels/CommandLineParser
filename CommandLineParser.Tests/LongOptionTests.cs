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
    public class LongOptionTests
    {
        [TestMethod]
        [TestDescription("--admin", true)]
        [TestDescription("-admin", false)]
        public void Long_Option_Must_Start_With_Double_Hyphens()
        {
            Dictionary<string, bool> testsAndExpectedStatus = new Dictionary<string, bool>()
            {
                { "--admin", true },
                { "-admin", false }
            };
            testsAndExpectedStatus.ToList().ForEach(test =>
            {
                bool isKey = CommandLineParser.Helpers.KeyDetection.GetLongKeyDetector().IsKey(test.Key);
                Console.WriteLine($"{test.Key} detected:{isKey} actual:{test.Value}");
                Assert.AreEqual(isKey, test.Value);
            });
        }

        [TestMethod]
        [TestDescription("--admin", true)]
        [TestDescription("--Admin", false)]
        public void Long_Option_Must_Be_Lowercase()
        {

            Dictionary<string, bool> testsAndExpectedStatus = new Dictionary<string, bool>()
            {
                { "--admin", true },
                { "--Admin", false }
            };
            testsAndExpectedStatus.ToList().ForEach(test =>
            {
                bool isKey = CommandLineParser.Helpers.KeyDetection.GetLongKeyDetector().IsKey(test.Key);
                Console.WriteLine($"{test.Key} detected:{isKey} actual:{test.Value}");
                Assert.AreEqual(isKey, test.Value);
            });
        }

        [TestMethod]
        [TestDescription("--admin", true)]
        [TestDescription("--3admin", false)]
        public void Long_Option_Name_Must_Start_With_A_Letter()
        {
            Dictionary<string, bool> testsAndExpectedStatus = new Dictionary<string, bool>()
            {
                { "--admin", true },
                { "--3admin", false }
            };
            testsAndExpectedStatus.ToList().ForEach(test =>
            {
                bool isKey = CommandLineParser.Helpers.KeyDetection.GetLongKeyDetector().IsKey(test.Key);
                Console.WriteLine($"{test.Key} detected:{isKey} actual:{test.Value}");
                Assert.AreEqual(isKey, test.Value);
            });
        }

        [TestMethod]
        [TestDescription("program --name=mykeels", true)]
        public void Long_Option_Can_Be_Joined_To_Value_By_Equals_Sign()
        {
            string[] args = CommandManager.CommandLineToArgs("program --name=mykeels");
            CommandParser parser = new CommandParser(args);
            var model = parser.Parse<TestModel>();
            Assert.AreEqual(model.name, "mykeels");
        }

        [TestMethod]
        [TestDescription("program --name=mykeels --admin", true)]
        public void Long_Option_Can_Have_Space_Separated_Values()
        {
            string[] args = CommandManager.CommandLineToArgs("program --name=mykeels --admin");
            CommandParser parser = new CommandParser(args);
            var model = parser.Parse<TestModel>();
            Assert.IsTrue(model.isAdmin);
            Assert.AreEqual(model.name, "mykeels");
        }
    }
}
