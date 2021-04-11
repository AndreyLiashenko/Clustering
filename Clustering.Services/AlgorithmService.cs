using Clustering.Common.Extensions;
using Clustering.Model.Dto;
using Clustering.Models.GaussianResponse;
using Clustering.Models.Request;
using Clustering.Services.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Clustering.Services
{
    public class AlgorithmService : IAlgorithmService
    {
        private readonly ILogger _logger;
        private readonly IGetScvRows _getScvRows;
        private readonly IClusteringService _clusteringService;
        private readonly IGaussParameterService _gaussParameterService;
        private readonly IDataService _dataService;
        private readonly IFuzzifyService _fuzzifyService;

        public AlgorithmService(ILogger<AlgorithmService> logger, IGetScvRows getScvRows, IClusteringService clusteringService,
            IGaussParameterService gaussParameterService, IDataService dataService, IFuzzifyService fuzzifyService)
        {
            _logger = logger;
            _getScvRows = getScvRows;
            _clusteringService = clusteringService;
            _gaussParameterService = gaussParameterService;
            _dataService = dataService;
            _fuzzifyService = fuzzifyService;
        }

        public List<Dictionary<string, string>> FuzzifyByKMeans(ClusteringRequestDto model)
        {
            try
            {
                var lines = _getScvRows.GetLines(model.File);
                var points = lines.Transform();
                var headers = _getScvRows.GetHeaders(model.File);
                var centroids = _clusteringService.ExecKMeansClustering(points, model.NumberOfClusters);
                var listOfLinguisticVariables = new List<LinguisticVariableModel>();

                for (int i = 0; i < headers.Count; i++)
                {
                    var parameters = _gaussParameterService.Get(centroids, i);
                    listOfLinguisticVariables.Add(MapToFuzzifyModel(headers[i], parameters, model.Terms));
                }

                return _fuzzifyService.Get(
                     new VariableWithData { Data = _dataService.MapData(points, headers), LinguisticVariables = listOfLinguisticVariables }
                    );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
        }

        #region private methods
        private LinguisticVariableModel MapToFuzzifyModel(string linguisticVariableName, List<GaussianModel> models, Dictionary<int, string> terms)
        {
            var listOfTerms = new List<Term>();

            for(int i = 0; i < models.Count; i++)
            {
                var term = new Term
                {
                    MathWaiting = models[i].MathWaiting,
                    Sigma = models[i].Sigma,
                    Name = terms[i]
                };
                listOfTerms.Add(term);
            }

            return new LinguisticVariableModel
            {
                Name = linguisticVariableName,
                Terms = listOfTerms
            };
        }
        #endregion

    }
}
