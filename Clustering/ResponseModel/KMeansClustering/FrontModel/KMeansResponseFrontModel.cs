﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clustering.ResponseModel.KMeansClustering.FrontModel
{
    public class KMeansResponseFrontModel
    {
        public Centroids Centroids { get; set; }

        public List<XYZModel> ListOfPoint { get; set; }
    }
}
