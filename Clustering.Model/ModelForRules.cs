using Clustering.Models.GaussianResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clustering.Models
{
    public class ModelForRules
    {
        public Centroids Centroids { get; set; }

        public List<GaussianModel> Gaussians { get; set; }
    }
}
