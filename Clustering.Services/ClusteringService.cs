using Clustering.Algorithms;
using Clustering.Algorithms.KMeans;
using Clustering.Common.Extensions;
using Clustering.Model.KMeansClustering.FrontModel;
using Clustering.Models;
using Clustering.Models.KMeansClustering;
using Clustering.Services.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Clustering.Services
{
    public class ClusteringService : IClusteringService
    {
        private readonly ILogger _logger;
        private readonly IGetScvRows _getScvRows;
        public ClusteringService(ILogger<ClusteringService> logger, IGetScvRows getScvRows)
        {
            _logger = logger;
            _getScvRows = getScvRows;
        }

        public Centroids ExecKMeansClustering(List<List<double>> data, int numberOfClusters)
        {
            _logger.LogInformation($"NUMBER OF CLUSTER -> {numberOfClusters}");

            var result = new Centroids();
            List<List<double>> clusterCenters = AlgorithmsUtils.MakeInitialSeeds(data, numberOfClusters);

            bool stop = false;
            Dictionary<List<double>, List<double>> clusters = null;

            while (!stop)
            {
                clusters = KMeansAlgorithm.MakeClusters(data, clusterCenters);
                List<List<double>> oldClusterCenters = clusterCenters;
                //recalculete center of clusters
                clusterCenters = KMeansAlgorithm.RecalculateCoordinateOfClusterCenters(clusters, clusterCenters);

                if (ListUtils.IsListEqualsToAnother(clusterCenters, oldClusterCenters))
                {
                    stop = true;
                    result.Centroid = clusterCenters;
                }
            }

            return result;
        }
    }
}
