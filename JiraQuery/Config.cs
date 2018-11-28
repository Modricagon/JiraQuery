using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiraQuery
{
    public static class Config
    {
        static string url; //base url jira.x.x.x.x:port
        static string username; //jira username
        static string password; //jira password

        static string jqlQuery; //copy and pasted from jira site
        static string issuesPerRequest; //max 1000

        static string consoleOutput; //messages outputted to console
        static string externalOutput; //messages stored in log

        static string docPath;
        static string docName;
        static string workSheetName; //Name of each sheet followed by number e.g. Sheet1, Sheet2, Sheet3

        //static List<string> columnHeadings = new List<string>() { "Summary", "Issue key", "Issue id", "Parent id", "Issue Type", "Status", "Project key", "Project name", "Project type", "Project lead", "Project description", "Project url", "Priority", "Resolution", "Assignee", "Reporter", "Creator", "Created Updated", "Last Viewed Resolved", "Due Date", "Votes", "Description", "Environment Watchers", "Log Work", "Original Estimate", "Remaining Estimate", "Time Spent", "Work Ratio", "Î£ Original Estimate", "Î£ Remaining Estimate", "Î£ Time Spent", "Security Level", "Outward issue link (Blocks)", "Custom field (Account)", "Custom field (Affected Version (Text))", "Custom field (As)", "Custom field (Blocked by)", "Custom field (Blocked by)", "Custom field (Bug Traced To)", "Custom field (Change Notice)", "Custom field (Change Source)", "Custom field (Check In Reference)", "Custom field (Developer Comments)", "Custom field (Engineering Prime)", "Custom field (Epic Color)", "Custom field (Epic Link)", "Custom field (Epic Name)", "Custom field (Epic Status)", "Custom field (FOR status category)", "Custom field (Fix Details)", "Custom field (Fix Group)", "Custom field (Historic Information)", "Custom field (I want)", "Custom field (Iteration)", "Custom field (Legacy ID)", "Custom field (New/Historic)", "Custom field (Ninject RCM)", "Custom field (Owner)", "Custom field (PA Reference)", "Custom field (Project)", "Custom field (Rank)", "Custom field (Relates To)", "Custom field (Release)", "Custom field (Repository)   Custom field (Revision) Custom field (Severity) Custom field (Story Id) Custom field (Story Verified By)    Custom field (System)   Custom field (T&C Priority) Custom field (Team) Custom field (Team Role)    Custom field (Test) Custom field (Test Script Author)   Custom field (Test Script Creation Date)    Custom field (Test Script Id)   Custom field (Test Script Location) Custom field (Test Script Review Date)  Custom field (Test Script Reviewer) Custom field (Test Script Status)   Custom field (Test Spec Reference)  Custom field (Test Step)    Custom field (Test Team)    Custom field (Traces to)    Custom field (W.I Ref)  Custom field (so that)

    }
}
