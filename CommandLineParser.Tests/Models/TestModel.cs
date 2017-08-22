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

        [Flag("role", "r")]
        public List<Role> roles { get; set; } = new List<Role>();

        [Flag("admin", "a")]
        public bool isAdmin {
            get
            {
                return roles.Any(role => role == Role.Admin);
            }
            set
            {
                if (!roles.Any(role => role == Role.Admin)) roles.Add(Role.Admin);
            }
        }

        [Flag("superadmin", "s")]
        public bool isSuperAdmin
        {
            get
            {
                return roles.Any(role => role == Role.SuperAdmin);
            }
            set
            {
                if (!roles.Any(role => role == Role.SuperAdmin)) roles.Add(Role.SuperAdmin);
            }
        }

        public enum Role
        {
            User,
            Admin,
            SuperAdmin
        }
    }
}
