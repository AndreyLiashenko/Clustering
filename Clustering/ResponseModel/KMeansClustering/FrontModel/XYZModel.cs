using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clustering.ResponseModel.KMeansClustering.FrontModel
{
    public class XYZModel
    {
        public double X { get; set; }

        public double Y { get; set; }

        public double Z { get; set; }

        public int ClusterNumber { get; set; }
    }
}
