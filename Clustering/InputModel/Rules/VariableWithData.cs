using Clustering.ResponseModel;
using Clustering.ResponseModel.Data;
using Clustering.ResponseModel.KMeansClustering.FrontModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clustering.InputModel.Rules
{
    public class VariableWithData
    {
        public List<List<KeyValueCsv>> Data { get; set; }

        public List<LinguisticVariableModel> LinguisticVariables { get; set; }
    }
}
