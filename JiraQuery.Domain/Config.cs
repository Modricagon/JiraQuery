using System;
using System.Collections.Generic;

namespace JiraQuery.Domain
{
    public static class Config
    {
        public static string url = "http://jira.c660cnc.siemens.cloud:8080";
        public static string username = "IsaacA";
        public static string password = "ia";

        public static string query = null;
        //public static string query = "project = JEP";

        public static List<string> Dates = new List<string>() { "Created", "DueDate", "Updated", "ResolutionDate" };

        public static List<string> headerIgnore = new List<string>() { "AdditionalFields", "Jira", "TimeTrackingData", "Development" };
    }
}
