using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLineParser.Attributes;

namespace CommandLineParser.Tests.Models
{
    public class TestNoHelpTextModel
    {
        [Flag("username", "u")]
        [Help("This is the Username property")]
        public string Username { get; set; }

        [Flag("name", "n")]
        public string Name { get; set; }

        [Flag("role", "r")]
        public List<Role> Roles { get; set; } = new List<Role>();

        [Flag("admin", "a")]
        public bool IsAdmin
        {
            get
            {
                return Roles.Any(role => role == Role.Admin);
            }
            set
            {
                if (!Roles.Any(role => role == Role.Admin)) Roles.Add(Role.Admin);
            }
        }

        [Flag("superadmin", "s")]
        public bool IsSuperAdmin
        {
            get
            {
                return Roles.Any(role => role == Role.SuperAdmin);
            }
            set
            {
                if (!Roles.Any(role => role == Role.SuperAdmin)) Roles.Add(Role.SuperAdmin);
            }
        }

        public string Gender { get; set; }

        public string[] Extras { get; set; }
        public string[] Options { get; set; }

        public enum Role
        {
            User,
            Admin,
            SuperAdmin
        }
    }
}
