using System;

namespace CommandLineParser.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class HelpAttribute : Attribute
    {
        private readonly string usageText;
        
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
