using System;
using System.Collections.Generic;

namespace JiraQuery.Domain
{
    public static class Config
    {
        public static string url = "http://jira.c660cnc.siemens.cloud:8080";
        public static string username = "IsaacA";
        public static string password = "ia";

        public static string query = "project = CET AND issuetype in (\"IFAT Bug\") AND affectedVersion in (4.1.0.11) AND affectedVersion not in (3.1.2, 3.1.2.1, 3.1.2.2, 3.1.2.3, 3.1.2.5, 3.1.3, 3.1.3.2, 3.1.3.4, 3.1.3.5, 3.1.3.6, 3.1.3.7, 3.4.0, 3.4.1, 3.4.2, 3.4.3, 3.4.4, 3.4.5, 3.4.6, 3.4.7, 3.4.8, 3.4.9, 4.0.0.7) ORDER BY Severity ASC, key ASC";
        //public static string query = "project = JEP";

        public static List<string> Dates = new List<string>() { "Created", "DueDate", "Updated", "ResolutionDate" };

        public static List<string> headerIgnore = new List<string>() { "AdditionalFields", "Jira", "TimeTrackingData", "Development" };
    }
}
