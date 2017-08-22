using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandLineParser.Tests.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class TestDescriptionAttribute: Attribute
    {
        private string _message;
        public TestDescriptionAttribute(string message, params object[] otherStuff)
        {
            _message = message;
        }
    }
}
