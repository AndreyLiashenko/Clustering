using Clustering.Models;
using NotVisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Clustering.Helpers
{
    public static class DataSetParserService
    {
        public static DataTable GetDataTabletFromCSVFile(Stream stream, bool isHeadings)
        {
            DataTable MethodResult = null;
            try
            {
                using (var TextFieldParser = new CsvTextFieldParser(stream))
                {
                    if (isHeadings)
                    {
                        MethodResult = GetDataTableFromTextFieldParser(TextFieldParser);

                    }
                    else
                    {
                        MethodResult = GetDataTableFromTextFieldParserNoHeadings(TextFieldParser);

                    }

                }

            }
            catch (Exception ex)
            {
                //ex.HandleException();
            }
            return MethodResult;
        }

        public static DataTable GetDataTableFromCsvString(string csvBody, bool isHeadings)
        {
            DataTable MethodResult = null;
            try
            {
                MemoryStream MemoryStream = new MemoryStream();


                StreamWriter StreamWriter = new StreamWriter(MemoryStream);

                StreamWriter.Write(csvBody);

                StreamWriter.Flush();


                MemoryStream.Position = 0;


                using (CsvTextFieldParser TextFieldParser = new CsvTextFieldParser(MemoryStream))
                {
                    if (isHeadings)
                    {
                        MethodResult = GetDataTableFromTextFieldParser(TextFieldParser);

                    }
                    else
                    {
                        MethodResult = GetDataTableFromTextFieldParserNoHeadings(TextFieldParser);

                    }

                }

            }
            catch (Exception ex)
            {
                //ex.HandleException();
            }
            return MethodResult;
        }

        public static DataTable GetDataTableFromRemoteCsv(string url, bool isHeadings)
        {
            DataTable MethodResult = null;
            try
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                StreamReader StreamReader = new StreamReader(httpWebResponse.GetResponseStream());

                using (CsvTextFieldParser TextFieldParser = new CsvTextFieldParser(StreamReader))
                {
                    if (isHeadings)
                    {
                        MethodResult = GetDataTableFromTextFieldParser(TextFieldParser);

                    }
                    else
                    {
                        MethodResult = GetDataTableFromTextFieldParserNoHeadings(TextFieldParser);

                    }

                }

            }
            catch (Exception ex)
            {
                //ex.HandleException();
            }
            return MethodResult;
        }

        public static string ToCsv(this IEnumerable<DataVector> items)
        {
            var csvBuilder = new StringBuilder();
            foreach (var item in items)
            {
                string line = string.Join(",", item.Features.Select(p => p.ToCsvValue()).ToArray());
                csvBuilder.AppendLine(line);
            }
            return csvBuilder.ToString();
        }

        private static string ToCsvValue<T>(this T item)
        {
            if (item == null) return "\"\"";

            if (item is string)
            {
                return string.Format("\"{0}\"", item.ToString().Replace("\"", "\\\""));
            }
            double dummy;
            if (double.TryParse(item.ToString(), out dummy))
            {
                return string.Format("{0}", item);
            }
            return string.Format("\"{0}\"", item);
        }


        private static DataTable GetDataTableFromTextFieldParser(CsvTextFieldParser textFieldParser)
        {
            DataTable MethodResult = null;
            try
            {
                textFieldParser.SetDelimiter(',');

                textFieldParser.HasFieldsEnclosedInQuotes = true;


                string[] ColumnFields = textFieldParser.ReadFields();

                DataTable dt = new DataTable();

                foreach (string ColumnField in ColumnFields)
                {
                    DataColumn DataColumn = new DataColumn(ColumnField);

                    DataColumn.AllowDBNull = true;

                    dt.Columns.Add(DataColumn);

                }


                while (!textFieldParser.EndOfData)
                {
                    string[] Fields = textFieldParser.ReadFields();


                    for (int i = 0; i < Fields.Length; i++)
                    {
                        if (Fields[i] == "")
                        {
                            Fields[i] = null;

                        }

                    }

                    dt.Rows.Add(Fields);

                }

                MethodResult = dt;

            }
            catch (Exception ex)
            {
                //ex.HandleException();
            }
            return MethodResult;
        }

        private static DataTable GetDataTableFromTextFieldParserNoHeadings(CsvTextFieldParser textFieldParser)
        {
            DataTable MethodResult = null;
            try
            {
                textFieldParser.SetDelimiter(',');

                textFieldParser.HasFieldsEnclosedInQuotes = true;

                bool FirstPass = true;

                DataTable dt = new DataTable();

                while (!textFieldParser.EndOfData)
                {
                    string[] Fields = textFieldParser.ReadFields();

                    if (FirstPass)
                    {
                        for (int i = 0; i < Fields.Length; i++)
                        {
                            DataColumn DataColumn = new DataColumn("Column " + i);

                            DataColumn.AllowDBNull = true;

                            dt.Columns.Add(DataColumn);

                        }

                        FirstPass = false;

                    }

                    for (int i = 0; i < Fields.Length; i++)
                    {
                        if (Fields[i] == "")
                        {
                            Fields[i] = null;

                        }

                    }

                    dt.Rows.Add(Fields);

                }

                MethodResult = dt;

            }
            catch (Exception ex)
            {
                //ex.HandleException();
            }
            return MethodResult;
        }
    }
}
