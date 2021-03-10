using Csv;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Clustering.Services.Contracts
{
    public interface IGetScvRows
    {
        List<ICsvLine> GetLines(IFormFile file);

        List<string> GetHeaders(IFormFile file);
    }
}
