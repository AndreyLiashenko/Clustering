using Microsoft.AspNetCore.Http;

using System;
using System.Collections.Generic;
using System.Text;

namespace Clustering.Model.Request
{
    public class NormalizationRequest
    {
        public IFormFile File { get; set; }
    }
}