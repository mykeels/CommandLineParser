using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandLineParser
{
    public interface IConfigModel
    {
        string[] Options { get; set; }
        string[] Extras { get; set; }
    }
}
