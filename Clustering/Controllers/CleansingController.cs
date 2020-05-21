using Clustering.Helpers;
using Clustering.Services.CsvRead;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ML;
using Microsoft.ML.Transforms;
using System;
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
        public string Post([FromForm(Name = "file")] IFormFile file)
        {
            var request = this.Request;
            var test = _getScvRows.GetLines(file);
            var lines = test.Transform().Select(x => new SampleData
            {
                Id = x[0],
                ProcessFrequency = x[1],
                ThreadsNumber = x[2],
                ConsumpredEnergy = x[3]
            });

            var schema = new DataViewSchema.Builder();
            _data = _mlContext.Data.LoadFromEnumerable(lines);
             
            //https://docs.microsoft.com/en-us/dotnet/machine-learning/how-to-guides/prepare-data-ml-net
            this.FilterData();
            this.ReplaceMissingValue();
            //to do
            this.NormalizeData();



            var enumerable = _mlContext.Data
                .CreateEnumerable<SampleData>(_data,
                reuseRowObject: true)
                .ToList();
            return "posted";
        }

        private void NormalizeData()
        {
            var minMaxEstimator = _mlContext.Transforms.NormalizeMinMax("Price");

            // Fit data to estimator
            // Fitting generates a transformer that applies the operations of defined by estimator
            ITransformer minMaxTransformer = minMaxEstimator.Fit(_data);

            // Transform data
            IDataView transformedData = minMaxTransformer.Transform(_data);
        }

        private void ReplaceMissingValue()
        {
            var replacementEstimator = _mlContext.Transforms.ReplaceMissingValues("ConsumpredEnergy", replacementMode: MissingValueReplacingEstimator.ReplacementMode.Mean);
            _data = replacementEstimator.Fit(_data).Transform(_data);
        }

        private void FilterData()
        {
            _data = _mlContext.Data.FilterRowsByColumn(_data, "ProcessFrequency", lowerBound: 1200, upperBound: 2900);
        }

        public class SampleData
        {
            public double Id { get; set; }
            public double ProcessFrequency { get; set; }
            public double ThreadsNumber { get; set; }
            public double ConsumpredEnergy { get; set; }
        }
    }
}
