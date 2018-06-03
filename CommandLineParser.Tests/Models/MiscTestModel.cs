using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLineParser.Attributes;
using Newtonsoft.Json;

namespace CommandLineParser.Tests.Models
{
    [Help("== This is a Test Model ==")]
    public class MiscTestModel: IConfigModel
    {
        [Flag("dataset", "d")]
        public int Dataset { get; set; }

        [Flag("runs", "r")]
        public int Runs { get; set; }

        [Flag("switch-probability", "x")]
        public double SwitchProbability { get; set; }

        [Flag("iterations", "i")]
        public int NoOfIterations { get; set; }

        [Flag("output", "o")]
        public string Output { get; set; }

        public string[] Extras { get; set; }
        public string[] Options { get; set; }
    }
}
