using Atlassian.Jira;
using JiraQuery.Domain;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiraQuery.Test
{
    [TestFixture]
    public class Tests
    {
        private string _url;
        private string _username;
        private string _password;
        private Jira _jiraConn;

        private string _query;
        private List<Issue> _issues = new List<Issue>();

        [SetUp]
        public void SetUp()
        {
            _url = Config.url;
            _username = Config.username;
            _password = Config.password;

            _query = Config.query;
        }

        [Test]
        public void Connect()
        {
            _jiraConn = Jira.CreateRestClient(_url, _username, _password);
        }

        [Test]
        public async Task Download()
        {
            Connect();
            int index = 0;
            int returnCount = 0;
            _issues.Clear();

            while (returnCount > -1)
            {
                bool output = false;
                var issues = await _jiraConn.Issues.GetIssuesFromJqlAsync(_query, _jiraConn.Issues.MaxIssuesPerRequest, returnCount);
                    foreach (Issue issue in issues)
                    {
                        index++;
                        _issues.Add(issue);
                        output = true;
                    }
                if (!output)
                {
                    returnCount = int.MinValue;
                    continue;
                }
                returnCount = index;
            }
        }

    }
}
