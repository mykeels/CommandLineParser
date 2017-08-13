using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace CommandLineParser.Helpers
{
    public class KeyDetection
    {
        public const string HELP_KEY_REGEX = @"^(--help)|(-h)|(-\?)|(\\\?)$";
        public const string SHORT_KEY_REGEX = @"^-([a-zA-Z])(\w*)((\=)?\w+)?$";
        public const string LONG_KEY_REGEX = @"^--[a-z]+(=(\w)+)?$";
        private static ShortKeyDetection _shortDetector;
        private static LongKeyDetection _longDetector;

        public static ShortKeyDetection GetShortKeyDetector()
        {
            if (_shortDetector == null) _shortDetector = new ShortKeyDetection();
            return _shortDetector;
        }

        public static LongKeyDetection GetLongKeyDetector()
        {
            if (_longDetector == null) _longDetector = new LongKeyDetection();
            return _longDetector;
        }

        public class ShortKeyDetection
        {
            private const string KEY_JOINS_VALUE_REGEX = @"=\w+$"; //-u=name
            public bool IsKey(string potentialKey)
            {
                return Regex.IsMatch(potentialKey, KeyDetection.SHORT_KEY_REGEX);
            }

            public bool IsJoinedToValue(string potentialKey)
            {
                return Regex.IsMatch(potentialKey, KEY_JOINS_VALUE_REGEX);
            }

            public bool IsFollowedByValue(string potentialKey)
            {
                return potentialKey.Length > 2;
            }

            public Tuple<bool, KeyValuePair<string[], string>> IsFollowedByValue(string potentialKey, char[] requiredKeyList)
            {
                Func<string, char[], KeyValuePair<string[], string>> getValueFn = (string pKey, char[] rList) =>
                 {
                     int indexOfNotKey = -1;
                     List<string> keys = new List<string>();
                     for (int i = 1; i < pKey.Length; i++)
                     {
                         if (!requiredKeyList.Contains(pKey[i]))
                         {
                             indexOfNotKey = i;
                             break;
                         }
                         else
                         {
                             keys.Add("-" + pKey[i]);
                         }
                     }
                     return new KeyValuePair<string[], string>(keys.ToArray(), string.Join(string.Empty, pKey.Skip(indexOfNotKey)));
                 };
                return new Tuple<bool, KeyValuePair<string[], string>>(IsFollowedByValue(potentialKey), getValueFn(potentialKey, requiredKeyList));
            }

            public bool IsAggregated(string potentialKey, char[] requiredKeyList)
            {
                var keys = potentialKey.AsEnumerable().Skip(1);
                return keys.All(key => requiredKeyList.Contains(key));
            }

            public string[] GetAggregatedKeys(string potentialKey, char[] requiredKeyList)
            {
                var keys = potentialKey.AsEnumerable().Skip(1);
                return keys.Where(key => requiredKeyList.Contains(key)).Select(key => "-" + key).ToArray();
            }
        }

        public class LongKeyDetection
        {
            private const string KEY_JOINS_VALUE_REGEX = @"=\w+$"; //--user=name
            public bool IsKey(string potentialKey)
            {
                return Regex.IsMatch(potentialKey, KeyDetection.LONG_KEY_REGEX);
            }

            public bool IsJoinedToValue(string potentialKey)
            {
                return Regex.IsMatch(potentialKey, KEY_JOINS_VALUE_REGEX);
            }
        }
    }
}
