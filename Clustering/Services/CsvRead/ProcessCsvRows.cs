using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Clustering.Models.DataModels;
using Csv;
using Microsoft.AspNetCore.Http;

namespace Clustering.Services.CsvRead
{
    public class ProcessCsvRows : IProcessCsvRows
    {
        public List<ICsvLine> GetLines(IFormFile file)
        {
            using (var stream = file.OpenReadStream())
            {
                var csvLines = CsvReader.ReadFromStream(stream).ToList();
                return csvLines;
            }
        }

        //public void WriteLines(IFormFile file, IEnumerable<SampleData> points, IEnumerable<string> headers)
        //{
        //    using (var writer = new StreamWriter()
        //    using (var csvWriter = new CsvWriter(writer))
        //    {
        //        csvWriter.Configuration.Delimiter = ";";
        //        csvWriter.WriteField("Task No");
        //        csvWriter.WriteField("Customer");
        //        csvWriter.NextRecord();
        //        foreach (var project in data)
        //        {
        //            csvWriter.WriteField(project.Code);
        //            csvWriter.NextRecord();
        //        }
        //    }
        //}
    }
}
