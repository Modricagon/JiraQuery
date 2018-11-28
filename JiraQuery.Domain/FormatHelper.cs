using System;
using System.Collections.Generic;
using System.Text;

namespace JiraQuery.Domain
{
    public static class FormatHelper
    {
        public static string Format(string headerName, string value)
        {
            switch (headerName)
            {
                case "Blocked By":
                    return Blocked_By(value);
                case "ResolutionDate":
                    return dateTime(value);
                    break;
                default:
                    return value;
            }
        }

        static string dateTime(string value)
        {
            //return DateTime.FromOADate(Convert.ToDouble(value)).ToShortDateString();
            return "working";
        }

        static string Blocked_By(string value)
        {
            return "";
        }
    }
}
