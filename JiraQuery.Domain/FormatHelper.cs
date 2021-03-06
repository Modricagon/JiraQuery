﻿using Newtonsoft.Json.Linq;
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
            switch (headerName)
            {
                case "Blocked by":
                    return Blocked_by(value);
                case "Epic Status":
                    return Epic_Status(value);
                case "Bug Traced To":
                    return Bug_Traced_To(value);
                case "Relates To":
                    return Relates_To(value);
                case "Traces to":
                    return Traces_to(value);
                case "Traced by":
                    return Traced_by(value);
                case "Test Case Link":
                    return Test_Case_Link(value);
                default:
                    if (value.Substring(0, 1) == "[" || value.Substring(0, 1) == "{")
                    {
                        if (!needsFormatting.Contains(headerName))
                        {
                            needsFormatting.Add(headerName);
                            Log.LogMessage(string.Format("[WARNING] Field {0} may be unformatted...",headerName));
                        }
                    }
                    return value;
            }
        }

        static string Blocked_by(string value) //formats the relevant column
        {
            return JsonSelection(value, "key"); //returns Json value from selected id
        }

        static string Epic_Status(string value)
        {
            return JsonSelection(value, "value");
        }

        static string Bug_Traced_To(string value)
        {
            return JsonSelection(value, "key");
        }

        static string Relates_To(string value)
        {
            return JsonSelection(value, "key");
        }

        static string Traces_to(string value)
        {
            return JsonSelection(value, "key");
        }

        static string Traced_by(string value)
        {
            return JsonSelection(value, "key");
        }

        static string Test_Case_Link(string value)
        {
            return JsonSelection(value, "key");
        }

        public static string JsonSelection(string source, string id) //selects value from json
        {
            string result = "";
            try
            {
                JArray jo = (JArray)JsonConvert.DeserializeObject(source);
                foreach (JObject Object in jo)
                {
                    result = result + Object.Property(id).Value + ", ";
                }
            }
            catch
            {
                JObject jo = (JObject)JsonConvert.DeserializeObject(source);
                result = result + jo.Property(id).Value + ", ";
            }
            return result;
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
