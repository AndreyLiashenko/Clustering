using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Csv;
using Microsoft.AspNetCore.Http;

namespace Clustering.Services.CsvRead
{
    public class GetCsvRows : IGetScvRows
    {
        public List<ICsvLine> GetLines(IFormFile file)
        {
            using (var stream = file.OpenReadStream())
            {
                var csvLines = CsvReader.ReadFromStream(stream).ToList();
                return csvLines;
            }
        }
    }
}
