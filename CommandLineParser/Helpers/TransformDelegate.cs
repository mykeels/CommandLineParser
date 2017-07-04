using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandLineParser.Helpers
{
    public delegate T TransformDelegate<T>(string address);
    public delegate object TransformDelegate(string address);
}
