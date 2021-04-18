using Clustering.Services.Contracts;
using Csv;
using CsvHelper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace Clustering.Services
{
    public class GetCsvRows : IGetScvRows
    {
        public List<ICsvLine> GetLines(IFormFile file)
        {
            using (var stream = file.OpenReadStream())
            {
                var csvLines = Csv.CsvReader.ReadFromStream(stream).ToList();
                return csvLines;
            }
        }

        public List<string> GetHeaders(IFormFile file)
        {
            var listOfHeaders = new List<string>();
            using (var stream = file.OpenReadStream())
            {
                TextReader tr = new StreamReader(stream);
                using (var csv = new CsvHelper.CsvReader(tr, CultureInfo.InvariantCulture))
                {
                    csv.Configuration.HasHeaderRecord = true;
                    csv.Read();
                    csv.ReadHeader();
                    listOfHeaders = ((CsvFieldReader)((CsvParser)csv.Parser).FieldReader).Context.HeaderRecord.ToList();
                    //string header = ((CsvFieldReader)((CsvParser)csv.Parser).FieldReader).Context.HeaderRecord.Single();
                    //listOfHeaders = header.Split(';').ToList();
                }
            }
            return listOfHeaders;
        }
    }
}
