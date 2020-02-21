using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Clustering.Algorithms;
using Clustering.Algorithms.KMeans;
using Clustering.Helpers;
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
        public ActionResult Post([FromForm(Name = "file")] IFormFile file, [FromQuery]int numberOfClusters = 3)
        {
            var lines = _getScvRows.GetLines(file);
            var points = lines.Transform();

            List<List<double>> clusterCenters = AlgorithmsUtils.MakeInitialSeeds(points, numberOfClusters);

            bool stop = false;
            Dictionary<List<double>, List<double>> clusters = null;

            while (!stop)
            {
                _logger.LogInformation($"Iteration = {iteration}");

                clusters = KMeansAlgorithm.MakeClusters(points, clusterCenters);
                List<List<double>> oldClusterCenters = clusterCenters;
                //recalculete center of clusters
                clusterCenters = KMeansAlgorithm.RecalculateCoordinateOfClusterCenters(clusters, clusterCenters);

                if (ListUtils.IsListEqualsToAnother(clusterCenters, oldClusterCenters))
                {
                    stop = true;
                }
            }

            return NoContent();
        }
    }
}