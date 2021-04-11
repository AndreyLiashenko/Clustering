using Clustering.Model.Request;
using Clustering.Services.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clustering.Controllers.Api
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class ClusteringController : Controller
    {
        private readonly ILogger _logger;
        private IAlgorithmService _algorithmService;

        public ClusteringController(ILogger<ClusteringController> logger, IAlgorithmService algorithmService)
        {
            _logger = logger;
            _algorithmService = algorithmService;
        }

        /// <summary>
        /// K-means clustering
        /// </summary>
        /// <param name="request">Object with Number of clusters, terms and file from which the data is read</param>
        /// <returns>Centroids and the point to which cluster it belongs</returns>
        [HttpPost("ExecuteKMeansClustering")]
        public ActionResult<List<Dictionary<string, string>>> ExecuteKMeansClustering([FromForm] ClusteringRequest request)
        {
            return Ok(_algorithmService.FuzzifyByKMeans(request.ToClusteringModel()));
        }
    }
}
