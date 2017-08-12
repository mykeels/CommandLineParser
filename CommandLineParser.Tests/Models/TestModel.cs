using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLineParser.Attributes;

namespace CommandLineParser.Tests.Models
{
    public class TestModel
    {
        [Flag("username", "u")]
        public string username { get; set; }

        [Flag("name", "n")]
        public string name { get; set; }

        [Flag("admin", "a")]
        public bool isAdmin { get; set; }

        [Flag("superadmin", "s")]
        public bool isSuperAdmin { get; set; }
    }
}
