using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace JiraQuery.Domain
{
    public static class FormatHelper
    {
        static List<string> needsFormatting = new List<string>();

        public static string Format(string headerName, string value)
        {
            if (value.Length > 100)
            {
                if (!needsFormatting.Contains(headerName))
                {
                    needsFormatting.Add(headerName);
                    Log.LogMessage(headerName);
                }
            }
            switch (headerName)
            {
                case "Blocked by":
                    return Blocked_by(value);
                case "Epic Status":
                    return Epic_Status(value);
                case "Bug Traced To":
                    return Bug_Traced_To(value);
                default:
                    return value;
            }
        }

        static string Blocked_by(string value) //formats the blocked by column
        {
            return GetStringSelection(value, "\"key\": \"", 8, true);
        }

        static string Epic_Status(string value)
        {
            return GetStringSelection(value, "\"value\": \"", 10);
        }

        static string Bug_Traced_To(string value)
        {
            //return GetStringSelection(value, "\"key\": \"", 8, true);
            string result = "";
            var jo = JObject.Parse(value);
            var data = (JObject)jo["key"];
            foreach (var item in data)
            {
                result = result + item.Value + ", ";
            }
            return result;
        }

        static string Relates_To(string value)
        {
            return "";
        }

        static string Traces_to(string value)
        {
            return "";
        }

        static string Blocked_Reason_Information(string value)
        {
            return "";
        }

        static string I_want(string value)
        {
            return "";
        }

        static string Defect_Retest_Results(string value)
        {
            return "";
        }

        static string As(string value)
        {
            return "";
        }

        static string Developer_Comments(string value)
        {
            return "";
        }

        static string Historic_Information(string value)
        {
            return "";
        }

        public static string GetStringSelection(string source, string searchString, int substringOffset ,bool multipleValues = false) //gets indexes of start of specific string inside another string
        {
            List<int> ret = new List<int>();
            int len = searchString.Length;
            int start = -len;
            while (true)
            {
                start = source.IndexOf(searchString, start + len);
                if (start == -1)
                {
                    break;
                }
                else
                {
                    ret.Add(start);
                }
            }

            string result = "";
            foreach (int startInt in ret)
            {
                string[] split = source.Substring(startInt + substringOffset).Split('"');
                if (multipleValues) result = result + split[0] + ", ";
                else result = result + split[0];
            }
            return result;
        }
    }
}
