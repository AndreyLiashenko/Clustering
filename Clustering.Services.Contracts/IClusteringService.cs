using Clustering.Models;
using Clustering.Models.KMeansClustering;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Clustering.Services.Contracts
{
    public interface IClusteringService
    {
        Centroids ExecKMeansClustering(List<List<double>> data, int numberOfClusters);
    }
}
