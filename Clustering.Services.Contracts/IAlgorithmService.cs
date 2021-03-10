using Clustering.Model.Dto;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Clustering.Services.Contracts
{
    public interface IAlgorithmService
    {
        List<Dictionary<string, string>> FuzzifyByKMeans(ClusteringRequestDto model);
    }
}
