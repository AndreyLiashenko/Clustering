using Clustering.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clustering.Models.Request
{
    public class VariableWithData
    {
        public List<List<KeyValueCsv>> Data { get; set; }

        public List<LinguisticVariableModel> LinguisticVariables { get; set; }
    }
}
