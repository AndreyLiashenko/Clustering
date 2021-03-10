using Clustering.Models.Data;
using Clustering.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Clustering.Services
{
    public class DataService : IDataService
    {
        public DataService()
        {
        }

        public List<List<KeyValueCsv>> MapData(List<List<double>> data, List<string> headers)
        {
            var result = new List<List<KeyValueCsv>>();

            foreach (var line in data)
            {
                var listOfKeyValue = new List<KeyValueCsv>();
                for (int i = 0; i < line.Count; i++)
                {
                    var keyValue = new KeyValueCsv
                    {
                        Value = line[i],
                        Key = headers[i]
                    };
                    listOfKeyValue.Add(keyValue);
                }
                result.Add(listOfKeyValue);
            }

            return result;
        }
    }
}
