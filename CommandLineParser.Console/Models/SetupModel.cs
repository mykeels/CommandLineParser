using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLineParser;
using CommandLineParser.Attributes;
using CommandLineParser.Helpers;

namespace CommandLineParser.Console.Models
{
    [Help("This is a Command Line Parser Library")]
    public class SetupModel
    {
        [Flag(shortName: "n", required: true)]
        [Help("The name of the User")]
        public string Name { get; set; }

        [Flag("free", "f")]
        [Help("This user is not enslaved")]
        public bool IsFree { get; set; }

        [Help("The user's age")]
        public int Age { get; set; }

        [Flag("address", "a", false)]
        [Help("The user's address")]
        [Transform(typeof(SetupModel), nameof(_addressTransform))]
        public AddressModel Address { get; set; }

        public static AddressModel _addressTransform(string address)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<AddressModel>(address);
        }
    }

    public class AddressModel
    {
        public string HomeAddress { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
    }
}
