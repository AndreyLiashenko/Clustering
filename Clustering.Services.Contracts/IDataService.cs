using Clustering.Models.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Clustering.Services.Contracts
{
    public interface IDataService
    {
        List<List<KeyValueCsv>> MapData(List<List<double>> data, List<string> headers);
    }
}
