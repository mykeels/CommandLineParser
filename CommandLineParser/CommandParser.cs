using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using CommandLineParser.Attributes;

namespace CommandLineParser
{
    public class CommandParser
    {
        private const string HELP_KEY_REGEX = @"^(--help)|(-h)|(-\?)|(\\\?)$";
        private const string KEY_REGEX = @"^--?(\w|\?)+$";
        private string[] _args { get; set; }

        public CommandParser(string[] _args = null)
        {
            this._args = _args;
        }

        public Dictionary<string, string> GetDictionary(string[] args = null)
        {
            args = args ?? this._args ?? new string[] { };
            this._args = args;
            var ret = new Dictionary<string, string>();
            bool isKey = false;
            string key = string.Empty;
            foreach (string arg in args)
            {
                isKey = Regex.IsMatch(arg, KEY_REGEX);
                if (isKey)
                {
                    ret[arg] = string.Empty;
                    key = arg;
                }
                else
                {
                    ret[key] += arg;
                }
            }
            return ret;
        }

        public TData Parse<TData>(string[] args = null)
        {
            args = args ?? this._args ?? new string[] { };
            this._args = args;
            var _dict = GetDictionary(args);
            TData ret = System.Activator.CreateInstance<TData>();
            System.Reflection.PropertyInfo[] properties = ret.GetType().GetProperties();
            foreach (var property in properties)
            {
                var flag = property.GetCustomAttribute<FlagAttribute>();
                var help = property.GetCustomAttribute<HelpAttribute>();
                var transform = property.GetCustomAttribute<TransformAttribute>();
                flag = flag ?? new FlagAttribute(property.Name);
                string keyPattern = $@"(^-{flag.ShortName}$)|(^--{flag.Name}$)";
                string key = _dict.Keys.FirstOrDefault(_key => Regex.IsMatch(_key, keyPattern, RegexOptions.IgnoreCase));
                if (key == null && flag.Required)
                {
                    throw new Exception($"Could not find argument key for --{property.Name} which is required");
                }
                else if (key != null)
                {   
                    switch (property.PropertyType.Name)
                    {
                        case "String":
                            try { property.SetValue(ret, _dict[key]); } catch { throw new Exception($"Could not convert argument value of \"{_dict[key]}\" to String"); }
                            break;
                        case "Int32":
                            try { property.SetValue(ret, Convert.ToInt32(_dict[key])); } catch { throw new Exception($"Could not convert argument value of \"{_dict[key]}\" to Int32"); }
                            break;
                        case "Int64":
                            try { property.SetValue(ret, Convert.ToInt64(_dict[key])); } catch { throw new Exception($"Could not convert argument value of \"{_dict[key]}\" to Int64"); }
                            break;
                        case "DateTime":
                            try { property.SetValue(ret, Convert.ToDateTime(_dict[key])); } catch { throw new Exception($"Could not convert argument value of \"{_dict[key]}\" to DateTime"); }
                            break;
                        case "Boolean":
                            try { property.SetValue(ret, Convert.ToBoolean(_dict[key])); } catch { throw new Exception($"Could not convert argument value of \"{_dict[key]}\" to Boolean"); }
                            break;
                        default:
                            if (transform != null)
                            {
                                property.SetValue(ret, transform.Execute.DynamicInvoke(_dict[key]));
                            }
                            else
                            {
                                throw new Exception($"Type of [{property.Name}] is not recognized");
                            }
                            break;
                    }
                }
            }
            return ret;
        }

        private bool _matchesCliKey(PropertyInfo property, string key)
        {
            var flag = property.GetCustomAttribute<FlagAttribute>();
            flag = flag ?? new FlagAttribute(property.Name);
            string keyPattern = $@"(^-{flag.ShortName}$)|(^--{flag.Name}$)";
            return Regex.IsMatch(key, keyPattern);
        }

        private string _getPropKeyHelpInfo(PropertyInfo property, bool includeDescription = false)
        {
            var flag = property.GetCustomAttribute<FlagAttribute>();
            var help = property.GetCustomAttribute<HelpAttribute>();
            flag = flag ?? new FlagAttribute(property.Name);
            string shortName = flag.ShortName != null ? $"-{flag.ShortName} " : null;
            string name = flag.Name != null ? $"--{flag.Name}" : null;
            string helpText = includeDescription ? (help != null ? $" ({help.Usage})" : string.Empty) : string.Empty;
            return (flag.Required ? $"{shortName}{name}{helpText}" : $"[{shortName}{name}{helpText}]");
        }

        public string GetHelpInfo<TData>(string[] args = null)
        {
            args = args ?? this._args ?? new string[] { };
            this._args = args;
            var _dict = GetDictionary(args);
            string key = string.Empty;
            for (int i = 0; i < args.Length; i++)
            {
                string arg = args[i];
                if (Regex.IsMatch(arg, KEY_REGEX) != Regex.IsMatch(arg, HELP_KEY_REGEX))
                {
                    key = arg;
                }
                else if (Regex.IsMatch(arg, HELP_KEY_REGEX))
                {
                    if (string.IsNullOrEmpty(key))
                    {
                        //first help text in argument
                        var help = typeof(TData).GetCustomAttribute<HelpAttribute>();
                        if (help != null) return $"========== Help Information ==========\n {help.Usage}\n\n" +
                                                $"{System.AppDomain.CurrentDomain.FriendlyName} " +
                                                String.Join(" ", System.Activator.CreateInstance<TData>().GetType().GetProperties().Select(prop => _getPropKeyHelpInfo(prop)).ToArray()) +
                                                "\n========== End Help Information ==========";
                    }
                    else
                    {
                        var property = System.Activator.CreateInstance<TData>().GetType().GetProperties().FirstOrDefault(prop => _matchesCliKey(prop, args[i - 1]));
                        if (property != null)
                        {
                            var help = property.GetCustomAttribute<HelpAttribute>();
                            if (help != null)
                            {
                                return "Usage: " + _getPropKeyHelpInfo(property, true);
                            }
                            else return $"No Help Attribute found on Property [{property.Name}]";
                        }
                        else return $"Invalid Help Request";
                    }
                }
            }
            return null;
        }
    }
}
