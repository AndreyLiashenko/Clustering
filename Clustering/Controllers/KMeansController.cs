using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Clustering.Algorithms;
using Clustering.Algorithms.KMeans;
using Clustering.Helpers;
using Clustering.ResponseModel;
using Clustering.ResponseModel.GaussianResponse;
using Clustering.ResponseModel.KMeansClustering;
using Clustering.ResponseModel.KMeansClustering.FrontModel;
using Clustering.Services.CsvRead;
using Csv;
using FLS;
using FLS.Rules;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Clustering.Controllers
{
    [Produces("application/json")]
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

        /// <summary>
        /// K-means clustering
        /// </summary>
        /// <param name="file">File from which the data is read</param>
        /// <param name="numberOfClusters">Number of clusters</param>
        /// <returns>Centroids and the point to which cluster it belongs</returns>
        [HttpPost]
        public ActionResult<KMeansResponse> Post(/*[FromForm(Name = "file")]*/ IFormFile file, [FromQuery]int numberOfClusters = 3)
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

        /// <summary>
        /// Clustering for front-end. draws a three-dimensional graph. ONLY 3 PROPERTY in csv file.
        /// </summary>
        /// <param name="file">File from which the data is read</param>
        /// <param name="numberOfClusters">Number of clusters</param>
        /// <returns>Centroids and the point to which cluster it belongs</returns>
        [HttpPost("frontModel")]
        public ActionResult<KMeansResponse> PostModel(/*[FromForm(Name = "file")]*/ IFormFile file, [FromQuery]int numberOfClusters = 3)
        {
            var lines = _getScvRows.GetLines(file);
            var points = lines.Transform();
            Console.WriteLine($"NUMBER OF CLUSTER -> {numberOfClusters}");

            var result = new KMeansResponseFrontModel();
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
                    int counter = 0;
                    stop = true;
                    result.Centroids = new Centroids();
                    var list = new List<XYZModel>();
                    foreach (var center in clusterCenters)
                    {
                        var map = clusters.Where(point => ListUtils.IsListEqualsToAnother(point.Value, center));
                        foreach (var item in map)
                        {
                            var xyzModel = new XYZModel();
                            xyzModel.X = item.Key[0];
                            xyzModel.Y = item.Key[1];
                            xyzModel.Z = item.Key[2];
                            xyzModel.ClusterNumber = counter;
                            list.Add(xyzModel);
                        }
                        counter++;
                    }
                    result.ListOfPoint = list;
                    result.Centroids.Centroid = clusterCenters;
                }
            }


            return Ok(result);
        }

        /// <summary>
        /// Method, which get parameters for Gaussin function.
        /// </summary>
        /// <param name="centroids">Object, which have list of cluster.</param>
        /// <param name="axisNumber">Number beetween 0 and 2.</param>
        /// <returns>Math waiting and sigma</returns>
        [HttpPost("getGaussParam")]
        public ActionResult<List<GaussianModel>> Get([FromBody] Centroids centroids, [FromQuery] int axisNumber = 0)
        {
            var listOfCenter = new List<double>();
            var response = new List<GaussianModel>();
            foreach (var cluster in centroids.Centroid)
            {
                int counter = 0;
                foreach (var point in cluster)
                {
                    if (counter == axisNumber)
                    {
                        listOfCenter.Add(point);
                        break;
                    }
                    counter++;
                }
            }

            listOfCenter.Sort();
            double sigma = 0;
            for (int i = 0; i < listOfCenter.Count; i++)
            {
                var model = new GaussianModel();
                if (listOfCenter[i] == listOfCenter.Last())
                {
                    var lasItem = response.Last();
                    model.MathWaiting = listOfCenter[i];
                    model.Sigma = lasItem.Sigma;
                    model.SimpleSigma = lasItem.SimpleSigma;
                    response.Add(model);
                    break;
                }

                model.MathWaiting = listOfCenter[i];

                sigma = Math.Abs((listOfCenter[i] - listOfCenter[i + 1]) / listOfCenter.Count());
                var sigmaTwoStep = 2 * (sigma * sigma);
                model.Sigma = sigmaTwoStep;
                model.SimpleSigma = sigma;
                response.Add(model);
            }

            return Ok(response);
        }

        //[HttpPost("getRules")]
        //public ActionResult Get([FromBody] ModelForRules model)
        //{
        //    Dictionary<string, List<double>> lineSegments = new Dictionary<string, List<double>>();
        //    foreach (var item in model.Gaussians)
        //    {
        //        var points = new List<double>();
        //        var firstPoint = item.MathWaiting - (3 * item.SimpleSigma);
        //        var secondPoint = item.MathWaiting + (3 * item.SimpleSigma);
        //        points.Add(firstPoint);
        //        points.Add(secondPoint);

        //    }
        //    return Ok();
        //}

        //[HttpGet]
        //public ActionResult<double> Get()
        //{
        //    var testVariable = new LinguisticVariable("Test");
        //    var low1 = testVariable.MembershipFunctions.AddGaussian("low", 1984, 30.8867).Fuzzify(2400);
        //    var middle1 = testVariable.MembershipFunctions.AddGaussian("middle1", 2076.8109, 12.7242).Fuzzify(2400);
        //    var high1 = testVariable.MembershipFunctions.AddGaussian("high1", 2114.8327, 12.7242).Fuzzify(2400);

        //    //var power1 = new LinguisticVariable("Power");
        //    //var low1 = power1.MembershipFunctions.AddTriangle("Low", 0, 25, 50).Fuzzify(27);
        //    //var high1 = power1.MembershipFunctions.AddTriangle("High", 25, 50, 75).Fuzzify(27);

        //    var water = new LinguisticVariable("Water");
        //    var cold = water.MembershipFunctions.AddTrapezoid("Cold", 0, 0, 20, 40);
        //    var warm = water.MembershipFunctions.AddTriangle("Warm", 30, 50, 70);
        //    var hot = water.MembershipFunctions.AddTrapezoid("Hot", 50, 80, 100, 100);

        //    var power = new LinguisticVariable("Power");
        //    var low = power.MembershipFunctions.AddTriangle("Low", 0, 25, 50);
        //    var high = power.MembershipFunctions.AddTriangle("High", 25, 50, 75);

        //    IFuzzyEngine fuzzyEngine = new FuzzyEngineFactory().Default();

        //    var rule1 = Rule.If(water.Is(cold).Or(water.Is(warm))).Then(power.Is(high));
        //    var rule2 = Rule.If(water.Is(hot)).Then(power.Is(low));
        //    fuzzyEngine.Rules.Add(rule1, rule2);

        //    var result = fuzzyEngine.Defuzzify(new { water = 60 });
        //    return Ok(result);
        //}

    }
}