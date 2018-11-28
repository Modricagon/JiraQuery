using Atlassian.Jira;
using JiraQuery.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;


namespace HelperClasses
{
    public static class Helper
    {
        public static List<T> ToListof<T>(DataTable dt)
        {
            const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;
            var columnNames = dt.Columns.Cast<DataColumn>()
                .Select(c => c.ColumnName)
                .ToList();
            var objectProperties = typeof(T).GetProperties(flags);
            var targetList = dt.AsEnumerable().Select(dataRow =>
            {
                var instanceOfT = Activator.CreateInstance<T>();

                foreach (var properties in objectProperties.Where(properties => columnNames.Contains(properties.Name) && dataRow[properties.Name] != DBNull.Value))
                {
                    properties.SetValue(instanceOfT, dataRow[properties.Name], null);
                }
                return instanceOfT;
            }).ToList();

            return targetList;
        }

        public static DataTable ToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection props =
                TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                table.Columns.Add(prop.Name, prop.PropertyType);  // Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
                table.Columns[i].Caption = prop.DisplayName != "" ? prop.DisplayName.Replace("_", " ") : prop.Name;
            }
            object[] values = new object[props.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = props[i].GetValue(item);  //  ?? DBNull.Value;
                }
                table.Rows.Add(values);
            }
            return table;
        }
    
        public static DataTable ToDataTableWithNulls(List<Issue> data, List<string> ignore = null)
        {
            PropertyDescriptorCollection props =
                TypeDescriptor.GetProperties(typeof(Issue));
            PropertyDescriptorCollection customProps = TypeDescriptor.GetProperties(typeof(CustomFieldValue));

            List<string> customfieldNames = new List<string>();

            DataTable table = new DataTable();
            int numProps = 0;
            int numCustomProps = 0;

            if (ignore == null)
            {
                ignore = new List<string>();
            }
            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                if (!ignore.Contains(prop.Name) && prop.Name != "CustomFields")
                {
                    table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
                    table.Columns[numProps].Caption = prop.DisplayName != "" ? prop.DisplayName.Replace("_", " ") : prop.Name;
                    numProps++;
                }

                if (!ignore.Contains(prop.Name) && prop.Name == "CustomFields")
                {
                    for (int x = 0; x < data.Count; x++)
                    {
                        for (int y = 0; y < data[x].CustomFields.Count; y++)
                        {
                            if (!table.Columns.Contains(data[x].CustomFields.ElementAt(y).Name))
                            {
                                string fieldName = data[x].CustomFields.ElementAt(y).Name;
                                if (!ignore.Contains(fieldName))
                                {
                                    customfieldNames.Add(fieldName);
                                    JiraQuery.Log.LogMessage("Custom Field Added: " + fieldName);
                                    table.Columns.Add(fieldName, Nullable.GetUnderlyingType(fieldName.GetType()) ?? fieldName.GetType());
                                    table.Columns[numProps].Caption = fieldName != "" ? fieldName.Replace("_", " ") : fieldName;
                                    numCustomProps++;
                                    numProps++;
                                }
                            }
                        }
                    }
                }
            }
            object[] values = new object[numProps];
            numProps = 0;
            foreach (Issue item in data)
            {
                for (int i = 0; i < props.Count; i++)  //int i = 0; i < values.Length; i++
                {
                    if (!ignore.Contains(props[i].Name) && props[i].Name != "CustomFields")
                    {
                        values[numProps] = props[i].GetValue(item).ToString();
                        numProps++;
                    }
                    if (!ignore.Contains(props[i].Name) && props[i].Name == "CustomFields")
                    {
                        int index = 0;
                        for (int x = 0; x < numCustomProps; x++)
                        {
                            if (index > item.CustomFields.Count - 1)
                            {
                                continue;
                            }

                            string customfieldName = customfieldNames.ElementAt(x);

                            if (customfieldName == (item.CustomFields.ElementAtOrDefault(index).Name))
                            {
                                values[numProps] = FormatHelper.Format(customfieldName ,item.CustomFields.ElementAt(index).Values.First());
                                index++;
                            }
                            else
                            {
                                values[numProps] = DBNull.Value;
                            }
                            numProps++;
                        }


                    }
                }
                table.Rows.Add(values);
                numProps = 0;
            }
            return table;
        }
    }
}
