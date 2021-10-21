using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using System.IO;
using System.Threading.Tasks;

namespace Clustering.Common.API
{
    public class BaseController : ControllerBase
    {

        protected IActionResult BuildExportResponse(Stream stream, string filename)
        {
            var result = File(stream, ResolveContentTypeHeader(filename), filename);

            //This cookie is needed by the jquery.file.download library in order to know when a file download has finished.
            Response.Cookies.Append("fileDownload", "true", new CookieOptions { Path = "/" });
            Response.Headers.Add("filename", filename);
            Response.Headers.Add("Access-Control-Expose-Headers", "*");

            return result;
        }

        protected IActionResult BuildExportResponse(byte[] data, string filename)
        {
            var bytes = data;
            var stream = new MemoryStream(bytes);

            return BuildExportResponse(stream, filename);
        }

        protected async Task<IActionResult> BuildExportResponse(Task<byte[]> data, string filename)
        {
            var bytes = data;
            var stream = new MemoryStream();
            await stream.ReadAsync(await data);

            return BuildExportResponse(stream, filename);
        }

        private static string ResolveContentTypeHeader(string fileName)
        {
            return new FileExtensionContentTypeProvider().TryGetContentType(fileName, out var contentType)
                ? contentType
                : "application/octet-stream";
        }
    }
}
