using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Atlassian.Jira;
using RestSharp;
using Newtonsoft.Json;
using Atlassian.Jira.Linq;
using OfficeOpenXml;

namespace JiraQuery
{
    class Program
    {
        //private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);//adds logging

        static void Main(string[] args)
        {
            //Log.ConsoleOutput = false;
            Log.ExternalOutput = false; //TEMP

            DateTime now = DateTime.Now;
            if (Log.ExternalOutput) Log.LogName = (string.Format("{0} {1}{2}{3}{4}{5}{6} startup", now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second, now.Millisecond));

            Program p = new Program();

            JiraCredentials jCredentials = new JiraCredentials("IsaacA", "ia"); //login details

            Log.LogMessage(string.Format("[username] = {0}\n[password] = {1}", jCredentials.UserName, jCredentials.Password));

            string jiraUrl = "http://jira.c660cnc.siemens.cloud:8080"; //jira base url


            Log.LogMessage(string.Format("Connecting to \"{0}\"...", jiraUrl));

            var jiraConn = Jira.CreateRestClient(jiraUrl, jCredentials.UserName, jCredentials.Password); //connection to jira (doesn't open it here)

            Log.LogMessage("Rest Client Created");

            jiraConn.Issues.MaxIssuesPerRequest = 1000;

            p.Test(jiraConn);

            Console.ReadKey();
        }

        public async void Test(Jira jiraConn)
        {
            DateTime now = DateTime.Now;
            Log.LogName = (string.Format("{0} {1}{2}{3}{4}{5}{6}", now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second, now.Millisecond));
            Log.LogMessage("Connecting...", true);

            int returnCount = 0;
            int index = 0;
            int batchCount = 0;

            string query = "project = JEP";

            List<Issue> returnIssues = new List<Issue>();

            while (returnCount > -1)
            {
                batchCount++;
                Log.LogMessage(string.Format("Batch [{0}]...", batchCount), true);

                bool output = false;
                try
                {
                    var issues = await jiraConn.Issues.GetIssuesFromJqlAsync(query, jiraConn.Issues.MaxIssuesPerRequest, returnCount);
                    foreach (Issue issue in issues)
                    {
                        index++;
                        Log.LogMessage(index + ".\t" + issue.Type.Name);
                        returnIssues.Add(issue);
                        //Log.LogMessage(issue.CustomFields.FirstOrDefault(c => c.Name == "Blocked By")?.Values.FirstOrDefault() ?? "nope");
                        //Log.LogMessage(issue.CustomFields.ToList().FirstOrDefault().Name);
                        output = true;
                    }
                }
                catch
                {
                    Log.LogMessage("DOWNLOAD FAILED! BAD QUERY OR INVALID CREDENTIALS");
                    output = false;
                }

                if (!output)
                {
                    Log.LogMessage("Finish\n");
                    returnCount = int.MinValue;
                    continue;
                    
                }
                returnCount = index;
            }

            Excel.CreateExport("", "test.xlsx", returnIssues);

        }
            
        /*public void CreateExcel(List<Issue> issues)
        {
            var newFile = new FileInfo("test.xlsx");

            foreach (Issue issue in issues)
            {
                for (int x = 0; x < issue.CustomFields.Count; x++)
                {
                    Console.Write((issue.CustomFields.ElementAt(x).Name) + "\t");
                }
                Console.WriteLine();
            }


            using (ExcelPackage package = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = package.Workbook.Worksheets.Add("Sheet22");

                ws.Cells[1, 1].Value = "test";
                package.Save();
            }
        }*/

    }
}

