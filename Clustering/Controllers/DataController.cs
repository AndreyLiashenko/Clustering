using Clustering.Helpers;
using Clustering.ResponseModel.Data;
using Clustering.Services.CsvRead;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clustering.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IGetScvRows _getScvRows;

        public DataController(IGetScvRows getScvRows, ILogger<DataController> logger)
        {
            _getScvRows = getScvRows;
            _logger = logger;
        }

        [HttpPost("mapData")]
        public ActionResult<List<List<KeyValueCsv>>> MapData(IFormFile file)
        {
            var result = new List<List<KeyValueCsv>>();
            var lines = _getScvRows.GetLines(file);
            var points = lines.Transform();
            var headers = _getScvRows.GetHeaders(file);

            foreach(var line in points)
            {
                var listOfKeyValue = new List<KeyValueCsv>();
                for (int i = 0; i < line.Count; i++)
                {
                    var keyValue = new KeyValueCsv();
                    keyValue.Value = line[i];
                    keyValue.Key = headers[i];
                    listOfKeyValue.Add(keyValue);
                }
                result.Add(listOfKeyValue);
            }

            return Ok(result);
        }
    }
}
