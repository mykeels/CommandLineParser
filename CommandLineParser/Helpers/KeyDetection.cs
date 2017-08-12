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
        public const string SHORT_KEY_REGEX = @"^-(\w)+((\=)?\w+)?$";
        public const string LONG_KEY_REGEX = @"^--[a-z]+(=(\w)+)?$";
        private ShortKeyDetection _shortDetector;

        public KeyDetection()
        {
            this._shortDetector = new ShortKeyDetection();
        }

        public ShortKeyDetection GetShortKeyDetector()
        {
            return this._shortDetector;
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

            public bool IsAggregated(string potentialKey, char[] requiredKeyList)
            {
                var keys = potentialKey.AsEnumerable().Skip(2);
                return keys.All(key => requiredKeyList.Contains(key));
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
