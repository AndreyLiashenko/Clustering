using Csv;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clustering.Services.CsvRead
{
    public interface IProcessCsvRows
    {
        List<ICsvLine> GetLines(IFormFile file);
        //void WriteLines(IFormFile file, List<List<double?>> points, IEnumerable<string> headers);
    }
}
