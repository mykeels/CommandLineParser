using System;
using CommandLineParser.Exceptions;

namespace CommandLineParser.Attributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class FlagAttribute : Attribute
    {
        private readonly string shortName;
        private readonly string name;
        public readonly bool required;
        
        public FlagAttribute(string name = null, string shortName = null, bool required = false)
        {
            this.name = name;
            this.required = required;
            this.shortName = shortName;
            if (!string.IsNullOrWhiteSpace(shortName) && shortName.Length > 1) throw new LengthExceededException(1);
            if (!string.IsNullOrWhiteSpace(name)) this.name = this.name.ToLower();
        }

        public string Name
        {
            get { return name; }
        }

        public string ShortName
        {
            get { return shortName; }
        }

        public bool Required
        {
            get { return required; }
        }
    }
}
