using Clustering.Services.Contracts;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clustering.Services
{
    public static class DIRepositoryService
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IGetScvRows, GetCsvRows>();
            services.AddScoped<IAlgorithmService, AlgorithmService>();
            services.AddScoped<IClusteringService, ClusteringService>();
            services.AddScoped<IDataService, DataService>();
            services.AddScoped<IFuzzifyService, FuzzifyService>();
            services.AddScoped<IGaussParameterService, GaussParameterService>();
        }
    }
}
