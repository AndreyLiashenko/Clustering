using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Syncfusion.XlsIO;

namespace Clustering.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<string>> Post([FromForm(Name = "file")] IFormFile file)
        {
            var stream = file.OpenReadStream();

            ExcelEngine excelEngine = new ExcelEngine();

            IApplication application = excelEngine.Excel;

            IWorkbook workbook = application.Workbooks.Open(stream);

            IWorksheet worksheet = workbook.Worksheets[0];

            int NumberOfTheRows = worksheet.Rows.Count();
            int NumberOfTheColums = worksheet.Columns.Count();

            List<string> Elements = new List<string>();

            List<double> result = new List<double>();

            for (int row = 2; row <= NumberOfTheRows; row++)
            {
                for (int col = 2; col <= NumberOfTheColums; col++)
                {
                    Elements.Add(string.Format("{0: 0.0}", worksheet.GetValueRowCol(row, col)));
                    result = Elements.Select(x => double.Parse(x)).ToList();
                }
            }

            return NoContent();
        }
    }
}