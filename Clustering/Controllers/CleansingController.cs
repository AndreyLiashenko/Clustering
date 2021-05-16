using Clustering.Common.Extensions;
using Clustering.Helpers;
using Clustering.Models;
using Clustering.Services.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace Clustering.Controllers
{
    public class CleansingController : ControllerBase
    {
        private readonly IGetScvRows _getScvRows;
        private readonly IHttpClientFactory _clientFactory;
        private readonly MLContext _mlContext;
        private IDataView _data;

        private List<DataVector> _lines;

        public CleansingController(IGetScvRows getScvRows, IHttpClientFactory clientFactory)
        {
            _getScvRows = getScvRows;
            _mlContext = new MLContext();
            _clientFactory = clientFactory;
        }

        [Route("api/cleanse")]
        [HttpPost]
        public MemoryStream Post([FromForm(Name = "file")] IFormFile file, string cleanseParams)
        {
            var request = this.Request;
            var test = _getScvRows.GetLines(file);
            var columnNames = _getScvRows.GetHeaders(file);
            var parameters = !string.IsNullOrEmpty(cleanseParams) ? JsonConvert.DeserializeObject<CleanseParameters>(cleanseParams) : new CleanseParameters();

            var lines = test.TransformRows()
                .Select((x, i) => new DataVector { Label = columnNames[i], Features = x.Select(f => new FeatureType { Value = f }).ToArray() })
                .ToList();
            _lines = lines;

            CleanData(parameters);

            //using (var stream = file.OpenReadStream())
            //{
            //    var actionUrl = "https://clustering-fuzzy.herokuapp.com/get_dummies/";
            //    HttpContent fileStreamContent = new StreamContent(stream);
            //    using (var client = new HttpClient())
            //    using (var formData = new MultipartFormDataContent())
            //    {
            //        formData.Add(fileStreamContent, "csv", "csv");
            //        var response = client.PostAsync(actionUrl, formData).Result;
            //        if (!response.IsSuccessStatusCode)
            //        {
            //            return null;
            //        }
            //        var res = response.Content.ReadAsStreamAsync().Result;
            //        //columnNames = _getScvRows.GetHeaders(ReturnFormFile(res));
            //    }
            //}

            var result = _lines.ToCsv();
            var header = string.Join(',', columnNames.ToArray(), 0, columnNames.Count) + Environment.NewLine;
            header += result;
            var bytes = Encoding.ASCII.GetBytes(header);
            return new MemoryStream(bytes);
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

        private void NormalizeData()
        {
            foreach(var column in _lines)
            {
                var data = _mlContext.Data.LoadFromEnumerable(column.Features);
                var minMaxEstimator = _mlContext.Transforms.NormalizeMinMax("Value");
                data = minMaxEstimator.Fit(data).Transform(data);
                column.Features = _mlContext.Data.CreateEnumerable<FeatureType>(data, reuseRowObject: false).ToArray();
            }
        }

        private void ReplaceMissingValue()
        {
            foreach (var column in _lines)
            {
                var data = _mlContext.Data.LoadFromEnumerable(column.Features);
                var replacementEstimator = _mlContext.Transforms.ReplaceMissingValues("Value", replacementMode: MissingValueReplacingEstimator.ReplacementMode.Mean);
                data = replacementEstimator.Fit(data).Transform(data);
                column.Features = _mlContext.Data.CreateEnumerable<FeatureType>(data, reuseRowObject: false).ToArray();
            }
        }

        private void FilterData()
        {
            //var schemaDef = SchemaDefinition.Create(typeof(DataVector), SchemaDefinition.Direction.Both);
            //using (var stream = file.OpenReadStream())
            //{
            //var result = DataSetParserService.GetDataTabletFromCSVFile(stream, true);
            //DataSet ds = new DataSet();
            //ds.Tables.Add(result);
            //DataViewManager dvManager = new DataViewManager(ds);

            //int numberOfFeatures = lines.FirstOrDefault().Features.Count();

            //schemaDef["Features"].ColumnType = new VectorDataViewType(NumberDataViewType.Double, numberOfFeatures);
            //_data = _mlContext.Data.LoadFromEnumerable<DataVector>(lines, schemaDef);
            //}

            //https://docs.microsoft.com/en-us/dotnet/machine-learning/how-to-guides/prepare-data-ml-net
            //_data = _mlContext.Data.FilterRowsByColumn(_data, "ProcessFrequency", lowerBound: 1200, upperBound: 2900);

            //var enumerable = _mlContext.Data
            //    .CreateEnumerable<DataVector>(_data, reuseRowObject: false, schemaDefinition: schemaDef)
            //    .ToList();
        }

        private void RemoveDublicates()
        {
        }

        public class CleanseQuery
        {
            public IFormFile file { get; set; }
            public CleanseParameters cleanseParams { get; set; }
        }

        private void InitializeParams()
        {

        }
    }
}
