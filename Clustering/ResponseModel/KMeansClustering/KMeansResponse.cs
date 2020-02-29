using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clustering.ResponseModel.KMeansClustering
{
    public class KMeansResponse
    {
        public Centroids Centroids { get; set; }

        public List<PointsAndClusterNumber> PointsAndClusterNumber { get; set; }
    }
}
