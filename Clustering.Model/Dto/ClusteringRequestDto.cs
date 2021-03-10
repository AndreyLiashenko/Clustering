using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Clustering.Model.Dto
{
    public class ClusteringRequestDto
    {
        public IFormFile File { get; set; }

        public int NumberOfClusters { get; set; }

        public Dictionary<int, string> Terms { get; set; }
    }
}
