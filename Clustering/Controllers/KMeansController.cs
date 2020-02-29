using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Clustering.Algorithms;
using Clustering.Algorithms.KMeans;
using Clustering.Helpers;
using Clustering.ResponseModel;
using Clustering.ResponseModel.KMeansClustering;
using Clustering.Services.CsvRead;
using Csv;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Clustering.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KMeansController : ControllerBase
    {
        private readonly ILogger<KMeansController> _logger;
        private readonly IGetScvRows _getScvRows;
        private int iteration = 0;

        public KMeansController(IGetScvRows getScvRows, ILogger<KMeansController> logger)
        {
            _getScvRows = getScvRows;
            _logger = logger;
        }

        [HttpPost]
        public ActionResult<KMeansResponse> Post([FromForm(Name = "file")] IFormFile file, [FromQuery]int numberOfClusters = 3)
        {
            var lines = _getScvRows.GetLines(file);
            var points = lines.Transform();

            var result = new KMeansResponse();
            var centroid = new Centroids();
            List<List<double>> clusterCenters = AlgorithmsUtils.MakeInitialSeeds(points, numberOfClusters);

            bool stop = false;
            Dictionary<List<double>, List<double>> clusters = null;

            while (!stop)
            {
                _logger.LogInformation($"Iteration = {iteration}");
                iteration++;

                clusters = KMeansAlgorithm.MakeClusters(points, clusterCenters);
                List<List<double>> oldClusterCenters = clusterCenters;
                //recalculete center of clusters
                clusterCenters = KMeansAlgorithm.RecalculateCoordinateOfClusterCenters(clusters, clusterCenters);

                if (ListUtils.IsListEqualsToAnother(clusterCenters, oldClusterCenters))
                {
                    int counter = 1;
                    stop = true;
                    result.Centroids = new Centroids();
                    var list = new List<PointsAndClusterNumber>();
                    foreach (var center in clusterCenters)
                    {
                        var map = clusters.Where(point => ListUtils.IsListEqualsToAnother(point.Value, center));
                        foreach (var item in map)
                        {
                            var pointAndCluster = new PointsAndClusterNumber() { Point = new List<double>() };
                            pointAndCluster.Point = item.Key;
                            pointAndCluster.ClusterNumber = counter;
                            list.Add(pointAndCluster);
                        }
                        counter++;
                    }
                    result.PointsAndClusterNumber = list;
                    result.Centroids.Centroid = clusterCenters;
                }
            }

            return Ok(result);
        }
    }
}