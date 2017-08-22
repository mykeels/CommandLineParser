using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using CommandLineParser.Attributes;
using CommandLineParser.Helpers;

namespace CommandLineParser
{
    public class CommandParser
    {
        private const string HELP_KEY_REGEX = @"^(--help)|(-h)|(-\?)|(\\\?)$";
        private const string KEY_REGEX = @"^--?(\w|\?)+$";
        private string[] _args { get; set; }

        public CommandParser()
        {
        }

        public CommandParser(string[] _args = null)
        {
            this._args = _args;
        }

        public Dictionary<string, List<string>> GetDictionary(string[] args = null, Type targetType = null)
        {
            args = args ?? this._args ?? new string[] { };
            this._args = args;
            var ret = new Dictionary<string, List<string>>();
            string key = string.Empty;
            foreach (string arg in args)
            {
                if (KeyDetection.GetShortKeyDetector().IsKey(arg))
                {
                    if (KeyDetection.GetShortKeyDetector().IsJoinedToValue(arg))
                    {
                        string[] split = arg.Split('=');
                        foreach (char potentialKey in split.First().AsEnumerable().Skip(1))
                        {
                            key = "-" + potentialKey;
                            ret[key] = new List<string>();
                        }
                        ret[key].Add(split.Last()); //to hold the value of the key before the equals sign incase of aggregation like -asu=mykeels
                    }
                    else if (KeyDetection.GetShortKeyDetector().IsAggregated(arg, _getShortKeys(targetType))) //works on aggregate short keys like -sa
                    {
                        foreach (string potentialKey in KeyDetection.GetShortKeyDetector().GetAggregatedKeys(arg, _getShortKeys(targetType)))
                        {
                            ret[potentialKey] = new List<string>();
                            key = potentialKey;
                        }
                    }
                    else if (KeyDetection.GetShortKeyDetector().IsFollowedByValue(arg))
                    {
                        var result = KeyDetection.GetShortKeyDetector().IsFollowedByValue(arg, _getShortKeys(targetType));
                        string[] keys = result.Item2.Key;
                        string value = result.Item2.Value;
                        foreach (string potentialKey in keys)
                        {
                            key = potentialKey;
                            ret[key] = new List<string>();
                        }
                        ret[key].Add(value);
                    }
                    else
                    {
                        ret[arg] = new List<string>(); //works for normal short keys like -u
                        key = arg;
                    }
                }
                else if (KeyDetection.GetLongKeyDetector().IsKey(arg))
                {
                    if (KeyDetection.GetLongKeyDetector().IsJoinedToValue(arg))
                    {
                        string[] split = arg.Split('=');
                        key = split.First();
                        if (ret.ContainsKey(key)) ret[key].Add(split.Last());
                        else ret[key] = new List<string>() { split.Last() };
                    }
                    else
                    {
                        ret[arg] = new List<string>();
                        key = arg;
                    }
                }
                else
                {
                    if (ret.ContainsKey(key)) ret[key].Add(arg);
                    else ret[key] = new List<string>() { arg };
                }
            }
            return ret;
        }

        private char[] _getShortKeys(Type targetType)
        {
            List<string> ret = new List<string>();
            PropertyInfo[] properties = targetType.GetProperties();
            foreach (var property in properties)
            {
                var flag = property.GetCustomAttribute<FlagAttribute>();
                ret.Add(flag.ShortName);
            }
            return ret.Select((key) => key.ElementAt(0)).ToArray();
        }

        public TData Parse<TData>(string[] args = null)
        {
            args = args ?? this._args ?? new string[] { };
            this._args = args;
            var _dict = GetDictionary(args, typeof(TData));
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
                    _setPropertyValue<TData>(ret, property, _dict[key], transform);
                }
            }
            return ret;
        }

        private void _setPropertyValue<TData>(TData ret, PropertyInfo property, List<string> values, TransformAttribute transform = null)
        {
            string flagValue = string.Join(null, values);
            switch (property.PropertyType.Name)
            {
                case "String":
                    try { property.SetValue(ret, flagValue); } catch { throw new Exception($"Could not convert argument value of \"{flagValue}\" to String"); }
                    break;
                case "Int32":
                    try { property.SetValue(ret, Convert.ToInt32(flagValue)); } catch { throw new Exception($"Could not convert argument value of \"{flagValue}\" to Int32"); }
                    break;
                case "Int64":
                    try { property.SetValue(ret, Convert.ToInt64(flagValue)); } catch { throw new Exception($"Could not convert argument value of \"{flagValue}\" to Int64"); }
                    break;
                case "DateTime":
                    try { property.SetValue(ret, Convert.ToDateTime(flagValue)); } catch { throw new Exception($"Could not convert argument value of \"{flagValue}\" to DateTime"); }
                    break;
                case "Boolean":
                    if (string.IsNullOrWhiteSpace(flagValue)) property.SetValue(ret, true); //default to true if no value is specified cos it's a boolean
                    else try { property.SetValue(ret, Convert.ToBoolean(flagValue)); } catch { throw new Exception($"Could not convert argument value of \"{flagValue}\" to Boolean"); }
                    break;
                default:
                    if (transform != null)
                    {
                        property.SetValue(ret, transform.Execute.DynamicInvoke(flagValue));
                    }
                    else
                    {
                        if (property.PropertyType.BaseType.Name == "Enum")
                        {
                            try { property.SetValue(ret, Enum.Parse(property.PropertyType, flagValue)); } catch { throw new Exception($"Could not convert argument value of \"{flagValue}\" to Enum"); }
                        }
                        else throw new Exception($"Type of [{property.Name}] is not recognized");
                    }
                    break;
            }
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
