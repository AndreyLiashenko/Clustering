using System.Linq;
using System.Threading.Tasks;
using Clustering.Services.CsvRead;
using Csv;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Clustering.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly IGetScvRows _getScvRows;

        public FilesController(IGetScvRows getScvRows)
        {
            _getScvRows = getScvRows;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromForm(Name = "file")] IFormFile file)
        {
            var lines = _getScvRows.GetLines(file);

           return NoContent();
        }
    }
}