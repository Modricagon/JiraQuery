using Atlassian.Jira;
using HelperClasses;
using JiraQuery.Domain;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiraQuery
{
    public static class Excel
    {
        public static void GenerateWorksheet(ExcelWorksheets worksheets, DataTable dataTable, string strWorksheetName)
        {
            try
            {
                ExcelWorksheet ws;
                if (worksheets.Count(w => w.Name == strWorksheetName) == 0)
                {
                    ws = worksheets.Add(strWorksheetName);
                    Log.LogMessage("Adding new Worksheet: " + strWorksheetName);
                }
                else
                {
                    ws = worksheets[strWorksheetName];
                    ws.Cells.Clear();
                    Log.LogMessage("Clearing Values for Worksheet: " + strWorksheetName);
                }

                int cols = ws.Dimension?.Columns ?? dataTable.Columns.Count;

                object[] columns = dataTable.Rows[0].ItemArray;

                ws.Cells["A1"].LoadFromDataTable(dataTable, true);



                for (int x = 1; x <= columns.Count(); x++)
                {
                    if (Config.Dates.Contains(ws.Cells[1, x].Value))
                    {
                        ws.Column(x).Style.Numberformat.Format = DateTimeFormatInfo.CurrentInfo.ShortDatePattern;
                    }
                }


                Log.LogMessage("Added data to Worksheet: " + strWorksheetName);

                // format header cells
                using (var range = ws.Cells[1, 1, 1, cols])
                {
                    Log.LogMessage("Formating Header Cells for Worksheet: " + strWorksheetName);
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
                    range.Style.Font.Color.SetColor(Color.White);
                }

                // Create autofilter for the range of cells
                ws.Cells[ws.Dimension?.Address].AutoFilter = true;
                // Autofit columns for all cells
                ws.Cells[ws.Dimension?.Address].AutoFitColumns();
                Log.LogMessage("Created " + strWorksheetName + " Worksheet");
            }
            catch (Exception ex)
            {
                Log.LogMessage("Error Creating " + strWorksheetName + ", Error: " + ex.Message);
            }
        }


        public static string CreateExport(string sPath, string sFileName, List<Issue> issues)
        {
            StringBuilder sb = new StringBuilder();

            try
            {
                string strFileName = Path.Combine(sPath, sFileName);
                DataTable dtSummary = Helper.ToDataTableWithNulls(issues, Config.headerIgnore);

                FileInfo excelFile = new FileInfo(strFileName);

                if (excelFile.Exists)
                {
                    Log.LogMessage("Deleting Report Excel file: " + strFileName);
                    excelFile.Delete();
                    excelFile = new FileInfo(strFileName);
                    sb.AppendLine("Deleted Report Excel file: " + strFileName);
                }

                Log.LogMessage("Creating File: " + strFileName);
                sb.AppendLine("Creating File: " + strFileName);
                using (ExcelPackage package = new ExcelPackage(excelFile))
                {
                    ExcelWorksheets worksheets = package.Workbook.Worksheets;
                    Log.LogMessage("Generating Summary Worksheet");
                    sb.AppendLine("Generating Summary Worksheet");
                    //Application.DoEvents();
                    GenerateWorksheet(worksheets, dtSummary, "Query");
                    package.Save();
                }
                Log.LogMessage("Created File: " + strFileName);
                sb.AppendLine("Created File: " + strFileName);
            }
            catch (Exception ex)
            {
                string s = ex.Message;
                if (ex.InnerException != null)
                {
                    s = ex.InnerException.Message;
                }
                Log.LogMessage("Error Creating Report Excel File.  Error: " + s);
                sb.Clear();
                sb.AppendLine("Error Creating Report Excel File.  Error: " + s);
            }

            return sb.ToString();
        }


    }
}
