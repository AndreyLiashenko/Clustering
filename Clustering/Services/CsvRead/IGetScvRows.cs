using Csv;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clustering.Services.CsvRead
{
    public interface IGetScvRows
    {
        List<ICsvLine> GetLines(IFormFile file);
    }
}
