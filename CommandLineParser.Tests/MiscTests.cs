using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CommandLineParser.Tests.Helpers;
using CommandLineParser.Tests.Models;
using CommandLineParser.Tests.Attributes;
using Newtonsoft.Json;

namespace CommandLineParser.Tests
{
    [TestClass]
    public class MiscTests
    {
        [TestMethod]
        [TestDescription("program --dataset=1 --runs=5 --switch-probability=0.7 --iterations 100 -o ../project/folder")]
        public void Test_That_Long_Names_Work()
        {
            string[] args = CommandManager.CommandLineToArgs("program --dataset=1 --runs=5 --switch-probability=0.7 --iterations 100 -o ../project/folder");
            CommandParser parser = new CommandParser(args);
            Console.WriteLine(JsonConvert.SerializeObject(parser.GetDictionary()));
            var model = parser.Parse<MiscTestModel>();
            Assert.AreEqual(1, model.Dataset);
            Assert.AreEqual(5, model.Runs);
            Assert.AreEqual(0.7, model.SwitchProbability);
            Assert.AreEqual(100, model.NoOfIterations);
            Assert.AreEqual("../project/folder", model.Output);
        }
        [TestMethod]
        [TestDescription("program -d=1 -r=5 -x=0.7 -i 100 --output ../project/folder")]
        public void Test_That_Short_Names_Work()
        {
            string[] args = CommandManager.CommandLineToArgs("program -d=1 -r=5 -x=0.7 -i 100 --output ../project/folder");
            CommandParser parser = new CommandParser(args);
            Console.WriteLine(JsonConvert.SerializeObject(parser.GetDictionary()));
            var model = parser.Parse<MiscTestModel>();
            Assert.AreEqual(1, model.Dataset);
            Assert.AreEqual(5, model.Runs);
            Assert.AreEqual(0.7, model.SwitchProbability);
            Assert.AreEqual(100, model.NoOfIterations);
            Assert.AreEqual("../project/folder", model.Output);
        }
    }
}
