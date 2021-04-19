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
                .Select((x, i) => new DataVector { Label = i, Features = x.ToArray() })
                .ToList();

            var schemaDef = SchemaDefinition.Create(typeof(DataVector), SchemaDefinition.Direction.Both);
            using (var stream = file.OpenReadStream())
            {
                //var result = DataSetParserService.GetDataTabletFromCSVFile(stream, true);
                //DataSet ds = new DataSet();
                //ds.Tables.Add(result);
                //DataViewManager dvManager = new DataViewManager(ds);

                int numberOfFeatures = lines.FirstOrDefault().Features.Count();

                schemaDef["Features"].ColumnType = new VectorDataViewType(NumberDataViewType.Double, numberOfFeatures);
                _data = _mlContext.Data.LoadFromEnumerable<DataVector>(lines, schemaDef);
            }

            //https://docs.microsoft.com/en-us/dotnet/machine-learning/how-to-guides/prepare-data-ml-net
            CleanData(parameters);

            using (var stream = file.OpenReadStream())
            {
                var actionUrl = "https://clustering-fuzzy.herokuapp.com/get_dummies/";
                HttpContent fileStreamContent = new StreamContent(stream);
                using (var client = new HttpClient())
                using (var formData = new MultipartFormDataContent())
                {
                    formData.Add(fileStreamContent, "csv", "csv");
                    var response = client.PostAsync(actionUrl, formData).Result;
                    if (!response.IsSuccessStatusCode)
                    {
                        return null;
                    }
                    var res = response.Content.ReadAsStreamAsync().Result;
                    //columnNames = _getScvRows.GetHeaders(ReturnFormFile(res));
                }
            }

            var enumerable = _mlContext.Data
                .CreateEnumerable<DataVector>(_data, reuseRowObject: false, schemaDefinition: schemaDef)
                .ToList();

            
            var result = enumerable.ToCsv();
            var header = string.Join(',', columnNames, 0, columnNames.Count - 1) + columnNames.LastOrDefault() + Environment.NewLine;
            header += result;
            var bytes = Encoding.ASCII.GetBytes(header);
            return new MemoryStream(bytes);
        }

        private IFormFile ReturnFormFile(Stream result)
        {
            var ms = new MemoryStream();
            try
            {
                result.CopyTo(ms);
                return new FormFile(ms, 0, ms.Length, "name", "csv");
            }
            catch (Exception e)
            {
                ms.Dispose();
                throw;
            }
            finally
            {
                ms.Dispose();
            }
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
            var minMaxEstimator = _mlContext.Transforms.NormalizeMinMax("Features");
            _data = minMaxEstimator.Fit(_data).Transform(_data);

            //minMaxEstimator = _mlContext.Transforms.NormalizeMinMax("ThreadsNumber");
            //_data = minMaxEstimator.Fit(_data).Transform(_data);

            //minMaxEstimator = _mlContext.Transforms.NormalizeMinMax("ConsumpredEnergy");
            //_data = minMaxEstimator.Fit(_data).Transform(_data);
        }

        private void ReplaceMissingValue()
        {
            var replacementEstimator = _mlContext.Transforms.ReplaceMissingValues("Features", replacementMode: MissingValueReplacingEstimator.ReplacementMode.Mean);
            _data = replacementEstimator.Fit(_data).Transform(_data);

            //replacementEstimator = _mlContext.Transforms.ReplaceMissingValues("ThreadsNumber", replacementMode: MissingValueReplacingEstimator.ReplacementMode.Mean);
            //_data = replacementEstimator.Fit(_data).Transform(_data);

            //replacementEstimator = _mlContext.Transforms.ReplaceMissingValues("ConsumpredEnergy", replacementMode: MissingValueReplacingEstimator.ReplacementMode.Mean);
            //_data = replacementEstimator.Fit(_data).Transform(_data);
        }

        private void FilterData()
        {
            //_data = _mlContext.Data.FilterRowsByColumn(_data, "ProcessFrequency", lowerBound: 1200, upperBound: 2900);
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
