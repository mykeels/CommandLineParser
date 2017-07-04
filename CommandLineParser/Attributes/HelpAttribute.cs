using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandLineParser.Attributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
    public sealed class HelpAttribute : Attribute
    {
        readonly string usageText;

        // This is a positional argument
        public HelpAttribute(string usageText)
        {
            this.usageText = usageText;
        }

        public string Usage
        {
            get { return usageText; }
        }
    }
}
