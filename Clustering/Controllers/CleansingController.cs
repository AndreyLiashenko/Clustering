using Clustering.Helpers;
using Clustering.Models;
using Clustering.Models.DataModels;
using Clustering.Services.CsvRead;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Runtime;
using Microsoft.ML.Transforms;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Clustering.Controllers
{
    public class CleansingController : ControllerBase
    {
        private readonly IGetScvRows _getScvRows;
        private readonly MLContext _mlContext;
        private IDataView _data;

        public CleansingController(IGetScvRows getScvRows)
        {
            _getScvRows = getScvRows;
            _mlContext = new MLContext();
        }

        [Route("api/cleanse")]
        [HttpPost]
        public List<SampleData> Post([FromForm(Name = "file")] IFormFile file, string cleanseParams)
        {
            var request = this.Request;
            var test = _getScvRows.GetLines(file);
            var parameters = !string.IsNullOrEmpty(cleanseParams) ? JsonConvert.DeserializeObject<CleanseParameters>(cleanseParams) : new CleanseParameters();

            var lines = test.Transform().Select(x => new SampleData
            {
                Id = x[0],
                ProcessFrequency = x[1],
                ThreadsNumber = x[2],
                ConsumpredEnergy = x[3]
            });
            //    .Select((x,i) => new DataVector { Label = i, Features = x });
            //var schemaDef = SchemaDefinition.Create(typeof(DataVector));
            //schemaDef["Features"].ColumnType = VectorConverter.
            //_data = _mlContext.Data.LoadFromEnumerable<DataVector>(lines);


            _data = _mlContext.Data.LoadFromEnumerable(lines);
            var headers = _data.Schema;

            //https://docs.microsoft.com/en-us/dotnet/machine-learning/how-to-guides/prepare-data-ml-net
            CleanData(parameters);

            var enumerable = _mlContext.Data
                .CreateEnumerable<SampleData>(_data, reuseRowObject: false)
                .ToList();
            return enumerable;
        }

        private void CleanData(CleanseParameters parameters)
        {
            if (parameters.AutoCleansing)
            {
                InitializeParams();
            }

            if (parameters.FilterData)
                FilterData();
            if (parameters.ReplaceMissingValue)
                ReplaceMissingValue();
            if (parameters.NormalizeData)
                NormalizeData();
            if (parameters.RemoveDublicates)
                RemoveDublicates();
        }

        private void InitializeParams()
        {
            
        }

        private void NormalizeData()
        {
            var minMaxEstimator = _mlContext.Transforms.NormalizeMinMax("ProcessFrequency");
            _data = minMaxEstimator.Fit(_data).Transform(_data);

            minMaxEstimator = _mlContext.Transforms.NormalizeMinMax("ThreadsNumber");
            _data = minMaxEstimator.Fit(_data).Transform(_data);

            minMaxEstimator = _mlContext.Transforms.NormalizeMinMax("ConsumpredEnergy");
            _data = minMaxEstimator.Fit(_data).Transform(_data);
        }

        private void ReplaceMissingValue()
        {
            var replacementEstimator = _mlContext.Transforms.ReplaceMissingValues("ProcessFrequency", replacementMode: MissingValueReplacingEstimator.ReplacementMode.Mean);
            _data = replacementEstimator.Fit(_data).Transform(_data);

            replacementEstimator = _mlContext.Transforms.ReplaceMissingValues("ThreadsNumber", replacementMode: MissingValueReplacingEstimator.ReplacementMode.Mean);
            _data = replacementEstimator.Fit(_data).Transform(_data);

            replacementEstimator = _mlContext.Transforms.ReplaceMissingValues("ConsumpredEnergy", replacementMode: MissingValueReplacingEstimator.ReplacementMode.Mean);
            _data = replacementEstimator.Fit(_data).Transform(_data);
        }

        private void FilterData()
        {
            _data = _mlContext.Data.FilterRowsByColumn(_data, "ProcessFrequency", lowerBound: 1200, upperBound: 2900);
        }

        private void RemoveDublicates()
        {
            //_data = 
        }

        public class DataVector
        {
            public double Label;
            [VectorType(4)]
            public List<double> Features;
        }

        public class CleanseQuery
        {
            public IFormFile file { get; set; }
            public CleanseParameters cleanseParams { get; set; }
        }
    }
}
