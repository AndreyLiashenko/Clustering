using Clustering.Model.Dto;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using Clustering.Common.Extensions;

namespace Clustering.Model.Request
{
    public class ClusteringRequest
    {
        public IFormFile File { get; set; }

        public int NumberOfClusters { get; set; }

        public string Terms { get; set; }

        public ClusteringRequestDto ToClusteringModel()
        {
            return new ClusteringRequestDto
            {
                File = File,
                NumberOfClusters = NumberOfClusters,
                Terms = Terms.TransformToDictionary()
            };
        }
    }
}
