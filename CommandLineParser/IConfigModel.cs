using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandLineParser
{
    public interface IConfigModel
    {
        string[] extras { get; set; }
    }
}
