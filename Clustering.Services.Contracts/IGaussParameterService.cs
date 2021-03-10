using Clustering.Models;
using Clustering.Models.GaussianResponse;
using System;
using System.Collections.Generic;
using System.Text;

namespace Clustering.Services.Contracts
{
    public interface IGaussParameterService
    {
        List<GaussianModel> Get(Centroids centroids, int axisNumber = 0);
    }
}
