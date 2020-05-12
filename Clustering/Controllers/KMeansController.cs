﻿using System;
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

        [HttpPost("frontModel")]
        public ActionResult<KMeansResponse> PostModel([FromForm(Name = "file")] IFormFile file, [FromQuery]int numberOfClusters = 3)
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
        /// <returns></returns>
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
                    if(counter == axisNumber)
                    {
                        listOfCenter.Add(point);
                        break;
                    }
                    counter++;
                }
            }

            listOfCenter.Sort();
            double sigma = 0;
            for(int i = 0; i < listOfCenter.Count; i++)
            {
                var model = new GaussianModel();
                if (listOfCenter[i] == listOfCenter.Last())
                {
                    var lasItem = response.Last();
                    model.MathWaiting = listOfCenter[i];
                    model.Sigma = lasItem.Sigma;
                    response.Add(model);
                    break;
                }

                model.MathWaiting = listOfCenter[i];

                sigma = Math.Abs((listOfCenter[i] - listOfCenter[i + 1]) / listOfCenter.Count());
                var sigmaTwoStep = 2 * (sigma * sigma);
                model.Sigma = sigmaTwoStep;
                response.Add(model);
            }

            return Ok(response);
        }

    }
}